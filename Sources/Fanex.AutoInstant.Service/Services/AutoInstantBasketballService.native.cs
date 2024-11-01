using AutoInstant.Basketball.Services;
using AutoInstant.Services;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using LogService = Fanex.Logging.LogService;

namespace Fanex.AutoInstant.Service.Services;

public class AutoInstantBasketballService : IHostedService
{
    private readonly AutoInstantBasketballController autoInstantBasketballController;

    private readonly IMaintenanceService maintenanceService;

    private readonly IConfiguration configuration;

    private bool running;

    private int counter;

    public AutoInstantBasketballService(
        AutoInstantBasketballController autoInstantBasketballController,
        IConfiguration configuration,
        IMaintenanceService maintenanceService)
    {
        this.autoInstantBasketballController = autoInstantBasketballController;
        this.configuration = configuration;
        this.maintenanceService = maintenanceService;
        InitializeExtraComponent();
    }

    private static void InitializeExtraComponent()
    {
        TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
    }

    private static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        e.Exception.Handle(exception =>
        {
            var logMessage = new LogMessage(exception.Message)
                .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                .ToString();
            LogService.Error(logMessage);
            return true;
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        running = true;
        var autoRefundThread = new Thread(ThreadProcess);
        Console.WriteLine($"Auto Instant Basketball Starting at {DateTimeOffset.Now}");
        autoRefundThread.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        running = false;
        Console.WriteLine($"Auto Instant Basketball Stopping at {DateTimeOffset.Now}");
        return Task.CompletedTask;
    }

    private void ThreadProcess()
    {
        while (running)
        {
            if (!maintenanceService.IsMaintenance)
            {
                MainProcess();

                var interval = configuration.GetSection("BasketballServiceInterval").Value ?? "0";

                Thread.Sleep(int.Parse(interval));
            }
        }
    }

    private void MainProcess()
    {
        try
        {
            autoInstantBasketballController.ProcessAsync();
            HandleGarbageCollector();
        }
        catch (Exception exception)
        {
            LogService.Error("Error from AutoInstant Service(Basketball):", exception);
        }
    }

    private void HandleGarbageCollector()
    {
        var gcInterval = configuration.GetSection("GarbageCollectorInterval").Value ?? "0";
        if (counter >= int.Parse(gcInterval))
        {
            counter = 0;
            GC.WaitForPendingFinalizers();
        }

        var interval = configuration.GetSection("ServiceInterval").Value ?? "0";

        counter += int.Parse(interval);
    }
}