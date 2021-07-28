using System.ComponentModel.DataAnnotations;

namespace Gevlee.RestTunes.Model.Playlist
{
    public class DeleteTrackFromPlaylist
    {
        [Required]
        public long TrackId { get; set; }
    }
}