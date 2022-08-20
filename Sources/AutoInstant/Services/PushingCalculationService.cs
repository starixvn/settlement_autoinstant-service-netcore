using System;
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
            var goal = await GetGoal(matchChecking.GoalRecord, match.MatchId);
            var resourceResult = match.TypeId == SettleType.FirstHalf ? resultsService.HalftimeResults(goal) : resultsService.FullTimeResults(goal);
            var result = await ValidateBeforeExecute(match);
            if (result.ErrorCode == 0)
            {
                await autoInstantResultService.Update(match, false, MatchStatus.Completed);
                var betStatus = GetBetStatus(goal, matchChecking);
                await UpdateMatchInfo(match, resourceResult, betStatus);
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

        private string GetBetStatus(Goal goal, MatchChecking matchChecking)
        {
            var betStatus = matchChecking.BetStatus;
            betStatus = pushingCalculationCriteriaService.RemoveOldValue(betStatus);
            betStatus += pushingCalculationCriteriaService.BetStatusHTFL(matchChecking, goal);
            betStatus += pushingCalculationCriteriaService.BetStatusFL(matchChecking, goal);
            return betStatus;
        }

        private async Task<Goal> GetGoal(string goalRecord, int matchId)
        {
            var ticketInfo = await GetTicketInfo(matchId);
            return new Goal
            {
                GoalRecord = goalRecord,
                IsExistFgNg = ticketInfo.IsExistFgNg,
                IsExistHtFgNg = ticketInfo.IsExistHtFgNg,
                IsExistsFG = ticketInfo.IsExistFG,
                IsExistsHTFG = ticketInfo.IsExistHTFG
            };
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
                IsExistHtFgNg = listTicketInfo.Exists(item => item.IsExistHtFgNg),
                IsExistFG = listTicketInfo.Exists(item => item.IsExistFG),
                IsExistHTFG = listTicketInfo.Exists(item => item.IsExistHTFG)
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
                Mode = Mode(match.TypeId),
                InstantKey = match.InstantKey
            };
            var resultMessage = await scaleOutSettleService.ValidateEventAndTicket(request);

            return new Models.Result
            {
                ErrorCode = resultMessage.ErrorCode,
                ErrorMessage = resultMessage.ErrorMessage
            };
        }

        private static string Mode(int typeId) => typeId == SettleType.FirstHalf ? ModeType.FirstHalf : ModeType.SecondHalf;

        private async Task UpdateMatchInfo(Match match, string resourceResult, string betStatus)
        {
            var requestUpdateInfo = new UpdateMatchInfoRequest
            {
                MatchId = match.MatchId,
                Action = "CAL",
                AdminId = 0,
                ResourceResult = resourceResult,
                IsAuto = true,
                Mode = Mode(match.TypeId),
                BetStatus = betStatus
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