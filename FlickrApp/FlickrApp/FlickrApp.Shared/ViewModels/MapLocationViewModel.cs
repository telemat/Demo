namespace FlickrApp.ViewModels
{
    #region Imports

    using Windows.Devices.Geolocation;

    #endregion

    public class MapLocationViewModel
        : BaseViewModel
    {
        public Geopoint Location { get; set; }
        public string Label { get; set; }
    }
}