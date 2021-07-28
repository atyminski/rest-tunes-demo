using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model;
using Gevlee.RestTunes.Model.Albums;
using Gevlee.RestTunes.Model.Artists;
using Gevlee.RestTunes.Model.Playlist;
using Gevlee.RestTunes.Model.Tracks;
using Microsoft.AspNetCore.Mvc;

namespace Gevlee.RestTunes.Mapping
{
    public class ResultFactory : IResultFactory
    {
        private readonly IUrlHelper _urlHelper;

        public ResultFactory(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public AlbumResult CreateAlbumResult(Album x)
        {
            var result = AlbumResult.FromEntity(x);
            result.Links.Add(new Link("self", _urlHelper.Link("GetAlbum", new { id = result.AlbumId })));
            result.Links.Add(new Link("tracks",
                _urlHelper.Link("GetAlbumTracks", new { albumId = x.AlbumId })));
            return result;
        }

        public ArtistResult CreateArtistResult(Artist x)
        {
            var artistResult = ArtistResult.FromEntity(x);
            artistResult.Links.Add(new Link("self", _urlHelper.Link("GetArtist", new { id = artistResult.ArtistId })));
            artistResult.Links.Add(new Link("albums",
                _urlHelper.Link("GetArtistAlbums", new { artistId = artistResult.ArtistId })));
            return artistResult;
        }

        private PlaylistResult CreatePlaylistResult(Playlist x)
        {
            var result = PlaylistResult.FromEntity(x);
            result.Links.Add(new Link("self", _urlHelper.Link("GetPlaylist", new { id = result.PlaylistId })));
            //result.Links.Add(new Link("tracks",
            //    _urlHelper.Link("GetPlaylistTracks", new { albumId = x.AlbumId })));
            return result;
        }

        private TrackResult CreateTrackResult(Track x)
        {
            var result = TrackResult.FromEntity(x);
            result.Links.Add(new Link("self", _urlHelper.Link("GetTrack", new { id = x.TrackId })));
            result.Links.Add(new Link("album", _urlHelper.Link("GetAlbum", new { id = x.AlbumId })));
            return result;
        }
    }
}