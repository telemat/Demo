namespace FlickrApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Diagnostics;
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

            SearchCommand = new RelayCommand(OnSearch, CanExecuteSearch);
            ToggleSearchBarCommand = new RelayCommand(OnToggleSearchBar);
            ThumbnailTappedCommand = new RelayCommand(OnThumbnailTapped, CanAcceptThumbnailTap);
        }

        #region Properties

        public bool IsSearchInProgress { get; private set; }

        public string SearchTerm { get; set; }

        public bool IsSearchBarVisible { get; private set; }

        public ObservableCollection<PhotoViewModel> Photos => PhotoProvider.Instance.Photos;

        #endregion

        #region Commands

        public ICommand SearchCommand { get; }

        public ICommand ToggleSearchBarCommand { get; }

        public ICommand ThumbnailTappedCommand { get; }

        #endregion

        private bool CanExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                SearchTerm = string.Empty;
                return false;
            }

            return true;
        }

        private void OnSearch()
        {
            IsSearchInProgress = true;
            _messengerService.Notify(new SearchPhotoEvent(this, SearchTerm));
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
            IsSearchInProgress = false; // TODO - should ideally receive message from task
        }

        private void OnToggleSearchBar()
        {
            IsSearchBarVisible = ! IsSearchBarVisible;

            if (IsSearchBarVisible)
                OnSearchBarShown();
            else
                OnSearchBarHidden();
        }

        private void OnThumbnailTapped()
        {
            


        }

        private bool CanAcceptThumbnailTap()
        {
            // ignore tapping if the search is running
            return ! IsSearchInProgress;
        }
    }
}