using AutoInstant.Basketball.Models;
using AutoInstant.Models;
using Fanex.AutoInstant.Service.Configurations;
using Fanex.AutoInstant.Service.Services;
using Fanex.Data.SqlServer;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFanexSql(builder.Configuration);

builder.Services.Configure<AppSettingsKey>(builder.Configuration);

builder.Services.Configure<AppSettingsBasketballKey>(builder.Configuration);

builder.Services.AutoInstantsConfigServices(builder.Configuration);

builder.Services.AutoInstantsBasketballConfigServices();

builder.Services.AddHostedService<AutoInstantService>();

builder.Services.AddHostedService<AutoInstantBasketballService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGet("/",
        async context => { await context.Response.WriteAsync("Fanex.AutoInstant.Service"); });

#if DEBUG
app.Run();
#else
await app.RunAsync("http://0.0.0.0:5000");
#endif