using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ValidationSettingMatchService
{
    void VerifySettingMatch(MatchInfo matchChecking);
}

public class ValidationSettingMatchServiceImp : ValidationSettingMatchService
{
    public void VerifySettingMatch(MatchInfo matchChecking)
    {
        if (matchChecking.IsSettingOff)
        {
            throw new SettingOffException();
        }

        var remark = matchChecking.Remark;

        bool[] conditionsToCheck = { remark.IsMtdHold, remark.IsUndecided, remark.IsPending, remark.IsUnderSettle, remark.IsHoldKey };

        if (Array.Exists(conditionsToCheck, item => item))
        {
            throw new RemarkException();
        }
    }
}