namespace FlickrApp.Providers
{
    #region Imports

    using System;
    using System.Threading;

    #endregion

    public abstract class BaseProvider
    {
        protected readonly SynchronizationContext UIContext;
        protected readonly bool IsInDesignMode;

        protected BaseProvider()
        {
            IsInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;

            if (! IsInDesignMode)
            {
                // it is assumed that providers are instantiated by the main thread (UI)
                UIContext = SynchronizationContext.Current;
            }
        }

        protected void RunOnUIThread(Action action)
        {
            if (IsInDesignMode || (SynchronizationContext.Current == UIContext))
            {
                action();
            }
            else
            {
                UIContext.Post(it => { action(); }, null);
            }
        }
    }
}