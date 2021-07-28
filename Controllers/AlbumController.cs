using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model;
using Gevlee.RestTunes.Model.Albums;
using Gevlee.RestTunes.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gevlee.RestTunes.Controllers
{
    [Route("api/v{version:apiVersion}/albums")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class AlbumController : Controller
    {
        private readonly TunesContext _context;
        private readonly IUrlHelper _urlHelper;

        public AlbumController(TunesContext tunesContext, IUrlHelper urlHelper)
        {
            _context = tunesContext;
            _urlHelper = urlHelper;
        }

        [HttpGet("/api/v{version:apiVersion}/artists/{artistId}/albums", Name = "GetArtistAlbums")]
        [HttpGet(Name = "GetAlbums")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(typeof(CollectionResult<AlbumResult>), 200)]
        public ActionResult<CollectionResult<AlbumResult>> GetAlbums(int? artistId, [FromQuery] PageNavigation pageNavigation)
        {
            IQueryable<Album> baseQuery = _context.Albums.OrderBy(x => x.Title);
            if (artistId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.AlbumId == artistId);
            }

            var page = Page.FromPageNavigation(pageNavigation, baseQuery.Count);

            var list = baseQuery.Skip(pageNavigation.Skip)
                .Take(pageNavigation.Size)
                .Select(CreateAlbumResult).ToList();

            var links = new List<Link>
            {
                new Link("self", _urlHelper.LinkForCurrentRoute(o =>
                {
                    if (artistId.HasValue)
                    {
                        o.artistId = artistId;
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
                        if (artistId.HasValue)
                        {
                            o.artistId = artistId;
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
                        if (artistId.HasValue)
                        {
                            o.artistId = artistId;
                        }

                        o.size = pageNavigation.Size;
                        o.page = pageNavigation.Page + 1;
                    })));
            }

            return new ActionResult<CollectionResult<AlbumResult>>(new CollectionResult<AlbumResult>
            {
                Page = page,
                Data = list,
                Links = links
            });
        }

        [ProducesResponseType(typeof(AlbumResult), 200)]
        [HttpGet("{id}", Name = "GetAlbum")]
        public async Task<ActionResult<AlbumResult>> GetAlbum(long id)
        {
            var album = CreateAlbumResult(await _context.Albums.FindAsync(id));

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        private AlbumResult CreateAlbumResult(Album x)
        {
            var result = AlbumResult.FromEntity(x);
            result.Links.Add(new Link("self", _urlHelper.Link("GetAlbum", new { id = result.AlbumId })));
            result.Links.Add(new Link("tracks",
                _urlHelper.Link("GetAlbumTracks", new { albumId = x.AlbumId })));
            return result;
        }
    }
}