﻿namespace FlickrApp.Core.Services
{
    #region Imports

    using System;
    using Contracts;
    using PubSub;

    #endregion

    internal class MessengerService
        : IMessengerService
    {
        public void Notify<T>(T obj)
        {
            this.Publish(obj);
        }

        public void Register<T>(Action<T> action)
        {
            this.Subscribe(action);
        }

        public void Unregister<T>()
        {
            this.Unsubscribe<T>();
        }
    }
}