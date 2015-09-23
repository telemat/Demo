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
            //if (IsInDesignMode)
            {
                PointOfInterest = new MapLocationViewModel
                {
                    Label = "ZALANDO Neue Bahnhofstraße 11, 10245 Berlin, Germany",
                    Location = new Geopoint(new BasicGeoposition
                    {
                        Latitude = 52.506984,
                        Longitude = 13.471250
                    })
                };
            }
        }

        public string ServiceToken
            =>
                ""
            ;

        public MapLocationViewModel PointOfInterest { get; }
    }
}