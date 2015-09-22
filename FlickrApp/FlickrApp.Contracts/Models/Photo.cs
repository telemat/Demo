namespace FlickrApp.Contracts.Models
{
    #region Imports

    using PropertyChanged;

    #endregion

    [ImplementPropertyChanged]
    public class Photo
    {
        public string Title { get; set; }

        public string Url { get; set; }
    }
}