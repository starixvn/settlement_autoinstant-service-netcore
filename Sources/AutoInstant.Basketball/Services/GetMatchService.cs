using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using Fanex.Caching;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using Match = AutoInstant.Basketball.Models.Match;

namespace AutoInstant.Basketball.Services;

public interface GetMatchService
{
    public Task<List<Match>> GetMatches();
}

public class GetMatchServiceImp : GetMatchService
{
    private const string KeyLeagueId = "Settlement-AutoPayoutOtherSport-LeagueId";

    private readonly ICache cache;

    private readonly IDynamicRepository repository;

    private readonly AppSettingsBasketballKey appSettingsBasketballKey;

    public GetMatchServiceImp(IDynamicRepository repository, ICache cache, IOptions<AppSettingsBasketballKey> appSettingsBasketballKey)
    {
        this.repository = repository;
        this.cache = cache;
        this.appSettingsBasketballKey = appSettingsBasketballKey.Value;
    }

    public async Task<List<Match>> GetMatches()
    {
        var matches = await GetWaitingMatches();

        cache.TryGet(KeyLeagueId, out int leagueId);

        return appSettingsBasketballKey.EnvironmentName switch
        {
            "PRO" => leagueId > 0 ? (from m in matches where m.LeagueId != leagueId select m).ToList() : matches,
            "UAT" => (from m in matches where m.LeagueId == leagueId select m).ToList(),
            _ => matches
        };
    }

    private async Task<List<Match>> GetWaitingMatches()
    {
        var criteria = new MatchWaitingCriteria();
        var resultDetails = (await repository.FetchAsync<ResultDetail>(criteria)).ToList();
        var groupResult = resultDetails.GroupBy(r => r.MatchId);
        return groupResult.Select(r => new Match { MatchId = r.Key, ResultDetails = r.ToList(), LeagueId = r.First().LeagueId }).ToList();
    }
}