namespace FlickrApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using Providers;

    #endregion

    public class PivotPageViewModel
        : BaseViewModel
    {
        public ObservableCollection<PhotoViewModel> Photos => PhotoProvider.Instance.Photos;
    }
}