using Gaa.Extensions.Observer;
using Gaa.Worker.Consumers;
using Gaa.Worker.Messages;
using Gaa.Worker.Workers;

namespace Gaa.Worker;

/// <summary>
/// Класс инициализирующий приложение.
/// </summary>
internal static class Startup
{
    /// <summary>
    /// Конфигурирует сервисы приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Набор свойств конфигурации приложения.</param>
    internal static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHostedService<ExampleWorker>()
            .AddHostedService<FirstWorker>()
            .AddHostedService<SecondWorker>()
            .Configure<TimeDelayOptions>(delayOptions =>
            {
                delayOptions.ExampleWorker = TimeSpan.FromMilliseconds(50);

                delayOptions.FirstWorker = TimeSpan.FromMilliseconds(150);
                delayOptions.SecondWorker = TimeSpan.FromMilliseconds(100);
            });

        services
            .AddInMemoryBus(busOptions =>
            {
                busOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(1);
            })
            .AddChildBus("Example.Bus", childBusOptions =>
            {
                childBusOptions.Capacity = 100;
            })
            .AddAsyncConsumer<ExampleConsumer, ExampleMessage>()
            .AddChildBus("Another.Bus", childBusOptions =>
            {
                childBusOptions.Capacity = 200;
            })
            .AddAsyncConsumer<FirstConsumer, FirstMessage>()
            .AddAsyncConsumer<SecondConsumer, SecondMessage>();

        services
            .AddHealthChecks();

        ////services
        ////    .AddOpenTelemetry()
        ////    .ConfigureResource(builder => builder.AddService("Gaa.Worker"))
        ////    .WithMetrics(builder => builder
        ////        .AddMeter(DefaultBusMetrics.MeterName)
        ////        .AddInstrumentation<DefaultBusMetrics>()
        ////        .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
        ////        {
        ////            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5_000;
        ////        }));
    }
}