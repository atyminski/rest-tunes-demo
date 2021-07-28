namespace Gevlee.RestTunes.Model.Playlist
{
    public class PlaylistResult : WithLinks
    {
        public long PlaylistId { get; set; }
        public string Name { get; set; }

        public static PlaylistResult FromEntity(Data.Playlist track)
        {
            return new PlaylistResult
            {
                Name = track.Name,
                PlaylistId = track.PlaylistId
            };
        }
    }
}