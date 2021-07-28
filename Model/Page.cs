using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Gevlee.RestTunes.Model
{
    public class Page
    {
        public int Size { get; set; }

        public int TotalElements { get; set; }

        public int TotalPages
        {
            get
            {
                var calculated = (int) Math.Round((TotalElements / (decimal) Size), 0);
                return calculated > 0 ? calculated : 1;
            }
        }

        public int Number { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public bool IsLast => Number == TotalPages;

        [JsonIgnore]
        [XmlIgnore]
        public bool IsFirst => Number <= 1;

        public static Page FromPageNavigation(PageNavigation pageNavigation, Func<int> countElements)
        {
            return new Page
            {
                Size = pageNavigation.Size,
                Number = pageNavigation.Page,
                TotalElements = countElements?.Invoke() ?? 0
            };
        }
    }
}