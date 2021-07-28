using Gevlee.RestTunes.Data;
using Gevlee.RestTunes.Model.Export;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Gevlee.RestTunes.Controllers
{
    [Route("api/v{version:apiVersion}/export")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ExportController : ControllerBase
    {
        private readonly TunesContext _context;

        public ExportController(TunesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArtistExportResult>), 200)]
        public ActionResult<IEnumerable<ArtistExportResult>> GetAllData()
        {
            return _context.Artists.Include(x => x.Albums).ThenInclude(x => x.Tracks)
                .OrderBy(x => x.Name).Select(CreateExportedData).ToList();
        }

        private ArtistExportResult CreateExportedData(Artist x)
        {
            return new ArtistExportResult
            {
                Name = x.Name,
                ArtistId = x.ArtistId,
                Albums = x.Albums.Select(a => new AlbumExportResult
                {
                    AlbumId = a.AlbumId,
                    Title = a.Title,
                    Tracks = a.Tracks.Select(t => new TrackExportResult
                    {
                        Name = t.Name,
                        Composer = t.Composer,
                        TrackId = t.TrackId
                    })
                })
            };
        }
    }
}