namespace FlickrApp.Contracts
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Models;

    #endregion

    public interface IFlickrService
    {
        bool Initialize(string authKey, string secretCode);

        Task<Collection<Photo>> SearchAsync(PhotoSearchOption option);
    }
}