using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutoInstant.Result.Models
{
    [XmlRoot("Results")]
    public class XmlResults
    {
        [XmlElement("Resource")]
        public List<XmlResource> Resources { get; set; }
    }
}