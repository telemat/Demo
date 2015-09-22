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
        private readonly IMessengerService _messenger;
        private Flickr _flickr;
        private Task _task;

        public FlickrService(IMessengerService messenger)
        {
            _messenger = messenger;
        }

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
                    ImageUrl = photo.DoesLargeExist
                        ? photo.LargeUrl
                        : (photo.DoesMediumExist ? photo.MediumUrl : photo.SquareThumbnailUrl)
                });
            }

            return photoCol;
        }
    }
}