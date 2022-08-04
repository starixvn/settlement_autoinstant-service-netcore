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

        public PushingCalculationServiceImp(
            IDynamicRepository repository,
            ResultsService resultsService,
            PushingCalculationCriteriaService pushingCalculationCriteriaService,
            IOptions<AppSettingsKey> appSettingsKey,
            ScaleOutSettleService scaleOutSettleService)
        {
            this.repository = repository;
            this.resultsService = resultsService;
            this.pushingCalculationCriteriaService = pushingCalculationCriteriaService;
            this.appSettingsKey = appSettingsKey.Value;
            this.scaleOutSettleService = scaleOutSettleService;
        }

        public async Task<Models.Result> Push(Match match, MatchChecking matchChecking)
        {
            var resourceResult = GetResource(matchChecking.GoalRecord, match, match.TypeId);
            var criteria = pushingCalculationCriteriaService.CreateCriteria(match);
            criteria.InstantKey = match.InstantKey;
            criteria.ResourceResult = resourceResult;
            criteria.BroadcastSessionId = Guid.NewGuid().ToString();

            var requestChecking = await ValidateBeforeExecute(criteria);
            if (requestChecking.ErrorCode > 0)
            {
                return requestChecking;
            }

            await UpdateMatchInfo(criteria, match.TypeId);
            Execute(criteria);

            return new Models.Result
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMsg
            };
        }

        private void Execute(PushingCalculationCriteria criteria)
        {
            foreach (var server in appSettingsKey.SiteDbServer)
            {
                criteria.DbServer = server;
                repository.Execute(criteria);
            }
        }

        private string GetResource(string goalRecord, Match match, int typeId)
        {
            var ticketInfo = GetTicketInfo(match);
            var goal = new Goal
            {
                GoalRecord = goalRecord,
                IsExistFgNg = ticketInfo.IsExistFgNg,
                IsExistHtFgNg = ticketInfo.IsExistHtFgNg
            };

            return typeId == SettleType.FirstHalf ? resultsService.HalftimeResults(goal)
                              : resultsService.FullTimeResults(goal);
        }

        private MatchTicketChecking GetTicketInfo(Match match)
        {
            var listTicketInfo = new List<MatchTicketChecking>();
            appSettingsKey.SiteDbServer.ForEach(async server =>
           {
               var criteria = new MatchTicketCheckingCriteria
               {
                   MatchId = match.MatchId,
                   DbServer = server
               };
               var ticketInfo = await repository.GetAsync<MatchTicketChecking>(criteria) ?? new MatchTicketChecking();
               listTicketInfo.Add(ticketInfo);
           });
            return new MatchTicketChecking
            {
                IsExistFgNg = listTicketInfo.Exists(item => item.IsExistFgNg),
                IsExistHtFgNg = listTicketInfo.Exists(item => item.IsExistHtFgNg)
            };
        }

        private async Task<Models.Result> ValidateBeforeExecute(PushingCalculationCriteria criteria)
        {
            var request = new EventTicketRequest
            {
                MatchId = criteria.MatchId,
                Action = criteria.Action,
                SpName = criteria.GetSettingKey(),
                IsAuto = true,
                Mode = criteria.Mode
            };
            var resultMessage = await scaleOutSettleService.ValidateEventAndTicket(request);
            return new Models.Result
            {
                ErrorCode = resultMessage.ErrorCode,
                ErrorMessage = resultMessage.ErrorMessage
            };
        }

        private async Task UpdateMatchInfo(PushingCalculationCriteria criteria, int typeId)
        {
            var requestUpdateInfo = new UpdateMatchInfoRequest
            {
                MatchId = criteria.MatchId,
                Action = criteria.Action,
                AdminId = criteria.AdminId,
                ResourceResult = criteria.ResourceResult,
                Mode = criteria.Mode,
                IsAuto = true
            };

            if (typeId == SettleType.FirstHalf)
            {
                requestUpdateInfo.HalfTimeAway = int.TryParse(criteria.AwayScore, out int away) ? away : 0;
                requestUpdateInfo.HalfTimeHome = int.TryParse(criteria.HomeScore, out int home) ? home : 0;
            }
            else
            {
                requestUpdateInfo.FullTimeAway = int.TryParse(criteria.AwayScore, out int away) ? away : 0;
                requestUpdateInfo.FullTimeHome = int.TryParse(criteria.HomeScore, out int home) ? home : 0;
            }

            await scaleOutSettleService.UpdateMatchInfo(requestUpdateInfo);
        }
    }
}