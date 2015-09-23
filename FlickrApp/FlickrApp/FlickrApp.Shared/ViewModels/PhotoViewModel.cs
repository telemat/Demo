﻿namespace FlickrApp.ViewModels
{
    #region Imports

    using Contracts.Models;
    using Extensions;

    #endregion

    public class PhotoViewModel
        : BaseViewModel
    {
        private const int WrapCharacterCount = 18;

        private string _thumbnailLabel;

        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ImageUrl { get; set; }

        public MapLocationViewModel Location { get; set; }

        public PhotoViewModel()
        {
        }

        public PhotoViewModel(Photo photo)
        {
            Title = photo.Title;
            Description = photo.Description;
            ThumbnailUrl = photo.ThumbnailUrl;
            ImageUrl = photo.ImageUrl;

            if (photo.Location != null)
                Location = new MapLocationViewModel(photo.Location, Title);
        }

        public string ThumbnailLabel => _thumbnailLabel ?? (_thumbnailLabel = Title.WrapAtSpace(WrapCharacterCount, 1));
    }
}