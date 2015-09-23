namespace FlickrApp.ViewModels
{
    #region Imports

    using Windows.Devices.Geolocation;

    #endregion

    public class LocationPageViewModel
        : BaseViewModel
    {
        public LocationPageViewModel()
        {
            if (IsInDesignMode)
            {
                MapLocation = new MapLocationViewModel
                {
                    Label = "Test Location",
                    Location = new Geopoint(new BasicGeoposition
                    {
                        Latitude = 52.5167,
                        Longitude = 13.3833
                    })
                };
            }
        }

        public string ServiceToken
            =>
                ""
            ;

        public MapLocationViewModel MapLocation { get; }
    }
}