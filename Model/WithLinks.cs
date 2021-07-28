using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gevlee.RestTunes.Model
{
    public class WithLinks
    {
        [JsonProperty("_links")]
        [XmlIgnore]
        public List<Link> Links { get; set; } = new List<Link>();

        [JsonIgnore]
        [XmlArray("Links")]
        [XmlArrayItem("Link")]
        public Link[] LinksArray
        {
            get { return Links.ToArray(); }
            set { Links = new List<Link>(value); }
        }
    }
}