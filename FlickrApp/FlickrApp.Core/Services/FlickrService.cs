namespace FlickrApp.Core.Services
{
    #region Imports

    using System.Diagnostics;
    using Contracts;
    using FlickrNet;

    #endregion

    internal class FlickrService
        : IFlickrService
    {
        private Flickr _flickr;

        public void Initialize(string authKey, string secretCode)
        {
            if (_flickr != null)
                return;

            _flickr = new Flickr(authKey);
        }

        public async void Search(string searchStr)
        {
            if (_flickr == null)
                return;

            var photos = await _flickr.PhotosSearchAsync(new PhotoSearchOptions
            {
                Text = searchStr,
                Page = 1,
                PerPage = 10
            });

            Debug.WriteLine(photos.Count);

            foreach (var photo in photos)
            {
                Debug.WriteLine(photo.UserId + "/" + photo.PhotoId + " # " + photo.Title);
            }
        }
    }
}