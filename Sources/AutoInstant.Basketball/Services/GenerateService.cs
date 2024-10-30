using AutoInstant.Basketball.Models;
using Fanex.Settlement.Converter.Services;

namespace AutoInstant.Basketball.Services;

public interface GenerateService
{
    int GetDeterminedType(IEnumerable<ResultDetail> resultDetails);

    string CreateResources(IEnumerable<ResultDetail> resultDetails);
}

public class GenerateServiceImp(XmlConvertService xmlConvertService) : GenerateService
{
    public int GetDeterminedType(IEnumerable<ResultDetail> resultDetails)
    {
        const int binary = 2;
        List<string> resourceIds = ["01", "NOT_DEFINE", "00", "Q1", "Q2", "Q3", "Q4", "OT"];
        var bitArray = new bool[8];
        foreach (var resultDetail in resultDetails.Where(resultDetail => resourceIds.IndexOf(resultDetail.ResourceId) > -1))
        {
            bitArray[resourceIds.IndexOf(resultDetail.ResourceId)] = true;
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