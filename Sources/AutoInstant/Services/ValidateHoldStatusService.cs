using AutoInstant.Criteria;
using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;
public interface ValidateHoldStatusService
{
    Task ValidateHoldStatus(Match match);
}

public class ValidateHoldStatusServiceImp(IDynamicRepository repository) : ValidateHoldStatusService
{
    public async Task ValidateHoldStatus(Match match)
    {
        var criteria = new GetResultDetailCriteria { MatchIdList = match.MatchId.ToString() };
        var resultDetails = await repository.FetchAsync<ResultDetail>(criteria);
        var resultMatched = resultDetails.Where(r => r.SourceId == 0).ToList();


        if (resultMatched.Any(r => r.IsHold))
        {
            throw new ResultOnHoldException();
        }
    }
}
