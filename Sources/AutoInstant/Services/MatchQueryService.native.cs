using AutoInstant.Criteria;
using AutoInstant.Models;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoInstant.Services
{
    public interface MatchQueryService
    {
        Task<List<Match>> GetWaitingMatches();

        Task<MatchChecking> GetMatchInfo(Match match);
    }

    public class MatchQueryServiceImp : MatchQueryService
    {
        private readonly IDynamicRepository repository;

        public MatchQueryServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Match>> GetWaitingMatches()
        {
            var criteria = new MatchWaitingCriteria();
            return (await repository.FetchAsync<Match>(criteria)).ToList();
        }

        public async Task<MatchChecking> GetMatchInfo(Match match)
        {
            var criteria = new MatchCheckingCriteria
            {
                MatchId = match.MatchId,
                InstantKey = match.InstantKey,
                HomeScore = match.HomeValue.ToString(),
                AwayScore = match.AwayValue.ToString()
            };

            return await repository.GetAsync<MatchChecking>(criteria) ?? new MatchChecking();
        }
    }
}