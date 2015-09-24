namespace FlickrApp.Contracts.Models
{
    public class Photo
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ImageUrl { get; set; }

        public GeoLocation Location { get; set; }

        public override string ToString()
        {
            return Id ?? "Uninitialized photo";
        }
    }
}