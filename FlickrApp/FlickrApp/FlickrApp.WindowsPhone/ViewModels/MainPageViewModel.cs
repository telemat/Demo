namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Windows.Input;
    using Common;
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
            SecretCode = "Hello there!!!";
        }
    }
}