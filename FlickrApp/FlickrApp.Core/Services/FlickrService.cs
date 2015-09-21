namespace FlickrApp.Core.Services
{
    #region Imports

    using Contracts;

    #endregion

    internal class FlickrService
        : IFlickrService
    {
        public string Initialize(string authKey, string secretCode)
        {
            return "Wazza!!!";
        }
    }
}