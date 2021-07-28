using System.ComponentModel.DataAnnotations;

namespace Gevlee.RestTunes.Model.Playlist
{
    public class AddTrackToPlaylist
    {
        [Required]
        public long TrackId { get; set; }
    }
}