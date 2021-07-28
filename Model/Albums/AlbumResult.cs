using Gevlee.RestTunes.Data;

namespace Gevlee.RestTunes.Model.Albums
{
    public class AlbumResult : WithLinks
    {
        public long AlbumId { get; set; }

        public string Title { get; set; }

        public static AlbumResult FromEntity(Album entity)
        {
            return new AlbumResult
            {
                AlbumId = entity.AlbumId,
                Title = entity.Title
            };
        }
    }
}