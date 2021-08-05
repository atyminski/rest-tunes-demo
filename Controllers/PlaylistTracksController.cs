using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model;
using Gevlee.RestTunes.Model.Playlist;
using Gevlee.RestTunes.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Gevlee.RestTunes.Controllers
{
    [Route("v{version:apiVersion}/playlists/{playlistId}/tracks")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class PlaylistTracksController : Controller
    {
        private readonly TunesContext _context;
        private readonly IUrlHelper _urlHelper;

        public PlaylistTracksController(TunesContext tunesContext, IUrlHelper urlHelper)
        {
            _context = tunesContext;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetPlaylistTracks")]
        [ProducesResponseType(typeof(CollectionResult<PlaylistTrackResult>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<CollectionResult<PlaylistTrackResult>> GetPlaylistTracks(long playlistId, [FromQuery] PageNavigation pageNavigation)
        {
            IQueryable<PlaylistTrack> baseQuery = _context.PlaylistTracks
                .Include(x => x.Track)
                .ThenInclude(x => x.Album)
                .ThenInclude(x => x.Artist)
                .Where(x => x.PlaylistId == playlistId);

            var page = Page.FromPageNavigation(pageNavigation, baseQuery.Count);

            var list = baseQuery.Skip(pageNavigation.Skip)
                .Take(pageNavigation.Size)
                .Select(CreatePlaylistTrackResult).ToList();

            var links = new List<Link>
            {
                new Link("self", _urlHelper.LinkForCurrentRoute(o =>
                {
                    o.playlistId = playlistId;
                    o.size = pageNavigation.Size;
                    o.page = pageNavigation.Page;
                }))
            };

            if (!page.IsFirst)
            {
                links.Add(new Link("prev",
                    _urlHelper.LinkForCurrentRoute(o =>
                    {
                        o.playlistId = playlistId;
                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page - 1;
                    })));
            }

            if (!page.IsLast)
            {
                links.Add(new Link("next",
                    _urlHelper.LinkForCurrentRoute(o =>
                    {
                        o.playlistId = playlistId;
                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page + 1;
                    })));
            }

            return new ActionResult<CollectionResult<PlaylistTrackResult>>(new CollectionResult<PlaylistTrackResult>
            {
                Page = page,
                Data = list,
                Links = links
            });
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResult>), 400)]
        [ProducesResponseType(404)]
        public IActionResult AddTrackToPlaylist(long playlistId, [FromBody] AddTrackToPlaylist model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState.Select(x => new ValidationErrorResult(x.Key, x.Value.Errors.Select(e => e.ErrorMessage)))
                );
            }

            var playlistEntity = _context.Playlists.Find(playlistId);
            var trackEntity = _context.Tracks.Find(model.TrackId);

            if (playlistEntity == null || trackEntity == null)
            {
                return NotFound();
            }

            if (_context.PlaylistTracks.Any(x => x.PlaylistId == playlistId && x.TrackId == model.TrackId))
            {
                return Conflict();
            }

            _context.PlaylistTracks.Add(new PlaylistTrack
            {
                Playlist = playlistEntity,
                Track = trackEntity
            });

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTrackFromPlaylist(long playlistId, [FromBody] DeleteTrackFromPlaylist model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState.Select(x => new ValidationErrorResult(x.Key, x.Value.Errors.Select(e => e.ErrorMessage)))
                );
            }

            var playlistTrack =
                _context.PlaylistTracks.FirstOrDefault(x => x.PlaylistId == playlistId && x.TrackId == model.TrackId);

            if (playlistTrack == null)
            {
                return NotFound();
            }

            _context.PlaylistTracks.Remove(playlistTrack);

            _context.SaveChanges();

            return NoContent();
        }

        private PlaylistTrackResult CreatePlaylistTrackResult(PlaylistTrack entity)
        {
            return new PlaylistTrackResult
            {
                Artist = entity.Track.Album.Artist.Name,
                Name = entity.Track.Name,
                Id = entity.TrackId,
                Links = new List<Link>
                {
                    new Link("track", _urlHelper.Link("GetTrack", new { id = entity.TrackId })),
                    new Link("artist", _urlHelper.Link("GetArtist", new { id = entity.Track.Album.ArtistId })),
                    new Link("playlist", _urlHelper.Link("GetPlaylist", new { id = entity.PlaylistId })),
                }
            };
        }
    }
}