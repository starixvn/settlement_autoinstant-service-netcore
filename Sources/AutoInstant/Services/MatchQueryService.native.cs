using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchQueryService
{
	Task<List<Match>> GetWaitingMatches();

	Task<MatchInfo> GetMatchInfo(Match match);
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

	public async Task<MatchInfo> GetMatchInfo(Match match)
	{
		var criteria = new MatchInfoCriteria
		{
			MatchId = match.MatchId,
			InstantKey = match.InstantKey,
			HomeScore = match.HomeValue.ToString(),
			AwayScore = match.AwayValue.ToString()
		};

		return await repository.GetAsync<MatchInfo>(criteria) ?? new MatchInfo();
	}
}