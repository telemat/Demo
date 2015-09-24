namespace FlickrApp.ViewModels
{
    #region Imports

    using Windows.Devices.Geolocation;
    using Extensions;

    #endregion

    public class MapLocationViewModel
        : BaseViewModel
    {
        private const int WrapCharacterCount = 30;

        private string _displayLabel;

        public MapLocationViewModel()
        {
        }

        public MapLocationViewModel(Contracts.Models.GeoLocation location, string label)
        {
            Location = new Geopoint(new BasicGeoposition
            {
                Latitude = location.Latitude,
                Longitude = location.Longtitude
            });

            Label = label;
        }

        public Geopoint Location { get; set; }

        public string Label { get; set; }

        public string DisplayLabel => _displayLabel ?? (_displayLabel = Label.WrapAtSpace(WrapCharacterCount));
    }
}