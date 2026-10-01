using Gaa.Extensions.Observer;
using Gaa.Worker.Error;
using Gaa.Worker.Multiple;
using Gaa.Worker.Single;
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
            .AddHostedService<ErrorWorker>()
            .AddHostedService<SingleExampleWorker>()
            .AddHostedService<MultipleExampleWorker>()
            .Configure<TimeOptions>(timeOptions =>
            {
                timeOptions.HostExecutionTimeLimit = TimeSpan.FromMinutes(1);

                timeOptions.PeriodForErrorWorker = TimeSpan.FromMilliseconds(1_000);
                timeOptions.PeriodForExampleWorker = TimeSpan.FromMilliseconds(10);

                timeOptions.PeriodForMultipleWorker = TimeSpan.FromMilliseconds(1_000);
                timeOptions.MessagePerPeriodForMultipleWorker = 10;
            });

        services
            .AddBus(busOptions => { })
            .AddInMemoryTransport("Queue.Error", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<ErrorConsumer, ErrorMessage>()
            .AddInMemoryTransport("Queue.Example.Single", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(1);
                transportOptions.Capacity = 100;
            })
            .AddAsyncConsumer<SingleExampleConsumer, SingleExampleMessage>()
            .AddConcurrentTransport("Queue.Example.Multiple", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(3);
                transportOptions.Capacity = 150;
            })
            .AddAsyncConsumer<MultipleExampleConsumer, MultipleExampleMessage>();

        services
            .AddHealthChecks();

        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("Gaa.Worker"))
            .WithMetrics(builder => builder
                .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                {
                    metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5_000;
                })
                .AddMeter(InMemoryTransportMetrics.MeterName)
                .AddInstrumentation<InMemoryTransportMetrics>()
                .AddMeter(ConcurrentTransportMetrics.MeterName)
                .AddInstrumentation<ConcurrentTransportMetrics>()
                .AddMeter(SingleExampleMetrics.MeterName)
                .AddInstrumentation<SingleExampleMetrics>()
                .AddMeter(MultipleExampleMetrics.MeterName)
                .AddInstrumentation<MultipleExampleMetrics>());
    }
}