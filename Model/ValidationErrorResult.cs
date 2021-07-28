using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gevlee.RestTunes.Model
{
    public class ValidationErrorResult
    {
        public string Field { get; set; }

        [XmlIgnore]
        public IEnumerable<string> Errors { get; set; }

        [JsonIgnore]
        [XmlArray("Errors")]
        [XmlArrayItem("Error")]
        public string[] ErrorsArray
        {
            get
            {
                return Errors.ToArray();
            }
            set
            {
                Errors = value;
            }
        }

        public ValidationErrorResult()
        {
        }

        public ValidationErrorResult(string field, IEnumerable<string> errors)
        {
            Field = field;
            Errors = errors;
        }
    }
}