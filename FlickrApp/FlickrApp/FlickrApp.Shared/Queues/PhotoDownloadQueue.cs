namespace FlickrApp.Providers
{
    #region Imports

    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using Contracts;
    using Contracts.Events;
    using ViewModels;

    #endregion

    public class PhotoDownloadQueue
    {
        #region Singleton

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static PhotoDownloadQueue()
        {
        }

        private PhotoDownloadQueue()
        {
            Initialize();
        }

        public static PhotoDownloadQueue Instance { get; } = new PhotoDownloadQueue();

        #endregion

        private volatile uint _currentSearchRequestId;
        private readonly ConcurrentQueue<PhotoViewModel> _queuePhotoViewModels = new ConcurrentQueue<PhotoViewModel>();

        private void Initialize()
        {
            var messenger = Resolver.Instance.Resolve<IMessengerService>();

            messenger.Register<SearchPhotoEvent>(OnSearch);
            messenger.Register<SearchPhotoResultEvent>(OnSearchResult);
        }

        public PhotoViewModel Get()
        {
            PhotoViewModel vm;
            _queuePhotoViewModels.TryDequeue(out vm);

            return vm;
        }

        private void OnSearchResult(SearchPhotoResultEvent searchResultEvent)
        {
            // ignore the results if it does not belong to the current search
            if (_currentSearchRequestId != searchResultEvent.SearchRequestId)
                return;
            
            foreach (var photoVM in searchResultEvent.Photos.Select(photo => new PhotoViewModel(photo)))
            {
                _queuePhotoViewModels.Enqueue(photoVM);
            }
        }

        private void OnSearch(SearchPhotoEvent searchPhotoEvent)
        {
            if (_currentSearchRequestId != searchPhotoEvent.SearchId)
            {
                // remember the search that is in progress
                _currentSearchRequestId = searchPhotoEvent.SearchId;

                //
                // flush photos from previous search
                //

                //PhotoViewModel vm;
                //do
                //{
                //    vm = null;
                //    _queuePhotoViewModels.TryDequeue(out vm);
                //} while (vm != null);
            }
        }
    }
}