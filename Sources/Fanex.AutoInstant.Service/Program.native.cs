using AutoInstant.Models;
using Fanex.AutoInstant.Service;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettingsKey>(builder.Configuration);

builder.Services.InitializeServices(builder.Configuration);

builder.Services.AddHostedService<AutoInstantService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/",
        async context => { await context.Response.WriteAsync("Fanex.AutoInstant.Service"); });
});

#if DEBUG
app.Run();
#else
await app.RunAsync("http://0.0.0.0:5000");
#endif