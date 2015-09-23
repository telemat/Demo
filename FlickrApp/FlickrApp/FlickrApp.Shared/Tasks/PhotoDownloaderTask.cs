namespace FlickrApp.Tasks
{
    #region Imports

    using System;
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

        private const int DefaultPageSize = 10;

        private readonly IMessengerService _messengerService;

        private readonly IFlickrService _flickrService;

        private readonly PhotoSearchOption _searchOption;

        private SearchRequestStruct _searchRequest = new SearchRequestStruct(new object());

        private bool _isSearching;

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

            // listen for these events
            _messengerService.Register<SearchPhotoEvent>(OnSearchPhoto);
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
                _isSearching = true;
            }
            else if (! _isSearching)
                return;

            ++_searchOption.PageNumber;

            var result = _flickrService.SearchAsync(_searchOption).Result;

            // check if this is the last page
            if (result.Count < _searchOption.PageSize)
                _isSearching = false;

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