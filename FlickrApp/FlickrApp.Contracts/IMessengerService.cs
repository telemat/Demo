namespace FlickrApp.Contracts
{
    #region Imports

    using System;

    #endregion

    public interface IMessengerService
    {
        void Notify<T>(T obj);

        void Register<T>(Action<T> action);

        void Unregister<T>();
    }
}