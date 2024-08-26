using AutoInstant.Basketball.Models;
using Fanex.Settlement.Converter.Services;

namespace AutoInstant.Basketball.Services;

public interface GenerateService
{
    int GetDeterminedType(IEnumerable<ResultDetail> resultDetails, List<string> templateSources);

    string CreateResources(IEnumerable<ResultDetail> resultDetails);
}

public class GenerateServiceImp : GenerateService
{
    private readonly XmlConvertService xmlConvertService;

    public GenerateServiceImp(XmlConvertService xmlConvertService)
    {
        this.xmlConvertService = xmlConvertService;
    }

    public int GetDeterminedType(IEnumerable<ResultDetail> resultDetails, List<string> templateSources)
    {
        const int binary = 2;
        var bitArray = new bool[templateSources.Count];
        foreach (var resultDetail in resultDetails.Where(resultDetail => templateSources.IndexOf(resultDetail.ResourceId) > -1))
        {
            bitArray[templateSources.IndexOf(resultDetail.ResourceId)] = true;
        }

        var binaryString = string.Join("", bitArray.Reverse().Select(b => b ? "1" : "0"));

        return Convert.ToInt32(binaryString, binary);
    }

    public string CreateResources(IEnumerable<ResultDetail> resultDetails)
    {
        var resources = new Resources
        {
            ResourceItems = resultDetails.Select(r => new ResourceItem
            {
                AwayValue = r.AwayValue.ToString(),
                HomeValue = r.HomeValue.ToString(),
                ResourceId = r.ResourceId
            }).ToList()
        };

        return xmlConvertService.Serialize(resources);
    }
}