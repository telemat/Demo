namespace FlickrApp.Contracts.Events
{
    public class AuthenticationEvent
    {
        public bool IsSuccessful { get; }

        public AuthenticationEvent(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
}