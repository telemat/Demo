namespace FlickrApp.Contracts.Models
{
    using System;

    public class GeoLocation
    {
        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }
    }
}