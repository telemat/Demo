namespace FlickrApp.Contracts.Events
{
    using System.Collections.Generic;
    using Models;

    public class SearchPhotoResultEvent
    {
        public ICollection<Photo> Photos { get; }

        public SearchPhotoResultEvent(ICollection<Photo> photos)
        {
            Photos = photos;
        } 
    }
}