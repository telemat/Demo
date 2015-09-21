namespace FlickrApp.ViewModels
{
    #region Imports

    using PropertyChanged;

    #endregion

    [ImplementPropertyChanged]
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            AuthenticationKey = "Hello";

            SecretCode = "World";
        }

        public string AuthenticationKey { get; set; }

        public string SecretCode { get; set; }
    }
}