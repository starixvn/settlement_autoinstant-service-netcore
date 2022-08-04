using AutoInstant.Result.Services;
using AutoInstant.Services;
using AutoMapper;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Settlement.ScaleOut;
using Fanex.Settlement.ScaleOut.Services;
using Kaden.XmlConverter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Transport;

namespace Fanex.AutoInstant.Service
{
    public static class AutoPayoutInjector
    {
        public static void InitializeServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddTransient<IMaintenanceService, MaintenanceServiceImp>();
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

            service.AddSingleton(new UMClientConfiguration().Initialize(configuration));
            service.AddSingleton<IUMCacheService, UMCacheService>();
            service.AddSingleton<IUMRestClient, UMRestClient>();
            service.AddSingleton<ONEbook.UM.Services.IMaintenanceService, ONEbook.UM.Services.MaintenanceService>();
            service.AddTransient<ScaleOutSettleService, ScaleOutSettleServiceImp>();
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(typeof(ScaleOutMappingProfile));
            });

            var mapper = mapperConfig.CreateMapper();
            service.AddSingleton(mapper);

            ConfigureRedis(service, configuration);
        }

        private static void ConfigureRedis(IServiceCollection service, IConfiguration configuration)
        {
            var redisHost = configuration.GetSection("Redis.Server").Value;
            var redisPort = configuration.GetSection("Redis.Port").Value;
            var redisPassword = configuration.GetSection("Redis.Password").Value;
            var redisDatabase = configuration.GetSection("Redis.DefaultDatabase").Value;
            service.AddSingleton<ICache>(new RedisCache(new RedisCacheOptions
            {
                ConnectionString = $"{redisHost}:{redisPort},password={redisPassword},defaultDatabase={redisDatabase}",
                ApplicationName = configuration.GetSection("Redis.ApplicationName").Value,
            }));
            service.AddSingleton<ICacheService>(new CacheService());
        }
    }
}