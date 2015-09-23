namespace FlickrApp.Contracts.Events
{
    public class AuthenticationEvent
    {
        public bool IsSuccessful { get; }

        public AuthenticationEvent(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }

        public override string ToString()
        {
            return nameof(AuthenticationEvent);
        }
    }
}