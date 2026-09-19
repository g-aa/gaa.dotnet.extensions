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
            .Configure<TimeOptions>(timeOptions =>
            {
                timeOptions.HostExecutionTimeLimit = TimeSpan.FromMinutes(1);

                timeOptions.PeriodForExampleWorker = TimeSpan.FromMilliseconds(10);
                timeOptions.PeriodForErrorWorker = TimeSpan.FromMilliseconds(1_000);
            });

        services
            .AddBus(busOptions => { })
            .AddInMemoryTransport("Queue.Example", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(1);
                transportOptions.Capacity = 100;
            })
            .AddAsyncConsumer<ExampleConsumer, ExampleMessage>()
            .AddInMemoryTransport("Queue.Error", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<ErrorConsumer, ErrorMessage>();

        services
            .AddHealthChecks();

        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("Gaa.Worker"))
            .WithMetrics(builder => builder
                .AddMeter(InMemoryTransportMetrics.MeterName)
                .AddInstrumentation<InMemoryTransportMetrics>()
                .AddMeter(ExampleMetrics.MeterName)
                .AddInstrumentation<ExampleMetrics>()
                .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                {
                    metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5_000;
                }));
    }
}