using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Набор тестов для <see cref="ITransportNameSelector"/>.
/// </summary>
[TestFixture]
internal sealed class DefaultTransportNameSelectorTest
{
    private ServiceProvider _provider;

    /// <summary>
    /// Настраивает тестовое окружение однократно.
    /// </summary>
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _provider = new ServiceCollection()
            .Configure<BusOptions>(options =>
            {
                options.Subscriptions.Add("Bool", [typeof(bool)]);
                options.Subscriptions.Add("Char", [typeof(char)]);
                options.Subscriptions.Add("Integer", [typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong)]);
                options.Subscriptions.Add("Float", [typeof(float), typeof(double), typeof(decimal)]);
            })
            .AddSingleton<ITransportNameSelector, DefaultTransportNameSelector>()
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
    /// Успешное выполнение <see cref="ITransportNameSelector.GetTransportName{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <typeparam name="TName">Тип наименование транспортной шины.</typeparam>
    /// <param name="message">Сообщение.</param>
    /// <param name="transportName">Наименование транспортной шины.</param>
    [TestCase<bool, string>(true, "Bool")]
    [TestCase<char, string>('A', "Char")]
    [TestCase<long, string>(100L, "Integer")]
    [TestCase<double, string>(2.73d, "Float")]
    [TestCase<string, string>("QWERTY", default!)]
    public void SuccessfulGetTransportName<TMessage, TName>(TMessage message, string transportName)
    {
        // arrange
        var func = () => _provider.GetRequiredService<ITransportNameSelector>().GetTransportName<TMessage>();

        // act & assertion
        func.Should().NotThrow().Subject.Should().Be(transportName);
    }
}