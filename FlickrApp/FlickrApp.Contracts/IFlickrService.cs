namespace FlickrApp.Contracts
{
    public interface IFlickrService
    {
        void Initialize(string authKey, string secretCode);
    }
}