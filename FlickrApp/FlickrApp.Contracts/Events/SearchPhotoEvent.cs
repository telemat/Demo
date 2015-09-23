namespace FlickrApp.Contracts.Events
{
    using System;

    public class SearchPhotoEvent
    {
        public object Sender { get; }
        public Guid SearchId { get; }
        public string SearchTerm { get; }

        public SearchPhotoEvent(object sender, string searchTerm)
        {
            SearchId = Guid.NewGuid();
            Sender = sender;
            SearchTerm = searchTerm;
        }

        public override string ToString()
        {
            return nameof(SearchPhotoEvent);
        }
    }
}