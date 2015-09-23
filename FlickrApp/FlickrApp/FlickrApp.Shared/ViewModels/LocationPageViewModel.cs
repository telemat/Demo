namespace FlickrApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Linq;
    using Windows.Devices.Geolocation;

    #endregion

    public class LocationPageViewModel
        : BaseViewModel
    {
        private Geopoint _mapCentre;

        public LocationPageViewModel()
        {
            MapLocations = new ObservableCollection<MapLocationViewModel>();

            //if (IsInDesignMode)
            {
                var vm = new MapLocationViewModel
                {
                    Label = "ZALANDO Neue Bahnhofstraße 11, 10245 Berlin, Germany",
                    Location = new Geopoint(new BasicGeoposition
                    {
                        Latitude = 52.506984,
                        Longitude = 13.471250
                    })
                };

                MapLocations.Add(vm);
            }
        }

        public string ServiceToken
            =>
                ""
            ;

        public Geopoint MapCentre
        {
            get
            {
                if (_mapCentre == null && MapLocations.Any())
                    _mapCentre = MapLocations.First().Location;

                return _mapCentre;
            }
            set { _mapCentre = value; }
        }

        public ObservableCollection<MapLocationViewModel> MapLocations { get; }
    }
}