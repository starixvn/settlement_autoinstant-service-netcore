using System.Threading.Tasks;
using AutoInstant.Constants;
using AutoInstant.Models;
using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using Fanex.Data.Repository;

namespace AutoInstant.Services
{
    public interface PushingCalculationService
    {
        Task<Models.Result> Push(Match match, MatchChecking matchChecking);
    }

    public class PushingCalculationServiceImp : PushingCalculationService
    {
        private readonly IDynamicRepository repository;

        private readonly ResultsService resultsService;

        private readonly PushingCalculationCriteriaService pushingCalculationCriteriaService;

        public PushingCalculationServiceImp(
            IDynamicRepository repository,
            ResultsService resultsService,
            PushingCalculationCriteriaService pushingCalculationCriteriaService)
        {
            this.repository = repository;
            this.resultsService = resultsService;
            this.pushingCalculationCriteriaService = pushingCalculationCriteriaService;
        }

        public async Task<Models.Result> Push(Match match, MatchChecking matchChecking)
        {
            var resourceResult = GetResource(matchChecking, match.TypeId);
            var criteria = pushingCalculationCriteriaService.CreateCriteria(match);
            criteria.InstantKey = match.InstantKey;
            criteria.ResourceResult = resourceResult;
            await repository.ExecuteAsync(criteria);

            return new Models.Result
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMsg
            };
        }

        private string GetResource(MatchChecking matchChecking, int typeId)
        {
            var goal = new Goal
            {
                GoalRecord = matchChecking.GoalRecord,
                IsExistFgNg = matchChecking.IsExistFgNg,
                IsExistHtFgNg = matchChecking.IsExistHtFgNg
            };

            return typeId == SettleType.FirstHalf ? resultsService.HalftimeResults(goal)
                              : resultsService.FullTimeResults(goal);
        }
    }
}