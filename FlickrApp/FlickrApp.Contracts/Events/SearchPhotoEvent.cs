namespace FlickrApp.Contracts.Events
{
    #region Imports

    using System;

    #endregion

    public class SearchPhotoEvent
    {
        public object Sender { get; }
        public uint SearchId { get; }
        public string SearchTerm { get; }

        public SearchPhotoEvent(object sender, string searchTerm, uint searchId)
        {
            SearchId = searchId;
            Sender = sender;
            SearchTerm = searchTerm;
        }

        public override string ToString()
        {
            return nameof(SearchPhotoEvent);
        }
    }
}