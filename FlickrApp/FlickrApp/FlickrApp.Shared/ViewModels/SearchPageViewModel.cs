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

    #endregion

    public class SearchPageViewModel
        : BaseViewModel
    {
        private readonly Lazy<ICommand> _cmdSearch;

        private readonly Lazy<ICommand> _cmdToggleSearchBar;
        private readonly IMessengerService _messengerService;

        public SearchPageViewModel()
        {
            _messengerService = Resolver.Instance.Resolve<IMessengerService>();

            _cmdSearch = new Lazy<ICommand>(() => new RelayCommand(Search));
            _cmdToggleSearchBar = new Lazy<ICommand>(() => new RelayCommand(ToggleSearchBar));

            SearchTerm = "Mauritius";
        }

        public string SearchTerm { get; set; }
        public bool IsSearchBarVisible { get; private set; }

        public ObservableCollection<PhotoViewModel> Photos => PhotoProvider.Instance.Photos;


        public ICommand SearchCommand => _cmdSearch.Value;

        public ICommand ToggleSearchBarCommand => _cmdToggleSearchBar.Value;


        private void Search()
        {
            //var flickrService = Resolver.Instance.Resolve<IFlickrService>();

            //flickrService.Search(SearchTerm);

            _messengerService.Notify(new SearchPhotoEvent(this, SearchTerm));
        }

        private void ToggleSearchBar()
        {
            IsSearchBarVisible = ! IsSearchBarVisible;
        }
    }
}