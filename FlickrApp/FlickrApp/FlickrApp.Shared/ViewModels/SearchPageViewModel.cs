namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common;
    using Contracts;
    using Providers;

    #endregion

    public class SearchPageViewModel
        : BaseViewModel
    {
        private readonly Lazy<ICommand> _cmdSearch;

        public SearchPageViewModel()
        {
            _cmdSearch = new Lazy<ICommand>(() => new RelayCommand(Search));

            SearchTerm = "Mauritius";
        }

        public string SearchTerm { get; set; }

        public ICommand SearchCommand => _cmdSearch.Value;

        public ObservableCollection<PhotoViewModel> Photos => PhotoProvider.Instance.Photos;

        private void Search()
        {
            var flickrService = Resolver.Instance.Resolve<IFlickrService>();

            flickrService.Search(SearchTerm);
        }
    }
}