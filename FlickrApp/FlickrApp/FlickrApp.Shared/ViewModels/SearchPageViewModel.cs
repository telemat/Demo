namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common;
    using Contracts;
    using Contracts.Events;
    using Providers;
    using Tasks;

    #endregion

    public class SearchPageViewModel
        : BaseViewModel
    {
        private readonly PhotoDownloaderTask _photoDownloaderTask;

        private readonly IMessengerService _messengerService;

        private readonly Lazy<ICommand> _cmdSearch;

        public SearchPageViewModel()
        {
            _photoDownloaderTask = new PhotoDownloaderTask();
            _messengerService = Resolver.Instance.Resolve<IMessengerService>();
            _cmdSearch = new Lazy<ICommand>(() => new RelayCommand(Search));

            _photoDownloaderTask.Start();

            SearchTerm = "Mauritius";
        }

        public string SearchTerm { get; set; }

        public ICommand SearchCommand => _cmdSearch.Value;

        public ObservableCollection<PhotoViewModel> Photos => PhotoProvider.Instance.Photos;

        private void Search()
        {
            //var flickrService = Resolver.Instance.Resolve<IFlickrService>();

            //flickrService.Search(SearchTerm);

            _messengerService.Notify(new SearchPhotoEvent(this, SearchTerm));
        }
    }
}