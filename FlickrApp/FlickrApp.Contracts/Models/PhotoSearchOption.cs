namespace FlickrApp.Contracts.Models
{
    public class PhotoSearchOption
    {
        public string SearchTerm { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}