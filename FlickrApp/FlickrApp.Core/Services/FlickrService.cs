namespace FlickrApp.Core.Services
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Models;
    using FlickrNet;
    using Photo = Contracts.Models.Photo;

    #endregion

    internal class FlickrService
        : IFlickrService
    {
        private Flickr _flickr;

        public bool Initialize(string authKey, string secretCode)
        {
            if (_flickr != null)
                return true;

            try
            {
                _flickr = new Flickr(authKey);
            }
            catch (Exception ex)
            {
                Debug.Assert(false);
            }

            return _flickr != null;
        }

        public async Task<Collection<Photo>> SearchAsync(PhotoSearchOption option)
        {
            if (_flickr == null)
                throw new Exception("Flickr service is not initialized");

            var photos = await _flickr.PhotosSearchAsync(new PhotoSearchOptions
            {
                Text = option.SearchTerm,
                Page = option.PageNumber,
                PerPage = option.PageSize
            });

            var photoCol = new Collection<Photo>();

            foreach (var photo in photos)
            {
                photoCol.Add(new Photo
                {
                    Title = photo.Title,
                    Description = photo.Description,
                    ThumbnailUrl = photo.SquareThumbnailUrl,
                    ImageUrl = SelectImageUrl(photo),
                    Location = GetLocation(photo)
                });
            }

            return photoCol;
        }

        private static string SelectImageUrl(FlickrNet.Photo photo)
        {
            if (! string.IsNullOrEmpty(photo.Medium800Url))
                return photo.Medium800Url;

            if (! string.IsNullOrEmpty(photo.LargeUrl))
                return photo.Medium800Url;

            if (! string.IsNullOrEmpty(photo.Medium640Url))
                return photo.Medium800Url;

            if (! string.IsNullOrEmpty(photo.MediumUrl))
                return photo.Medium800Url;

            if (! string.IsNullOrEmpty(photo.OriginalUrl))
                return photo.Medium800Url;

            return photo.ThumbnailUrl;
        }

        private static GeoLocation GetLocation(FlickrNet.Photo photo)
        {
            if (double.IsNaN(photo.Latitude) || double.IsNaN(photo.Longitude))
                return default(GeoLocation);

            return new GeoLocation {Latitude = photo.Latitude, Longtitude = photo.Longitude};
        }
    }
}