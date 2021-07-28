using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Gevlee.RestTunes.Model
{
    public class CollectionResult<T> : WithLinks
    {
        [JsonProperty("items")]
        [XmlIgnore]
        public IEnumerable<T> Data { get; set; }

        [JsonIgnore]
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public T[] DataArray
        {
            get
            {
                return Data.ToArray<T>();
            }
            set
            {
                Data = value;
            }
        }

        public Page Page { get; set; }
    }
}