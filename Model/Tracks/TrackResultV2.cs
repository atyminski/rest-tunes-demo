using Gevlee.RestTunes.Data;
using System;

namespace Gevlee.RestTunes.Model.Tracks
{
    public class TrackResultV2 : WithLinks
    {
        public long TrackId { get; set; }
        public string Name { get; set; }
        public long? AlbumId { get; set; }

        //public long MediaTypeId { get; set; }
        //public long? GenreId { get; set; }
        //public string Composer { get; set; }
        public TimeSpan? Duration { get; set; }

        //public long? Bytes { get; set; }
        //public string UnitPrice { get; set; }
        public static TrackResultV2 FromEntity(Track track)
        {
            return new TrackResultV2
            {
                AlbumId = track.AlbumId,
                TrackId = track.TrackId,
                Name = track.Name,
                //Composer = track.Composer,
                Duration = TimeSpan.FromMilliseconds(track.Milliseconds)
            };
        }
    }
}