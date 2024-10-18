using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ValidationScoreChangeService
{
    void VerifyScoreChanged(Match match, MatchInfo matchChecking);
}

public class ValidationScoreChangeServiceImp : ValidationScoreChangeService
{
    public void VerifyScoreChanged(Match match, MatchInfo matchChecking)
    {
        if (IsWrongScore(match, matchChecking))
        {
            if (matchChecking.InstantKey <= match.InstantKey)
            {
                throw new ScoreChangedException();
            }

            var score = $"{matchChecking.PreviousHomeScore}-{matchChecking.PreviousAwayScore}";
            throw new ManualCalculatedException(score, true);
        }
    }

    private static bool IsWrongScore(Match match, MatchInfo matchChecking)
    {
        return match.HomeValue < Convert.ToInt32(matchChecking.PreviousHomeScore)
            || match.AwayValue < Convert.ToInt32(matchChecking.PreviousAwayScore);
    }
}