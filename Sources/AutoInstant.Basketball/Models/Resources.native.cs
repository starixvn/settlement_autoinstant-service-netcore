using System.Xml.Serialization;

namespace AutoInstant.Basketball.Models;

[XmlRoot("Resources")]
public class Resources
{
    [XmlElement("Resource")]
    public List<ResourceItem> ResourceItems { get; set; }
}