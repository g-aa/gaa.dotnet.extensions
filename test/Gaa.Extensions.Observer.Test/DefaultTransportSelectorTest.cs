using Gaa.Extensions.Observer.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Набор тестов для <see cref="ITransportSelector"/>.
/// </summary>
[TestFixture]
internal sealed class DefaultTransportSelectorTest
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
            .AddSingleton<ITransportSelector, DefaultTransportSelector>()
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
    /// Успешное выполнение <see cref="ITransportSelector.GetTransport(string)"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    [TestCase(TransportName)]
    public void SuccessfulGetTransport(string transportName)
    {
        // arrange
        var func = () => _provider.GetRequiredService<ITransportSelector>().GetTransport(transportName);

        // act & assertion
        func.Should().NotThrow();
    }

    /// <summary>
    /// Успешное выполнение <see cref="ITransportSelector.GetTransport{T}(string)"/>.
    /// </summary>
    /// <typeparam name="TName">Тип наименования транспортной шины.</typeparam>
    /// <typeparam name="T">Тип транспортной шины.</typeparam>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="transport">Транспортная шина.</param>
    [TestCaseSource(nameof(SuccessfulGetTransportSource))]
    public void SuccessfulGetTransport<TName, T>(string transportName, T transport)
        where T : ITransport
    {
        // arrange
        var func = () => _provider.GetRequiredService<ITransportSelector>().GetTransport<T>(transportName);

        // act & assertion
        func.Should().NotThrow();
    }

    /// <summary>
    /// Не успешное выполнение <see cref="ITransportSelector.GetTransport(string)"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    [TestCase("Queue.Other")]
    public void UnsuccessfulGetTransport(string transportName)
    {
        // arrange
        var func = () => _provider.GetRequiredService<ITransportSelector>().GetTransport(transportName);

        // act & assertion
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Sequence contains no matching element");
    }

    /// <summary>
    /// Успешное выполнение <see cref="ITransportSelector.GetTransport{T}(string)"/>.
    /// </summary>
    /// <typeparam name="TName">Тип наименования транспортной шины.</typeparam>
    /// <typeparam name="T">Тип транспортной шины.</typeparam>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="transport">Транспортная шина.</param>
    [TestCaseSource(nameof(UnsuccessfulGetTransportSource))]
    public void UnsuccessfulGetTransport<TName, T>(string transportName, T transport)
        where T : ITransport
    {
        // arrange
        var func = () => _provider.GetRequiredService<ITransportSelector>().GetTransport<T>(transportName);

        // act & assertion
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Не удается получить экземпляр транспортной шины '{typeof(T).FullName}' по наименованию '{transportName}'!");
    }

    private static IEnumerable<TestCaseData> SuccessfulGetTransportSource()
    {
        yield return new TestCaseData<string, MockTransport?>(TransportName, default);
    }

    private static IEnumerable<TestCaseData> UnsuccessfulGetTransportSource()
    {
        yield return new TestCaseData<string, InMemoryTransport?>(TransportName, default);
    }
}