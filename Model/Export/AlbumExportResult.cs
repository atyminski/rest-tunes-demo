using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gevlee.RestTunes.Model.Export
{
    public class AlbumExportResult
    {
        public long AlbumId { get; set; }

        public string Title { get; set; }

        [XmlIgnore]
        public IEnumerable<TrackExportResult> Tracks { get; set; }

        [JsonIgnore]
        [XmlArray("Tracks")]
        [XmlArrayItem("Track")]
        public TrackExportResult[] TracksArray
        {
            get
            {
                return Tracks.ToArray();
            }
            set
            {
                Tracks = value;
            }
        }
    }
}