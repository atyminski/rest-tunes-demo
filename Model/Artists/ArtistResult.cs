using Gevlee.RestTunes.Data;

namespace Gevlee.RestTunes.Model.Artists
{
    public class ArtistResult : WithLinks
    {
        public long ArtistId { get; set; }

        public string Name { get; set; }

        public static ArtistResult FromEntity(Artist artist)
        {
            return new ArtistResult
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name
            };
        }
    }
}