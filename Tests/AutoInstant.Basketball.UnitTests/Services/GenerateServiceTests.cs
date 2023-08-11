using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;
using Fanex.Settlement.Converter.Services;

namespace AutoInstant.Basketball.UnitTests.Services;

public class GenerateServiceTests
{
    private readonly XmlConvertService xmlConvertService;

    private readonly GenerateService service;

    public GenerateServiceTests()
    {
        xmlConvertService = new XmlConvertServiceImp();
        service = new GenerateServiceImp(xmlConvertService);
    }

    [Theory]
    [InlineData(new[] { "01", "00", "Q3" }, 37)]
    [InlineData(new string[] { }, 0)]
    [InlineData(new[] { "01", "UNKNOWN", "Q2" }, 17)]
    public void GetDeterminedType_Always_ReturnExpectedValue(IEnumerable<string> resourceIds, int expected)
    {
        // Arrange
        var resultDetails = resourceIds.Select(resourceId => new ResultDetail { ResourceId = resourceId }).ToList();

        // Act
        var result = service.GetDeterminedType(resultDetails);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new[] { "01", "Q3" }, "<Resources><Resource Id=\"01\" Home=\"1\" Away=\"1\" /><Resource Id=\"Q3\" Home=\"1\" Away=\"1\" /></Resources>")]
    [InlineData(new string[] { }, "<Resources />")]
    public void CreateResources_Always_ReturnsExpectedResult(IEnumerable<string> resourceIds, string expected)
    {
        // Arrange
        var resultDetails = resourceIds.Select(resourceId => new ResultDetail { ResourceId = resourceId, HomeValue = 1, AwayValue = 1 }).ToList();

        // Act
        var actualResult = service.CreateResources(resultDetails);

        // Assert
        Assert.Equal(expected, actualResult);
    }
}