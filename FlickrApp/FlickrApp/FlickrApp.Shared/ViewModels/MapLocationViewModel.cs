namespace FlickrApp.ViewModels
{
    #region Imports

    using Windows.Devices.Geolocation;
    using Extensions;

    #endregion

    public class MapLocationViewModel
        : BaseViewModel
    {
        private string _displayLabel;

        public Geopoint Location { get; set; }
        public string Label { get; set; }

        public string DisplayLabel => _displayLabel ?? (_displayLabel = Label.Wrap(25));
    }
}