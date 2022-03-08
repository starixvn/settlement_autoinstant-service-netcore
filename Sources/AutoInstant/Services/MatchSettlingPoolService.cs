using AutoInstant.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoInstant.Services
{
    public interface MatchSettlingPoolService
    {
        Task<List<Match>> PeekNewMatch();

        void Remove(Match match);
    }

    public class MatchSettlingPoolServiceImp : MatchSettlingPoolService
    {
        private readonly MatchByLeagueService matchByLeagueService;

        private readonly List<Match> poolOfMatches;

        private readonly object lockObject;

        public MatchSettlingPoolServiceImp(MatchByLeagueService matchByLeagueService)
        {
            this.matchByLeagueService = matchByLeagueService;

            poolOfMatches = new List<Match>();
            lockObject = new object();
        }

        public List<Match> PoolOfMatches => poolOfMatches;

        public async Task<List<Match>> PeekNewMatch()
        {
            var matches = await matchByLeagueService.GetMatches();
            lock (lockObject)
            {
                var newMatches = matches.Except(poolOfMatches).ToList();
                poolOfMatches.AddRange(newMatches);

                return newMatches;
            }
        }

        public void Remove(Match match)
        {
            lock (lockObject)
            {
                poolOfMatches.RemoveAll(m => m.MatchId == match.MatchId);
            }
        }
    }
}