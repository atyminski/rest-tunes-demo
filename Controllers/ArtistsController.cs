using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model;
using Gevlee.RestTunes.Model.Artists;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gevlee.RestTunes.Controllers
{
    [Route("api/v{version:apiVersion}/artists")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ArtistController : ControllerBase
    {
        private readonly TunesContext _context;
        private readonly IUrlHelper _urlHelper;

        public ArtistController(TunesContext context, IUrlHelper urlHelper)
        {
            _context = context;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetArtists")]
        [ProducesResponseType(typeof(CollectionResult<ArtistResult>), 200)]
        public ActionResult<CollectionResult<ArtistResult>> GetArtist([FromQuery] PageNavigation pageNavigation)
        {
            var page = new Page
            {
                Size = pageNavigation.Size,
                Number = pageNavigation.Page,
                TotalElements = _context.Artists.Count()
            };

            var list = _context.Artists.OrderBy(x => x.Name).Skip(pageNavigation.Skip).Take(pageNavigation.Size).ToList()
                .Select(CreateArtistResult).ToList();

            var links = new List<Link>
            {
                new Link("self", _urlHelper.Link("GetArtists", new {size = pageNavigation.Size, page = pageNavigation.Page}))
            };

            if (!page.IsFirst)
            {
                links.Add(new Link("prev",
                    _urlHelper.Link("GetArtists", new { size = pageNavigation.Size, page = pageNavigation.Page - 1 })));
            }

            if (!page.IsLast)
            {
                links.Add(new Link("next",
                    _urlHelper.Link("GetArtists", new { size = pageNavigation.Size, page = pageNavigation.Page + 1 })));
            }

            return new ActionResult<CollectionResult<ArtistResult>>(new CollectionResult<ArtistResult>
            {
                Page = page,
                Data = list,
                Links = links
            });
        }

        [HttpGet("{id}", Name = "GetArtist")]
        [ProducesResponseType(typeof(ArtistResult), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ArtistResult>> GetArtist(long id)
        {
            var artist = CreateArtistResult(await _context.Artists.FindAsync(id));

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        private ArtistResult CreateArtistResult(Artist x)
        {
            var artistResult = ArtistResult.FromEntity(x);
            artistResult.Links.Add(new Link("self", _urlHelper.Link("GetArtist", new { id = artistResult.ArtistId })));
            artistResult.Links.Add(new Link("albums",
                _urlHelper.Link("GetArtistAlbums", new { artistId = artistResult.ArtistId })));
            return artistResult;
        }
    }
}