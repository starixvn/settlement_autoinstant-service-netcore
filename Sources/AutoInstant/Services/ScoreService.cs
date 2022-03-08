using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoInstant.Services
{
    public interface ScoreService
    {
        Task<List<Score>> GetScore(int matchId);
    }

    public class ScoreServiceImp : ScoreService
    {
        private readonly IDynamicRepository repository;

        public ScoreServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Score>> GetScore(int matchId)
        {
            var criteria = new MatchScoreCriteria { MatchId = matchId };
            return (await repository.FetchAsync<Score>(criteria))
                             .Where(s => s.SourceId == Source.SoccerWay
                             && (s.TypeId == SettleType.FirstHalf || s.TypeId == SettleType.SecondHalf)).ToList();
        }
    }
}