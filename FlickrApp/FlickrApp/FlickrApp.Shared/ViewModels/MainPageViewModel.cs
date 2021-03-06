﻿namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Common;
    using Contracts;
    using Contracts.Events;

    #endregion

    public class MainPageViewModel
        : BaseViewModel
    {
        private readonly Lazy<ICommand> _cmdAuthenticate;

        public MainPageViewModel()
        {
            AuthenticationKey = "c4e9f03344dc58da787a58c8aaf9b9b5";

            _cmdAuthenticate = new Lazy<ICommand>(() => new RelayCommand(Authenticate));
        }

        public string AuthenticationKey { get; set; }

        public string SecretCode { get; set; }

        public ICommand AuthenticateCommand => _cmdAuthenticate.Value;

        private void Authenticate()
        {
            var flickrService = Resolver.Instance.Resolve<IFlickrService>();
            var messengerService = Resolver.Instance.Resolve<IMessengerService>();

            try
            {
                var isSuccess = flickrService.Initialize(AuthenticationKey, SecretCode);

                messengerService.Notify(new AuthenticationEvent(isSuccess));
            }
            catch (Exception)
            {
                messengerService.Notify(new AuthenticationEvent(false));
            }
        }
    }
}