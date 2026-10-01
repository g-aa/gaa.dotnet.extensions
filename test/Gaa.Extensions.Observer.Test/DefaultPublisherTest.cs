using Gaa.Extensions.Observer.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Набор тестов для <see cref="DefaultPublisher"/>.
/// </summary>
[TestFixture]
internal sealed class DefaultPublisherTest
{
    private const string TransportName = "Queue.Test";

    private ServiceProvider _provider;

    /// <summary>
    /// Настраивает тестовое окружение однократно.
    /// </summary>
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _provider = new ServiceCollection()
            .AddSingleton<ITransportFactory, MockTransportFactory>()
            .Configure<BusOptions>(options =>
            {
                options.Subscriptions.Add(TransportName, [typeof(string)]);
                options.Transports.Add(new MockTransportOptions
                {
                    Name = TransportName,
                    Type = typeof(MockTransport),
                });
            })
            .AddBus(busOptions => { })
            .Services
            .BuildServiceProvider();
    }

    /// <summary>
    /// Настраивает окружение для тестирования.
    /// </summary>
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _provider.Dispose();
    }

    /// <summary>
    /// Успешное выполнение <see cref="IPublisher.PublishAsync{TMessage}(TMessage, CancellationToken)"/>.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Сообщение.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCaseSource(nameof(SuccessfulPublishSource))]
    public async Task SuccessfulPublishAsync<TMessage>(TMessage message)
        where TMessage : notnull
    {
        // arrange
        var func = () => _provider.GetRequiredService<IPublisher>().PublishAsync(message, CancellationToken.None);

        // act && assertion
        await func.Should().NotThrowAsync();
    }

    private static IEnumerable<TestCaseData> SuccessfulPublishSource()
    {
        yield return new TestCaseData<string>("QWERTY");
        yield return new TestCaseData<FirstMessage>(new() { Id = Guid.Empty });
    }
}