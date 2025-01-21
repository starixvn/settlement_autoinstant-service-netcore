using AutoInstant.Result.Models;

namespace AutoInstant.Result.Services;

public interface ResultsService
{
    string HalftimeResults(Goal goal);

    string FullTimeResults(Goal goal);

    string Resources();
}

public class ResultsServiceImp(
    XmlConvertService xmlConvertService,
    ResourceService resourceService)
    : ResultsService
{

    private const string HalfTimeResult = "01";

    private const string FullTimeResult = "00";
    public string HalftimeResults(Goal goal)
    {
        var resources = resourceService.Halftime(goal);
        return SerializeObjectToString(resources);
    }

    public string FullTimeResults(Goal goal)
    {
        var resources = resourceService.FullTime(goal);
        return SerializeObjectToString(resources);
    }

    private string SerializeObjectToString(List<XmlResource> resources)
    {
        if (resources.Count > 0)
        {
            var xmlResults = new XmlResults { Resources = resources };
            return xmlConvertService.Serialize(xmlResults);
        }

        return string.Empty;
    }

    public string Resources()
    {
        List<XmlResource> resources = [new() { ResourceId = HalfTimeResult }, new() { ResourceId = FullTimeResult }];
        return SerializeObjectToString(resources);
    }
}