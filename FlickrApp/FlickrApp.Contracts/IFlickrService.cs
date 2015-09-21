namespace FlickrApp.Contracts
{
    public interface IFlickrService
    {
        string Initialize(string authKey, string secretCode);
    }
}