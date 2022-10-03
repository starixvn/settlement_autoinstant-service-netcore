using AutoInstant.Result.Models;
using Kaden.XmlConverter;

namespace AutoInstant.Result.Services;

public interface ResultsService
{
    string HalftimeResults(Goal goal);

    string FullTimeResults(Goal goal);
}

public class ResultsServiceImp : ResultsService
{
    private readonly XmlConvertService xmlConvertService;

    private readonly ResourceService resourceService;

    public ResultsServiceImp(
        XmlConvertService xmlConvertService,
        ResourceService resourceService)
    {
        this.xmlConvertService = xmlConvertService;
        this.resourceService = resourceService;
    }

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
        else
        {
            return string.Empty;
        }
    }
}