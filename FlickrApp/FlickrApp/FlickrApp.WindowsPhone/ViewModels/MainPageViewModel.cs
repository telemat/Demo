namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Windows.Input;
    using Autofac;
    using Common;
    using Contracts;
    using PropertyChanged;

    #endregion

    [ImplementPropertyChanged]
    public class MainPageViewModel
    {
        private readonly Lazy<ICommand> _cmdAuthenticate;

        public MainPageViewModel()
        {
            

            _cmdAuthenticate = new Lazy<ICommand>(() => new RelayCommand(DoIt));
        }        

        public string AuthenticationKey { get; set; }

        public string SecretCode { get; set; }

        public ICommand AuthenticateCommand => _cmdAuthenticate.Value;

        private void DoIt()
        {
            var service = Globals.Container.Resolve<IFlickrService>();

            var x = service.Initialize(AuthenticationKey, SecretCode);

            SecretCode = x;
        }
    }
}