using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Gaa.Extensions.Benchmark.Observer.Features;
using Gaa.Extensions.Observer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    private const string TransportName = "Mock.Transport";

    private const string Message = "Test message!";

    private static readonly TimeSpan ExecutionTimeLimit = TimeSpan.FromMinutes(1);

    private ServiceProvider _provider;

    private IServiceScopeFactory _scopeFactory;

    private DefaultPublisher _publisher;

    private InMemoryTransport _transport;

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
                options.Subscriptions.Add(TransportName, [typeof(string)]);
                options.Transports.Add(new InMemoryTransportOptions { Name = TransportName });
            })
            .AddSingleton<DefaultPublisher>()
            .AddSingleton<ITransportFactory, InMemoryTransportFactory>()
            .AddSingleton<ITransportNameSelector, DefaultTransportNameSelector>()
            .AddSingleton<ITransportSelector, DefaultTransportSelector>()

            .AddSingleton<IAsyncConsumer<string>, StringConsumer>()
            .BuildServiceProvider();

        _scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();
        _transport = (InMemoryTransport)_provider.GetRequiredService<ITransportSelector>().GetTransport(TransportName);
        _publisher = _provider.GetRequiredService<DefaultPublisher>();
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
        await BusExecuteAsync(_transport, ExecutionTimeLimit, CancellationToken.None);
    }

    private static TimeSpan GetTimeLimit(TimeSpan? taskTimeLimit, TimeSpan defaultTimeLimit)
    {
        if (taskTimeLimit == null)
        {
            return defaultTimeLimit;
        }

        return taskTimeLimit < defaultTimeLimit ? taskTimeLimit.Value : defaultTimeLimit;
    }

    private async Task BusExecuteAsync(InMemoryTransport transport, TimeSpan defaultTimeLimit, CancellationToken stoppingToken)
    {
        try
        {
            var executionContext = await transport.ReadAsync(stoppingToken);
            await using var scope = _scopeFactory.CreateAsyncScope();
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            cts.CancelAfter(defaultTimeLimit);
            await executionContext.ExecuteAsync(scope.ServiceProvider, cts.Token);
        }
        catch
        {
            /* Можно не обрабатывать */
        }
    }
}