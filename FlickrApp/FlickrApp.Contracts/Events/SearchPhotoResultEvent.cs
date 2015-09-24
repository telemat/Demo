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
        public uint SearchRequestId { get; }
        public ICollection<Photo> Photos { get; }

        public SearchPhotoResultEvent(object sender, uint searchRequestId, ICollection<Photo> photos)
        {
            Sender = sender;
            SearchRequestId = searchRequestId;
            Photos = photos;
        }

        public override string ToString()
        {
            return nameof(SearchPhotoResultEvent);
        }
    }
}