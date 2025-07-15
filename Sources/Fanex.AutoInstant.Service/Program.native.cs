using AutoInstant.Basketball.Models;
using AutoInstant.Models;
using Fanex.AutoInstant.Service.Configurations;
using Fanex.AutoInstant.Service.Services;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.Builder;
using ONEbook.UM.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddOneBookUmForService(builder.Configuration);

services.AddFanexLogSentry(builder.Configuration);

services.AddFanexSql(builder.Configuration);

services.AddConfigRedisCache(builder.Configuration);

services.AddAutoMapper();

services.Configure<AppSettingsKey>(builder.Configuration);

services.Configure<AppSettingsBasketballKey>(builder.Configuration);

services.AutoInstantsConfigServices(builder.Configuration);

services.AutoInstantsBasketballConfigServices();

services.AddHostedService<AutoInstantService>();

services.AddHostedService<AutoInstantBasketballService>();
services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/healthz");
await app.RunAsync();