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
    private const string BusName = "Test.Bus";

    private const string Message = "Test message!";

    private ServiceProvider _provider;

    private IServiceScopeFactory _scopeFactory;

    private DefaultBusPublisher _publisher;

    private IBackgroundTaskBus _bus;

    private TimeSpan _timeLimit;

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
                mockFactory.Setup(l => l.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
                return mockFactory.Object;
            })
            .Configure<BusOptions>(options =>
            {
                options.ExecutionTimeLimit = TimeSpan.FromMinutes(1);
                options.Subscriptions.Add(BusName, [typeof(string)]);
                options.Options.Add(new() { Name = BusName, Capacity = 1_000 });
            })
            .AddSingleton<DefaultBusPublisher>()
            .AddSingleton<IBackgroundTaskBusFactory, DefaultBackgroundTaskBusFactory>()
            .AddSingleton<IBackgroundTaskBusNameSelector, DefaultBackgroundTaskBusNameSelector>()

            .AddSingleton<IAsyncConsumer<string>, StringConsumer>()
            .BuildServiceProvider();

        _scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();
        _timeLimit = _provider.GetRequiredService<IOptions<BusOptions>>().Value.ExecutionTimeLimit;
        _bus = _provider.GetRequiredService<IBackgroundTaskBusFactory>().GetOrCreate(BusName);
        _publisher = _provider.GetRequiredService<DefaultBusPublisher>();
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
        await BusExecuteAsync(_bus, _timeLimit, CancellationToken.None);
    }

    private static TimeSpan GetTimeLimit(TimeSpan? taskTimeLimit, TimeSpan defaultTimeLimit)
    {
        if (taskTimeLimit == null)
        {
            return defaultTimeLimit;
        }

        return taskTimeLimit < defaultTimeLimit ? taskTimeLimit.Value : defaultTimeLimit;
    }

    private async Task BusExecuteAsync(IBackgroundTaskBus bus, TimeSpan defaultTimeLimit, CancellationToken stoppingToken)
    {
        try
        {
            var backgroundTask = await bus.DequeueTaskAsync(stoppingToken);
            await using var scope = _scopeFactory.CreateAsyncScope();
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            cts.CancelAfter(GetTimeLimit(backgroundTask.ExecutionTimeLimit, defaultTimeLimit));
            await backgroundTask.ExecuteAsync(scope.ServiceProvider, cts.Token);
        }
        catch
        {
            /* Можно не обрабатывать */
        }
    }
}