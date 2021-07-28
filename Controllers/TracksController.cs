using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model;
using Gevlee.RestTunes.Model.Tracks;
using Gevlee.RestTunes.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gevlee.RestTunes.Controllers
{
    [Route("api/v{version:apiVersion}/tracks")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class TracksController : ControllerBase
    {
        private readonly TunesContext _context;
        private readonly IUrlHelper _urlHelper;

        public TracksController(TunesContext tunesContext, IUrlHelper urlHelper)
        {
            _context = tunesContext;
            _urlHelper = urlHelper;
        }

        [HttpGet("/api/v{version:apiVersion}/albums/{albumId}/tracks", Name = "GetAlbumTracks")]
        [HttpGet(Name = "GetTracks")]
        [ProducesResponseType(typeof(CollectionResult<TrackResult>), 200)]
        public ActionResult<CollectionResult<TrackResult>> GetTracks(int? albumId,
            [FromQuery] PageNavigation pageNavigation)
        {
            IQueryable<Track> baseQuery = _context.Tracks.OrderBy(x => x.Name);
            if (albumId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.AlbumId == albumId);
            }

            var page = Page.FromPageNavigation(pageNavigation, baseQuery.Count);

            var list = baseQuery.Skip(pageNavigation.Skip)
                .Take(pageNavigation.Size)
                .Select(CreateTrackResult).ToList();

            var links = new List<Link>
            {
                new Link("self", _urlHelper.LinkForCurrentRoute(o =>
                {
                    if (albumId.HasValue)
                    {
                        o.albumId = albumId;
                    }

                    o.size = pageNavigation.Size;
                    o.page = pageNavigation.Page;
                }))
            };

            if (!page.IsFirst)
            {
                links.Add(new Link("prev",
                    _urlHelper.LinkForCurrentRoute(o =>
                    {
                        if (albumId.HasValue)
                        {
                            o.albumId = albumId;
                        }

                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page - 1;
                    })));
            }

            if (!page.IsLast)
            {
                links.Add(new Link("next",
                    _urlHelper.LinkForCurrentRoute(o =>
                    {
                        if (albumId.HasValue)
                        {
                            o.albumId = albumId;
                        }

                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page + 1;
                    })));
            }

            return new ActionResult<CollectionResult<TrackResult>>(new CollectionResult<TrackResult>
            {
                Page = page,
                Data = list,
                Links = links
            });
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}", Name = "GetTrack")]
        [ProducesResponseType(typeof(TrackResult), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TrackResult>> GetTrack(long id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
            {
                return NotFound();
            }

            var result = CreateTrackResult(track);

            return result;
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{id}", Name = "GetTrack")]
        [ProducesResponseType(typeof(TrackResult), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TrackResult>> GetTrackV2(long id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
            {
                return NotFound();
            }

            var result = CreateTrackResult(track);
            result.Duration = null;
            return result;
        }

        private TrackResult CreateTrackResult(Track x)
        {
            var result = TrackResult.FromEntity(x);
            result.Links.Add(new Link("self", _urlHelper.Link("GetTrack", new { id = x.TrackId })));
            result.Links.Add(new Link("album", _urlHelper.Link("GetAlbum", new { id = x.AlbumId })));
            result.Links.Add(new Link("artist",
                _urlHelper.Link("GetArtist", new { id = _context.Albums.Find(x.AlbumId).ArtistId })));
            return result;
        }
    }
}