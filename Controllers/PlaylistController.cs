using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model;
using Gevlee.RestTunes.Model.Playlist;
using Gevlee.RestTunes.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gevlee.RestTunes.Controllers
{
    [Route("v{version:apiVersion}/playlists")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class PlaylistController : ControllerBase
    {
        private readonly TunesContext _context;
        private readonly IUrlHelper _urlHelper;

        public PlaylistController(TunesContext tunesContext, IUrlHelper urlHelper)
        {
            _context = tunesContext;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetPlaylists")]
        [ProducesResponseType(typeof(CollectionResult<PlaylistResult>), 200)]
        public ActionResult<CollectionResult<PlaylistResult>> GetPlaylist([FromQuery] PageNavigation pageNavigation)
        {
            IQueryable<Playlist> baseQuery = _context.Playlists.OrderBy(x => x.Name);

            var page = Page.FromPageNavigation(pageNavigation, baseQuery.Count);

            var list = baseQuery.Skip(pageNavigation.Skip)
                .Take(pageNavigation.Size)
                .Select(CreatePlaylistResult).ToList();

            var links = new List<Link>
            {
                new Link("self", _urlHelper.LinkForCurrentRoute(o =>
                {
                    o.size = pageNavigation.Size;
                    o.page = pageNavigation.Page;
                }))
            };

            if (!page.IsFirst)
            {
                links.Add(new Link("prev",
                    _urlHelper.LinkForCurrentRoute(o =>
                    {
                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page - 1;
                    })));
            }

            if (!page.IsLast)
            {
                links.Add(new Link("next",
                    _urlHelper.LinkForCurrentRoute(o =>
                    {
                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page + 1;
                    })));
            }

            return new ActionResult<CollectionResult<PlaylistResult>>(new CollectionResult<PlaylistResult>
            {
                Page = page,
                Data = list,
                Links = links
            });
        }

        [HttpGet("{id}", Name = "GetPlaylist")]
        [ProducesResponseType(typeof(PlaylistResult), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PlaylistResult>> GetPlaylist(long id)
        {
            var result = await _context.Playlists.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return CreatePlaylistResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlaylistResult), 201)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResult>), 400)]
        public async Task<ActionResult> CreatePlaylist([FromBody] NewPlaylist newPlaylist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState.Select(x => new ValidationErrorResult(x.Key, x.Value.Errors.Select(e => e.ErrorMessage)))
                );
            }

            var entity = _context.Playlists.Add(new Playlist
            {
                Name = newPlaylist.Name
            });

            await _context.SaveChangesAsync();

            return Created(_urlHelper.Link("GetPlaylist", new { id = entity.Entity.PlaylistId }),
                CreatePlaylistResult(entity.Entity));
        }

        [HttpPut("{playlistId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResult>), 400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdatePlaylist(long playlistId, [FromBody] UpdatedPlaylist updatedPlaylist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState.Select(x => new ValidationErrorResult(x.Key, x.Value.Errors.Select(e => e.ErrorMessage)))
                );
            }

            var entity = await _context.Playlists.FindAsync(playlistId);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = updatedPlaylist.Name;

            await _context.SaveChangesAsync();

            return Ok(
                CreatePlaylistResult(entity));
        }

        [HttpDelete("{playlistId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePlaylist(long playlistId)
        {
            var entity = await _context.Playlists.FindAsync(playlistId);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private PlaylistResult CreatePlaylistResult(Playlist x)
        {
            var result = PlaylistResult.FromEntity(x);
            result.Links.Add(new Link("self", _urlHelper.Link("GetPlaylist", new { id = result.PlaylistId })));
            result.Links.Add(new Link("tracks",
                _urlHelper.Link("GetPlaylistTracks", new { playlistId = x.PlaylistId })));
            return result;
        }
    }
}