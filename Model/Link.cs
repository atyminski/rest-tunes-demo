using System.Xml.Serialization;

namespace Gevlee.RestTunes.Model
{
    public class Link
    {
        public Link()
        {
        }

        public Link(string rel, string url)
        {
            Url = url;
            Rel = rel;
        }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("rel")]
        public string Rel { get; set; }
    }
}