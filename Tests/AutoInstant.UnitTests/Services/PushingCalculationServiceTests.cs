using System.Threading.Tasks;
using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using AutoInstant.Services;
using Fanex.Data.Repository;
using NSubstitute;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    public class PushingCalculationServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly ResultsService resultsService;
        private readonly PushingCalculationCriteriaService pushingCalculationCriteriaService;

        public PushingCalculationServiceTests()
        {
            pushingCalculationCriteriaService = Substitute.For<PushingCalculationCriteriaService>();
            repository = Substitute.For<IDynamicRepository>();
            resultsService = Substitute.For<ResultsService>();
            resultsService.HalftimeResults(Arg.Any<Goal>()).Returns("HalftimeResults");
            resultsService.FullTimeResults(Arg.Any<Goal>()).Returns("FullTimeResults");
        }

        [Fact]
        public async Task Push_MatchIsFirstHalf_InvokeExecuteWithRightCriteria()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.FirstHalf,
            };
            var matchChecking = new MatchChecking();
            pushingCalculationCriteriaService.CreateCriteria(Arg.Any<Match>()).Returns(new PushingCalculationCriteria(new Match()));

            var pushingCalculationService = new PushingCalculationServiceImp(repository, resultsService, pushingCalculationCriteriaService);

            // Act
            var result = await pushingCalculationService.Push(match, matchChecking);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<PushingCalculationCriteria>(criteria =>
                criteria.ResourceResult == "HalftimeResults"
            ));
            Assert.Equal(result.ErrorMessage, string.Empty);
        }

        [Fact]
        public async Task Push_MatchIsSecondHalf_InvokeExecuteWithRightCriteria()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.SecondHalf
            };
            var matchChecking = new MatchChecking();
            pushingCalculationCriteriaService.CreateCriteria(Arg.Any<Match>()).Returns(new PushingCalculationCriteria(new Match()));

            var pushingCalculationService = new PushingCalculationServiceImp(repository, resultsService, pushingCalculationCriteriaService);

            // Act
            var result = await pushingCalculationService.Push(match, matchChecking);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<PushingCalculationCriteria>(criteria =>
                 criteria.ResourceResult == "FullTimeResults"
            ));
            Assert.Equal(result.ErrorMessage, string.Empty);
        }

        [Fact]
        public async Task Push_MatchIsFirstHalfAndExecuteFail_ReturnsFailureResult()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.SecondHalf
            };
            var matchChecking = new MatchChecking();
            var fakeCriteria = new PushingCalculationHalftimeCriteria(match, "")
            {
                ErrCode = 1,
                ErrMsg = "Fail"
            };

            pushingCalculationCriteriaService.CreateCriteria(Arg.Any<Match>(), Arg.Any<string>()).Returns(fakeCriteria);
            var pushingCalculationService = new PushingCalculationServiceImp(repository, resultsService, pushingCalculationCriteriaService);

            // Act
            var result = await pushingCalculationService.Push(match, matchChecking);

            // Assert
            Assert.True(result.ErrorMessage == "Fail");
        }
    }
}