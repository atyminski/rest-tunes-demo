namespace Gevlee.RestTunes.Model.Playlist
{
    public class PlaylistTrackResult : WithLinks
    {
        public string Artist { get; set; }

        public string Name { get; set; }

        public long Id { get; set; }
    }
}