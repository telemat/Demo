namespace FlickrApp.Providers
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Diagnostics;
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
            Resolver.Instance.Resolve<IMessengerService>().Register<SearchPhotoResultEvent>(OnSearchResult);
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

        private void OnSearchResult(SearchPhotoResultEvent searchResultEvent)
        {
            var photoViewModels = searchResultEvent.Photos.Select(
                photo => new PhotoViewModel(photo)).ToList();

            RunOnUIThread(() =>
            {
                foreach (var vm in photoViewModels)
                {
                    Debug.WriteLine(vm.Title);
                    Debug.WriteLine(vm.ThumbnailUrl);
                    Debug.WriteLine(vm.ImageUrl);

                    Photos.Add(vm);
                }
            });
        }
    }
}