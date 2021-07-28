using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gevlee.RestTunes.Model.Export
{
    public class ArtistExportResult
    {
        public long ArtistId { get; set; }

        public string Name { get; set; }

        [XmlIgnore]
        public IEnumerable<AlbumExportResult> Albums { get; set; }

        [JsonIgnore]
        [XmlArray("Albums")]
        [XmlArrayItem("Album")]
        public AlbumExportResult[] AlbumsArray
        {
            get
            {
                return Albums.ToArray();
            }
            set
            {
                Albums = value;
            }
        }
    }
}