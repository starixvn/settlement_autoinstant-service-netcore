using System.Collections.Generic;
using System.Threading.Tasks;
using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using Fanex.Data.Repository;
using Fanex.Settlement.ScaleOut.Request;
using Fanex.Settlement.ScaleOut.Services;
using Microsoft.Extensions.Options;

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

        private readonly AppSettingsKey appSettingsKey;

        private readonly ScaleOutSettleService scaleOutSettleService;

        private readonly AutoInstantResultService autoInstantResultService;

        public PushingCalculationServiceImp(
            IDynamicRepository repository,
            ResultsService resultsService,
            PushingCalculationCriteriaService pushingCalculationCriteriaService,
            IOptions<AppSettingsKey> appSettingsKey,
            ScaleOutSettleService scaleOutSettleService,
            AutoInstantResultService autoInstantResultService)
        {
            this.repository = repository;
            this.resultsService = resultsService;
            this.pushingCalculationCriteriaService = pushingCalculationCriteriaService;
            this.appSettingsKey = appSettingsKey.Value;
            this.scaleOutSettleService = scaleOutSettleService;
            this.autoInstantResultService = autoInstantResultService;
        }

        public async Task<Models.Result> Push(Match match, MatchChecking matchChecking)
        {
            var resourceResult = await GetResource(matchChecking.GoalRecord, match, match.TypeId);
            
            var result = await ValidateBeforeExecute(match);
            if (result.ErrorCode == 0)
            {
                await autoInstantResultService.Update(match, false, MatchStatus.Completed);
                await UpdateMatchInfo(match, resourceResult);
                Execute(match, resourceResult);
            }

            return result;
        }

        private void Execute(Match match, string resourceResult)
        {
            var criteria = pushingCalculationCriteriaService.CreateCriteria(match, resourceResult);

            foreach (var server in appSettingsKey.SiteDbServer)
            {
                criteria.DbServer = server;
                repository.Execute(criteria);
            }
        }

        private async Task<string> GetResource(string goalRecord, Match match, int typeId)
        {
            var ticketInfo = await GetTicketInfo(match.MatchId);
            var goal = new Goal
            {
                GoalRecord = goalRecord,
                IsExistFgNg = ticketInfo.IsExistFgNg,
                IsExistHtFgNg = ticketInfo.IsExistHtFgNg
            };

            return typeId == SettleType.FirstHalf ? resultsService.HalftimeResults(goal)
                              : resultsService.FullTimeResults(goal);
        }

        private async Task<MatchTicketChecking> GetTicketInfo(int matchId)
        {
            var listTicketInfo = new List<MatchTicketChecking>();
            var criteria = new MatchTicketCheckingCriteria { MatchId = matchId };
            foreach (var server in appSettingsKey.SiteDbServer)
            {
                criteria.DbServer = server;
                var item = await repository.GetAsync<MatchTicketChecking>(criteria) ?? new MatchTicketChecking();
                listTicketInfo.Add(item);
            }

            return new MatchTicketChecking
            {
                IsExistFgNg = listTicketInfo.Exists(item => item.IsExistFgNg),
                IsExistHtFgNg = listTicketInfo.Exists(item => item.IsExistHtFgNg)
            };
        }

       

        private async Task<Models.Result> ValidateBeforeExecute(Match match)
        {
            var request = new EventTicketRequest
            {
                MatchId = match.MatchId,
                Action = "CAL",
                SpName = "NAP_Admin_Settlement_1_InstantCalculation_Push",
                IsAuto = true,
                Mode = Mode(match.TypeId)
            };
            var resultMessage = await scaleOutSettleService.ValidateEventAndTicket(request);

            return new Models.Result
            {
                ErrorCode = resultMessage.ErrorCode,
                ErrorMessage = resultMessage.ErrorMessage
            };
        }

        private static string Mode(int typeId) => typeId == SettleType.FirstHalf ? ModeType.FirstHalf : ModeType.SecondHalf;

        private async Task UpdateMatchInfo(Match match, string resourceResult)
        {
            var requestUpdateInfo = new UpdateMatchInfoRequest
            {
                MatchId = match.MatchId,
                Action = "CAL",
                AdminId = 0,
                ResourceResult = resourceResult,
                IsAuto = true,
                Mode = Mode(match.TypeId)
            };

            if (match.TypeId == SettleType.FirstHalf)
            {
                requestUpdateInfo.HalfTimeHome = match.HomeValue;
                requestUpdateInfo.HalfTimeAway = match.AwayValue;
            }
            else
            {
                requestUpdateInfo.FullTimeHome = match.HomeValue;
                requestUpdateInfo.FullTimeAway = match.AwayValue;
            }

            await scaleOutSettleService.UpdateMatchInfo(requestUpdateInfo);
        }
    }
}