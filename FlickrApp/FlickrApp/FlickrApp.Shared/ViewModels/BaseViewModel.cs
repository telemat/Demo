namespace FlickrApp.ViewModels
{
    #region Imports

    using PropertyChanged;

    #endregion

    [ImplementPropertyChanged]
    public abstract class BaseViewModel
    {
        protected readonly bool IsInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;        
    }
}