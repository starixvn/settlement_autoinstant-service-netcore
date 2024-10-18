using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchByLeagueService
{
    Task<List<Match>> GetMatches();
}

public class MatchByLeagueServiceImp(
    MatchQueryService matchQueryService,
    ICache cache,
    IOptions<AppSettingsKey> appSettingsKey)
    : MatchByLeagueService
{
    private const string KeyLeagueId = "Settlement-AutoPayout-AutoInstant-LeagueId";

    public async Task<List<Match>> GetMatches()
    {
        var matches = await matchQueryService.GetWaitingMatches();

        cache.TryGet(KeyLeagueId, out int leagueId);

        return appSettingsKey.Value.EnvironmentName switch
        {
            "PRO" => leagueId > 0 ? (from m in matches where m.LeagueId != leagueId select m).ToList() : matches,
            "UAT" => (from m in matches where m.LeagueId == leagueId select m).ToList(),
            _ => matches
        };
    }
}