using AutoInstant.Basketball.Services;

namespace Fanex.AutoInstant.Service.Configurations;

public static class AutoInstantBasketballConfiguration
{
    public static void AutoInstantsBasketballConfigServices(this IServiceCollection service)
    {
        service.AddSingleton<SettleMatchService, SettleMatchServiceImp>();
        service.AddSingleton<AutoInstantBasketballController, AutoInstantBasketballControllerImp>();
        service.AddSingleton<GetMatchService, GetMatchServiceImp>();
        service.AddSingleton<LogService, LogServiceImp>();
        service.AddSingleton<MatchSettlingPoolService, MatchSettlingPoolServiceImp>();
        service.AddSingleton<ScoreValidateService, ScoreValidateServiceImp>();
        service.AddSingleton<MatchSettleService, MatchSettleServiceImp>();
        service.AddSingleton<GenerateService, GenerateServiceImp>();
    }
}