using AutoInstant.Exceptions;
using AutoInstant.Models;
using System.Linq;

namespace AutoInstant.Services
{
    public interface ValidationSettingMatchService
    {
        void VerifySettingMatch(MatchChecking matchChecking);
    }

    public class ValidationSettingMatchServiceImp : ValidationSettingMatchService
    {
        public void VerifySettingMatch(MatchChecking matchChecking)
        {
            if (matchChecking.IsSettingOff)
            {
                throw new SettingOffException();
            }

            var remark = matchChecking.Remark;

            bool[] conditionsToCheck = { remark.IsMtdHold, remark.IsUndecided, remark.IsPending, remark.IsUnderSettle, remark.IsHoldKey };

            if (conditionsToCheck.Any(item => item))
            {
                throw new RemarkException();
            }
        }
    }
}