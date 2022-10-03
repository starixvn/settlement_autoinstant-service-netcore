using AutoInstant.Models;
using Fanex.AutoInstant.Service;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .Configure<AppSettingsKey>(builder.Configuration)
    .AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettingsKey>>().Value);

builder.Services.InitializeServices(builder.Configuration);

builder.Services.AddHostedService<AutoInstantService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
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
app.Run("http://0.0.0.0:5000")
#endif
