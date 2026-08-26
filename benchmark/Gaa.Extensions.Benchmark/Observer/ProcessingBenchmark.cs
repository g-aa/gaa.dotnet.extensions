using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Gaa.Extensions.Benchmark.Observer.Features;
using Gaa.Extensions.Observer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Gaa.Extensions.Benchmark.Observer;

#pragma warning disable CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.

/// <summary>
/// Контрольный тест.
/// </summary>
[Orderer(SummaryOrderPolicy.Declared)]
[MemoryDiagnoser]
public class ProcessingBenchmark
{
    private const string Message = "Test message!";

    private ServiceProvider _provider;

    private DefaultBusPublisher _publisher;

    private DefaultBackgroundTaskQueue _taskQueue;

    /// <summary>
    /// Глобально настраивает окружение.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        _provider = new ServiceCollection()
            .AddSingleton(p =>
            {
                var mockFactory = new Mock<ILoggerFactory>();
                mockFactory.Setup(l => l.CreateLogger(It.IsAny<string>()));
                return mockFactory;
            })
            .Configure<BusOptions>(options =>
            {
                options.BackgroundTaskQueueCapacity = 100;
                options.BackgroundTaskExecutionTimeLimit = TimeSpan.FromMinutes(1);
            })
            .AddSingleton<IAsyncConsumer<string>, StringConsumer>()
            .BuildServiceProvider();

        var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
        var options = _provider.GetRequiredService<IOptions<BusOptions>>();

        _taskQueue = new DefaultBackgroundTaskQueue(loggerFactory, options);
        _publisher = new DefaultBusPublisher(_taskQueue);
    }

    /// <summary>
    /// Глобально освобождает ресурсы.
    /// </summary>
    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _provider.Dispose();
    }

    /// <summary>
    /// Публикует и потребляет сообщение.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public async Task PublishAndConsumeAsync()
    {
        // arrange & act
        await _publisher.PublishAsync(Message, CancellationToken.None);
        var backgroundTask = await _taskQueue.DequeueTaskAsync(CancellationToken.None);
        await backgroundTask.ExecuteAsync(_provider, CancellationToken.None);
    }
}