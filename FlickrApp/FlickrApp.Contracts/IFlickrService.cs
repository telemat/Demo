namespace FlickrApp.Contracts
{
    #region Imports

    using System;
    using Models;

    #endregion

    public interface IFlickrService
    {
        bool Initialize(string authKey, string secretCode);

        bool Search(string searchStr);
    }
}