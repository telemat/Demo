namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Threading;
    using PropertyChanged;

    #endregion

    [ImplementPropertyChanged]
    public abstract class BaseViewModel
    {
        private readonly SynchronizationContext _uiContext;

        protected BaseViewModel()
        {
            // it is assumed that viewmodels are instantiated by the view
            _uiContext = SynchronizationContext.Current;
        }

        protected void RunOnUIThread(Action action)
        {
            if (SynchronizationContext.Current == _uiContext)
            {
                action();
            }
            else
            {
                _uiContext.Post(it => { action(); }, null);
            }
        }
    }
}