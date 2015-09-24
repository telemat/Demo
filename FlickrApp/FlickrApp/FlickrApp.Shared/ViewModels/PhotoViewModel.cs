namespace FlickrApp.ViewModels
{
    #region Imports

    using System.Runtime.InteropServices.ComTypes;
    using Contracts.Models;
    using Extensions;

    #endregion

    public class PhotoViewModel
        : BaseViewModel
    {
        private const int WrapCharacterCount = 17;

        private string _thumbnailLabel;


        public uint SearchRequestId { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ImageUrl { get; set; }

        public MapLocationViewModel Location { get; set; }

        public string ThumbnailLabel => _thumbnailLabel ?? (_thumbnailLabel = Title.WrapAtSpace(WrapCharacterCount, 1));


        public PhotoViewModel()
        {
        }

        public PhotoViewModel(Photo photo)
        {
            SearchRequestId = photo.SearchRequestId;
            Id = photo.Id;
            Title = photo.Title;
            Description = photo.Description;
            ThumbnailUrl = photo.ThumbnailUrl;
            ImageUrl = photo.ImageUrl;

            if (photo.Location != null)
                Location = new MapLocationViewModel(photo.Location, Title);
        }

        public override string ToString()
        {
            return Id ?? "Uninitialized photo";
        }
    }
}