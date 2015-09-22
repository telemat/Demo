namespace FlickrApp.Core.Services
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Events;
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

        public bool Search(string searchStr)
        {
            if (_flickr == null)
                return false;

            _task = Task.Factory.StartNew(() => DoWork(searchStr));

            return true;
        }

        private void DoWork(string str)
        {
            var photos = _flickr.PhotosSearchAsync(new PhotoSearchOptions
            {
                Text = str,
                Page = 1,
                PerPage = 10
            }).Result;

            var photoCol = new Collection<Photo>();

            foreach (var photo in photos)
            {
                photoCol.Add(new Photo
                {
                    Title = photo.Title,
                    Url = photo.SquareThumbnailUrl
                });
            }

            _messenger.Notify(new SearchPhotoResultEvent(photoCol));
        }
    }
}