namespace FlickrApp.ViewModels
{
    #region Imports

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
        private readonly IMessengerService _messengerService;

        public SearchPageViewModel()
        {
            _messengerService = Resolver.Instance.Resolve<IMessengerService>();

            SearchCommand = new RelayCommand(Search, CanExecuteSearch);
            ToggleSearchBarCommand = new RelayCommand(ToggleSearchBar);
        }

        public string SearchTerm { get; set; }
        public bool IsSearchBarVisible { get; private set; }

        public ObservableCollection<PhotoViewModel> Photos => PhotoProvider.Instance.Photos;


        public ICommand SearchCommand { get; }

        public ICommand ToggleSearchBarCommand { get; }


        private bool CanExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                SearchTerm = string.Empty;
                return false;
            }

            return true;
        }

        private void Search()
        {
            _messengerService.Notify(new SearchPhotoEvent(this, SearchTerm));
        }

        private void ToggleSearchBar()
        {
            IsSearchBarVisible = ! IsSearchBarVisible;

            if (IsSearchBarVisible)
                OnSearchBarShown();
            else
                OnSearchBarHidden();
        }

        private void OnSearchBarShown()
        {
        }

        private void OnSearchBarHidden()
        {
            StopSearch();
        }

        private void StopSearch()
        {
            _messengerService.Notify(new SearchPhotoEndEvent(this));
        }
    }
}