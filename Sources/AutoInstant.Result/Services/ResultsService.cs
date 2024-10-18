using AutoInstant.Result.Models;

namespace AutoInstant.Result.Services;

public interface ResultsService
{
    string HalftimeResults(Goal goal);

    string FullTimeResults(Goal goal);
}

public class ResultsServiceImp(
    XmlConvertService xmlConvertService,
    ResourceService resourceService)
    : ResultsService
{
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
}