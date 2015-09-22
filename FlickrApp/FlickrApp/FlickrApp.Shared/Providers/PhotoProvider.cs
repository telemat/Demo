namespace FlickrApp.Providers
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Contracts;
    using Contracts.Events;
    using ViewModels;

    #endregion

    public class PhotoProvider
        : BaseProvider
    {
        #region Singleton

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static PhotoProvider()
        {
        }

        private PhotoProvider()
        {
            Initialize();

            if (IsInDesignMode)
            {
                ProvideDesignData();
            }
        }

        public static PhotoProvider Instance { get; } = new PhotoProvider();

        #endregion

        private void Initialize()
        {
            var messenger = Resolver.Instance.Resolve<IMessengerService>();
            messenger.Register<SearchPhotoEvent>(OnSearch);
            messenger.Register<SearchPhotoResultEvent>(OnSearchResult);
        }        

        protected void ProvideDesignData()
        {
            Photos.Add(new PhotoViewModel
            {
                Title = "Photo #1",
                ThumbnailUrl = "https://farm1.staticflickr.com/686/21434216860_fe73b9c1fa_s.jpg"
            });
            Photos.Add(new PhotoViewModel
            {
                Title = "Photo #2",
                ThumbnailUrl = "https://farm6.staticflickr.com/5685/21631181921_3772d8d2c8_s.jpg"
            });
            Photos.Add(new PhotoViewModel
            {
                Title = "Photo #3",
                ThumbnailUrl = "https://farm1.staticflickr.com/669/21435308409_56669340d7_s.jpg"
            });
        }

        public ObservableCollection<PhotoViewModel> Photos { get; } = new ObservableCollection<PhotoViewModel>();

        private Guid _currentSearchId = Guid.Empty;

        private void OnSearchResult(SearchPhotoResultEvent searchResultEvent)
        {
            // ignore the results if it does not belong to the current search
            if (_currentSearchId != searchResultEvent.SearchId)
                return;

            var photoViewModels = searchResultEvent.Photos.Select(
                photo => new PhotoViewModel(photo)).ToList();

            RunOnUIThread(() =>
            {
                foreach (var vm in photoViewModels)
                {
                    Photos.Add(vm);
                }
            });
        }

        private void OnSearch(SearchPhotoEvent searchPhotoEvent)
        {
            // remember the search that is in progress
            _currentSearchId = searchPhotoEvent.SearchId;

            if (Photos.Any())
                Photos.Clear();
        }
    }
}