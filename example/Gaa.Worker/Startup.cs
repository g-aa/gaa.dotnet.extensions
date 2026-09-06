using Gaa.Extensions.Observer;
using Gaa.Worker.Error;
using Gaa.Worker.Example;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

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
            .AddHostedService<ErrorWorker>()
            .Configure<TimeDelayOptions>(delayOptions =>
            {
                delayOptions.ExampleWorker = TimeSpan.FromMilliseconds(500);
                delayOptions.ErrorWorker = TimeSpan.FromMilliseconds(1_000);
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
            .AddChildBus("Error.Bus", childBusOptions =>
            {
                childBusOptions.Capacity = 200;
            })
            .AddAsyncConsumer<ErrorConsumer, ErrorMessage>();

        services
            .AddHealthChecks();

        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("Gaa.Worker"))
            .WithMetrics(builder => builder
                .AddMeter(DefaultBusMetrics.MeterName)
                .AddInstrumentation<DefaultBusMetrics>()
                .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                {
                    metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5_000;
                }));
    }
}