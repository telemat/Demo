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
                "tyJrgR7wkvNGcFBJ0JIo~WmCmkWBU61h3K3xp-IRhyw~AibI8AiSJpNtj9im5Jn-_DYST8AQZyna5Oyh0r5lWvst8ADtafTEP2C81cEgHTio"
            ;

        public MapLocationViewModel PointOfInterest { get; set; }
    }
}