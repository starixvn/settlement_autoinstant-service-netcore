using AutoInstant.Constants;
using AutoInstant.Exceptions;
using AutoInstant.Models;
using System;

namespace AutoInstant.Services
{
    public interface ValidationStatusMatchService
    {
        void VerifyStatusMatch(Match match, MatchChecking matchChecking);
    }

    public class ValidationStatusMatchServiceImp : ValidationStatusMatchService
    {
        public void VerifyStatusMatch(Match match, MatchChecking matchChecking)
        {
            if (matchChecking.IsInstCalculated && !IsWrongScore(match, matchChecking))
            {
                var score = $"{match.HomeValue}-{match.AwayValue}";
                throw new ManualCalculatedException(score, false);
            }

            if (IsCalculated(matchChecking))
            {
                throw new FTCalculatedException();
            }

            if (IsHalfTimeNotDone(matchChecking))
            {
                throw new HalfTimeNotDoneException();
            }
        }

        private static bool IsHalfTimeNotDone(MatchChecking matchChecking)
        {
            return matchChecking.TypeId == SettleType.SecondHalf && !matchChecking.IsCalculatedHT;
        }

        private static bool IsCalculated(MatchChecking matchChecking)
        {
            return matchChecking.IsCalculatedFT;
        }

        private static bool IsWrongScore(Match match, MatchChecking matchChecking)
        {
            return match.HomeValue < Convert.ToInt32(matchChecking.PreviousHomeScore)
                || Convert.ToInt32(match.AwayValue) < Convert.ToInt32(matchChecking.PreviousAwayScore);
        }
    }
}