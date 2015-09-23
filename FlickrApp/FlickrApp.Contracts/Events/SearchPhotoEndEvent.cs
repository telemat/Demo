namespace FlickrApp.Contracts.Events
{
    public class SearchPhotoEndEvent
    {
        public object Sender { get; }

        public SearchPhotoEndEvent(object sender)
        {
            Sender = sender;
        }

        public override string ToString()
        {
            return nameof(SearchPhotoEndEvent);
        }
    }
}