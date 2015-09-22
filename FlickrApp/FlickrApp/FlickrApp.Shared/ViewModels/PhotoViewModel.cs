namespace FlickrApp.ViewModels
{
    #region Imports

    using Contracts.Models;

    #endregion

    public class PhotoViewModel
        : BaseViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ImageUrl { get; set; }

        public PhotoViewModel()
        {
        }

        public PhotoViewModel(Photo photo)
        {
            Title = photo.Title;
            Description = photo.Description;
            ThumbnailUrl = photo.ThumbnailUrl;
            ImageUrl = photo.ImageUrl;
        }
    }
}