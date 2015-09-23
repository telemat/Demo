namespace FlickrApp.Contracts.Events
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using Models;

    #endregion

    public class SearchPhotoResultEvent
    {
        public object Sender { get; }
        public Guid SearchId { get; }
        public ICollection<Photo> Photos { get; }

        public SearchPhotoResultEvent(object sender, Guid searchId, ICollection<Photo> photos)
        {
            Sender = sender;
            SearchId = searchId;
            Photos = photos;
        }

        public override string ToString()
        {
            return nameof(SearchPhotoResultEvent);
        }
    }
}