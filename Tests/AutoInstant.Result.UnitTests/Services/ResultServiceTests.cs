using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using Kaden.XmlConverter;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace AutoInstant.Result.UnitTests.Services
{
    public class ResultServiceTests
    {
        private readonly XmlConvertService xmlConvertService;

        private readonly ResourceService resourceService;

        private readonly List<XmlResource> fakeResourceHalftime = new List<XmlResource>
        {
              new XmlResource {
                  ResourceId ="127F", HomeValue = "A", SettlementStatusId=4, ResultStatusId =5
              },
              new XmlResource {
                  ResourceId ="14F", HomeValue = "A", SettlementStatusId=4, ResultStatusId =5
              }
        };

        private readonly List<XmlResource> fakeResourceFullTime = new List<XmlResource>
        {
              new XmlResource {
                  ResourceId ="14F", HomeValue = "A", SettlementStatusId=4, ResultStatusId =5
              }
        };

        public ResultServiceTests()
        {
            xmlConvertService = new XmlConvertServiceImp();
            resourceService = Substitute.For<ResourceService>();
        }

        [Fact]
        public void HalftimeResults_ResourceIsEmptied_ReturnsEmpty()
        {
            // Arrange
            var goal = Arg.Any<Goal>();
            resourceService.Halftime(goal).Returns(new List<XmlResource>());
            var resultsService = new ResultsServiceImp(xmlConvertService, resourceService);

            // Act
            var actual = resultsService.HalftimeResults(goal);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void HalftimeResults_HasResource_ReturnsExpectedResult()
        {
            // Arrange
            var goal = Arg.Any<Goal>();
            resourceService.Halftime(goal).Returns(fakeResourceHalftime);
            var resultsService = new ResultsServiceImp(xmlConvertService, resourceService);

            // Act
            var actualResult = resultsService.HalftimeResults(goal);

            // Assert
            var expectedResult = "<Results><Resource Id=\"127F\" HomeValue=\"A\" SettlementStatusId=\"4\" ResultStatusId=\"5\" /><Resource Id=\"14F\" HomeValue=\"A\" SettlementStatusId=\"4\" ResultStatusId=\"5\" /></Results>";
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void FullTimeResults_ResourceIsEmptied_ReturnsEmpty()
        {
            // Arrange
            var goal = Arg.Any<Goal>();
            resourceService.FullTime(goal).Returns(new List<XmlResource>());
            var resultsService = new ResultsServiceImp(xmlConvertService, resourceService);

            // Act
            var actual = resultsService.FullTimeResults(goal);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void FullTimeResults_HasResource_ReturnsExpectedResult()
        {
            // Arrange
            var goal = Arg.Any<Goal>();
            resourceService.FullTime(goal).Returns(fakeResourceFullTime);
            var resultsService = new ResultsServiceImp(xmlConvertService, resourceService);

            // Act
            var actualResult = resultsService.FullTimeResults(goal);

            // Assert
            const string expectedResult = "<Results><Resource Id=\"14F\" HomeValue=\"A\" SettlementStatusId=\"4\" ResultStatusId=\"5\" /></Results>";
            Assert.Equal(expectedResult, actualResult);
        }
    }
}