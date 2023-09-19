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

        var (_, response) = await cache.TryGetAsync<string>(KeyLeagueId);
        var leagueIds = (response ?? "").Split(",");

        return appSettingsBasketballKey.EnvironmentName switch
        {
            "PRO" => leagueIds.Length > 0
                ? matches.Where(m => !leagueIds.Contains(m.LeagueId.ToString())).ToList()
                : matches,
            "UAT" => matches.Where(m => leagueIds.Contains(m.LeagueId.ToString())).ToList(),
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