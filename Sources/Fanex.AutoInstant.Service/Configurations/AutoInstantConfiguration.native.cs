using AutoInstant.Result.Services;
using AutoInstant.Services;
using AutoMapper;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Settlement.ScaleOut;
using Fanex.Settlement.ScaleOut.Services;
using LogService = AutoInstant.Services.LogService;

namespace Fanex.AutoInstant.Service.Configurations;

public static class AutoInstantConfiguration
{
    public static void AutoInstantsConfigServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddSingleton<IDynamicRepository, DynamicRepository>();
        service.AddSingleton<ActionResourceService, ActionResourceServiceImp>();
        service.AddSingleton<FirstGoalService, FirstGoalServiceImp>();
        service.AddSingleton<GoalRecordService, GoalRecordServiceImp>();
        service.AddSingleton<ResourceService, ResourceServiceImp>();
        service.AddSingleton<ResultsService, ResultsServiceImp>();
        service.AddSingleton<MatchSettlingPoolService, MatchSettlingPoolServiceImp>();
        service.AddSingleton<MatchQueryService, MatchQueryServiceImp>();
        service.AddSingleton<XmlConvertService, XmlConvertServiceImp>();
        service.AddSingleton<MatchSettleController, MatchSettleControllerImp>();
        service.AddSingleton<ScoreService, ScoreServiceImp>();
        service.AddSingleton<MatchByLeagueService, MatchByLeagueServiceImp>();
        service.AddSingleton<AutoInstantController, AutoInstantControllerImp>();
        service.AddSingleton<AutoInstantResultService, AutoInstantResultServiceImp>();
        service.AddSingleton<LogCriteriaService, LogCriteriaServiceImp>();
        service.AddSingleton<LogService, LogServiceImp>();
        service.AddSingleton<MatchSettleService, MatchSettleServiceImp>();
        service.AddSingleton<NotificationService, NotificationServiceImp>();
        service.AddSingleton<PushingCalculationCriteriaService, PushingCalculationCriteriaServiceImp>();
        service.AddSingleton<PushingCalculationService, PushingCalculationServiceImp>();
        service.AddSingleton<ScoreMatcherService, ScoreMatcherServiceImp>();
        service.AddSingleton<ScoreRetryService, ScoreRetryServiceImp>();
        service.AddSingleton<ValidationScoreChangeService, ValidationScoreChangeServiceImp>();
        service.AddSingleton<ValidationService, ValidationServiceImp>();
        service.AddSingleton<ValidationSettingMatchService, ValidationSettingMatchServiceImp>();
        service.AddSingleton<ValidationStatusMatchService, ValidationStatusMatchServiceImp>();
        service.AddSingleton<ValidateHoldStatusService, ValidateHoldStatusServiceImp>();
        service.AddTransient<GeneralService, GeneralServiceImp>();
        service.AddSingleton<LogDirectlyService, LogDirectlyServiceImp>();
        service.AddTransient<ScaleOutSettleService, ScaleOutSettleServiceImp>();
        service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static void AddConfigRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Redis:ConnectionString");
        var applicationName = configuration.GetValue<string>("Redis:ApplicationName");

        services.AddSingleton<ICache>(new RedisCache(new RedisCacheOptions
        {
            ConnectionString = connectionString,
            ApplicationName = applicationName,
        }));
        services.AddSingleton<ICacheService>(new CacheService());
    }

    public static void AddAutoMapper(this IServiceCollection service)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(typeof(ScaleOutMappingProfile));
        });

        var mapper = mapperConfig.CreateMapper();
        service.AddSingleton(mapper);
    }
}