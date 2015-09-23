namespace FlickrApp.Tasks
{
    #region Imports

    using System;
    using System.Threading;
    using Contracts;
    using Contracts.Events;
    using Contracts.Models;

    #endregion

    internal sealed class PhotoDownloaderTask
        : BaseTask
    {
        private struct SearchRequestStruct
        {
            public readonly object SynLock;
            public bool IsUpdated;
            public Guid SearchId;
            public string SearchTerm;

            public SearchRequestStruct(object obj)
            {
                SynLock = obj;
                IsUpdated = false;
                SearchId = Guid.Empty;
                SearchTerm = string.Empty;
            }
        }

        private const int DefaultPageSize = 6;

        private readonly IMessengerService _messengerService;

        private readonly IFlickrService _flickrService;

        private readonly PhotoSearchOption _searchOption;

        private SearchRequestStruct _searchRequest = new SearchRequestStruct(new object());

        private readonly ManualResetEventSlim _isSearching;

        private bool _isDisposed;


        public PhotoDownloaderTask()
            : base(nameof(PhotoDownloaderTask), 500 /* ms */)
        {
            _messengerService = Resolver.Instance.Resolve<IMessengerService>();
            _flickrService = Resolver.Instance.Resolve<IFlickrService>();

            _searchOption = new PhotoSearchOption
            {
                PageNumber = 0,
                PageSize = DefaultPageSize
            };

            _isSearching = new ManualResetEventSlim(false);

            // listen for these events
            _messengerService.Register<SearchPhotoEvent>(OnSearchPhoto);
            _messengerService.Register<SearchPhotoEndEvent>(OnSearchPhotoEnd);
        }

        protected override void DoUsefulWork()
        {
            if (_searchRequest.IsUpdated)
            {
                lock (_searchRequest.SynLock)
                {
                    _searchOption.SearchTerm = _searchRequest.SearchTerm;
                    _searchRequest.SearchTerm = null;
                    _searchRequest.IsUpdated = false;
                }

                _searchOption.PageNumber = 0;
                _isSearching.Set();
            }
            else if (! _isSearching.IsSet)
                return;

            ++_searchOption.PageNumber;

            var result = _flickrService.SearchAsync(_searchOption).Result;

            // check if this is the last page
            if (result.Count < _searchOption.PageSize)
            {
                _isSearching.Reset();
            }

            Guid currentSearchId;

            lock (_searchRequest.SynLock)
            {
                // flush these results if the search has been updated
                if (_searchRequest.IsUpdated)
                    return;

                // deliver the current result
                currentSearchId = _searchRequest.SearchId;
            }

            // notify listeners
            _messengerService.Notify(new SearchPhotoResultEvent(this, currentSearchId, result));
        }

        private void OnSearchPhoto(SearchPhotoEvent searchPhotoEvent)
        {
            lock (_searchRequest.SynLock)
            {
                _searchRequest.SearchTerm = searchPhotoEvent.SearchTerm;
                _searchRequest.SearchId = searchPhotoEvent.SearchId;
                _searchRequest.IsUpdated = true;
            }

            if (! IsRunning)
                Resume();
        }

        private void OnSearchPhotoEnd(SearchPhotoEndEvent obj)
        {
            Pause();
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            // cleanup
            _messengerService.Unregister<SearchPhotoEvent>();

            base.Dispose(disposing);
        }
    }
}