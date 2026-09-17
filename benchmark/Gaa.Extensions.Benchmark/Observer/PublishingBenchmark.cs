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
public class PublishingBenchmark
{
    private const string TransportName = "Mock.Transport";

    private const string Message = "Test message!";

    private ServiceProvider _provider;

    private DefaultPublisher _publisher;

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
            .AddSingleton<ITransportFactory, MockTransportFactory>()
            .AddSingleton<ITransportNameSelector, DefaultTransportNameSelector>()
            .AddSingleton<ITransportSelector, DefaultTransportSelector>()

            .AddSingleton<IAsyncConsumer<string>, StringConsumer>()
            .BuildServiceProvider();

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
    public async Task PublishAsync()
    {
        // arrange & act
        await _publisher.PublishAsync(Message, CancellationToken.None);
    }
}