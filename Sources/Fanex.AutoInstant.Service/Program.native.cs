using AutoInstant.Basketball.Models;
using AutoInstant.Models;
using Fanex.AutoInstant.Service.Configurations;
using Fanex.AutoInstant.Service.Services;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.Builder;
using ONEbook.UM.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOneBookUm(builder.Configuration);

builder.Services.AddFanexLogSentry(builder.Configuration);

builder.Services.AddFanexSql(builder.Configuration);

builder.Services.AddConfigRedisCache(builder.Configuration);

builder.Services.AddAutoMapper();

builder.Services.Configure<AppSettingsKey>(builder.Configuration);

builder.Services.Configure<AppSettingsBasketballKey>(builder.Configuration);

builder.Services.AutoInstantsConfigServices(builder.Configuration);

builder.Services.AutoInstantsBasketballConfigServices();

builder.Services.AddHostedService<AutoInstantService>();

builder.Services.AddHostedService<AutoInstantBasketballService>();

var app = builder.Build();

app.Run(async context =>
{
    await context.Response.WriteAsync("OK");
});

await app.RunAsync();