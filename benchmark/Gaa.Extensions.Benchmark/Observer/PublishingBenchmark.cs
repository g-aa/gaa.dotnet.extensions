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
    private const string BusName = "Test.Bus";

    private const string Message = "Test message!";

    private ServiceProvider _provider;

    private DefaultBusPublisher _publisher;

    private DefaultChildBus _childBus;

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
            .AddSingleton<IChildBusFactory<DefaultChildBus>, DefaultChildBusFactory>()
            .AddSingleton<IChildBusSelector, DefaultChildBusSelector>()
            .AddSingleton<DefaultBusPublisher>()

            .AddSingleton<IAsyncConsumer<string>, StringConsumer>()
            .BuildServiceProvider();

        _childBus = _provider.GetRequiredService<IChildBusFactory<DefaultChildBus>>().GetOrCreate(BusName);
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
    public async Task PublishAsync()
    {
        // arrange & act
        await _publisher.PublishAsync(Message, CancellationToken.None);
        var backgroundTask = await _childBus.DequeueTaskAsync(CancellationToken.None);
        await backgroundTask.ExecuteAsync(_provider, CancellationToken.None);
    }
}