using System.ComponentModel.DataAnnotations;

namespace Gevlee.RestTunes.Model.Playlist
{
    public class UpdatedPlaylist
    {
        [Required]
        public string Name { get; set; }
    }
}