using System.ComponentModel.DataAnnotations;

namespace Gevlee.RestTunes.Model.Playlist
{
    public class NewPlaylist
    {
        [Required]
        public string Name { get; set; }
    }
}