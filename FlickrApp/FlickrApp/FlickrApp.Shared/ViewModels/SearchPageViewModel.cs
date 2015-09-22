namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using Common;
    using Contracts;

    #endregion

    public class SearchPageViewModel
        : BaseViewModel
    {
        private readonly Lazy<ICommand> _cmdSearch;

        public SearchPageViewModel()
        {
            _cmdSearch = new Lazy<ICommand>(() => new RelayCommand(Search));
        }

        public string SearchTerm { get; set; }

        public ICommand SearchCommand => _cmdSearch.Value;

        private void Search()
        {
            var flickrService = Resolver.Instance.Resolve<IFlickrService>();

            Debug.WriteLine(SearchTerm);
        }
    }
}