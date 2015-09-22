namespace FlickrApp.Contracts
{
    public interface IFlickrService
    {
        bool Initialize(string authKey, string secretCode);

        void Search(string searchStr);
    }
}