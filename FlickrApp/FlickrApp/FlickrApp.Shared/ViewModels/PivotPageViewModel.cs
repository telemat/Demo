namespace FlickrApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;

    #endregion

    public class PivotPageViewModel
        : BaseViewModel
    {
        public ReadOnlyObservableCollection<PhotoViewModel> Photos { get; set; }

        public PhotoViewModel SelectedItem { get; set; }

        public bool IsAppBarVisible => SelectedItem?.Location != null;
    }
}