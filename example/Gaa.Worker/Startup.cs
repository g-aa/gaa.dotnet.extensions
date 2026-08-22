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
            .AddBus(options =>
            {
                options.BackgroundTaskQueueCapacity = 1_000;
                options.BackgroundTaskExecutionTimeLimit = TimeSpan.FromMinutes(1);
            })
            .AddAsyncConsumer<ExampleConsumer, ExampleMessage>(ServiceLifetime.Singleton)
            .Services
            .AddHostedService<ExampleWorker>();

        services
            .AddHealthChecks();
    }
}