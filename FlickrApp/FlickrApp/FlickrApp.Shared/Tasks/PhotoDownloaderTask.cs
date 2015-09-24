namespace FlickrApp.Tasks
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
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
            public uint SearchRequestId;
            public string SearchTerm;

            public SearchRequestStruct(object obj)
            {
                SynLock = obj;
                IsUpdated = false;
                SearchRequestId = 0;
                SearchTerm = string.Empty;
            }
        }

        private const int DefaultPageSize = 5;

        private readonly IMessengerService _messengerService;

        private readonly IFlickrService _flickrService;

        private readonly PhotoSearchOption _searchOption;

        private SearchRequestStruct _searchRequest = new SearchRequestStruct(new object());

        private readonly ManualResetEventSlim _isMorePageAvailable;

        private bool _isDisposed;


        public PhotoDownloaderTask()
            : base(nameof(PhotoDownloaderTask), 250 /* ms */)
        {
            _messengerService = Resolver.Instance.Resolve<IMessengerService>();
            _flickrService = Resolver.Instance.Resolve<IFlickrService>();

            _searchOption = new PhotoSearchOption
            {
                PageNumber = 1,
                PageSize = DefaultPageSize
            };

            _isMorePageAvailable = new ManualResetEventSlim(false);

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
                    _searchOption.SearchRequestId = _searchRequest.SearchRequestId;
                    _searchOption.SearchTerm = _searchRequest.SearchTerm;
                    _searchRequest.SearchTerm = null;
                    _searchRequest.IsUpdated = false;
                }

                _searchOption.PageNumber = 1;
                _isMorePageAvailable.Set();
            }
            else if (! _isMorePageAvailable.IsSet)
                return;

            Collection<Photo> result;

            if (_flickrService.Initialize("c4e9f03344dc58da787a58c8aaf9b9b5", string.Empty))
                result = _flickrService.SearchAsync(_searchOption).Result;
            else
                return;

            ++_searchOption.PageNumber;

            // check if this is the last page
            if (result.Count < _searchOption.PageSize)
            {
                _isMorePageAvailable.Reset();
            }

            uint currentSearchId;

            lock (_searchRequest.SynLock)
            {
                // flush these results if the search has been updated
                if (_searchRequest.IsUpdated)
                    return;

                // deliver the current result
                currentSearchId = _searchRequest.SearchRequestId;
            }

            // notify listeners
            _messengerService.Notify(new SearchPhotoResultEvent(this, currentSearchId, result));
        }

        private void OnSearchPhoto(SearchPhotoEvent searchPhotoEvent)
        {
            // update the search request if it is a new one
            if (_searchRequest.SearchRequestId != searchPhotoEvent.SearchId)
            {
                lock (_searchRequest.SynLock)
                {
                    _searchRequest.SearchRequestId = searchPhotoEvent.SearchId;
                    _searchRequest.SearchTerm = searchPhotoEvent.SearchTerm;
                    _searchRequest.IsUpdated = true;
                }
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