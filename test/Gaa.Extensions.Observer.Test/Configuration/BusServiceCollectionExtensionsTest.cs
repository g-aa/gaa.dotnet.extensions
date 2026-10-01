using Gaa.Extensions.Observer.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test.Configuration;

/// <summary>
/// Набор тестов для <see cref="BusServiceCollectionExtensions"/>.
/// </summary>
[TestFixture]
internal sealed class BusServiceCollectionExtensionsTest
{
    /// <summary>
    /// Успешная регистрация базовых компонент шины.
    /// </summary>
    [Test]
    public void SuccessfulBasicBusComponentRegistration()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddBus(busOptions => { })
            .Services
            .BuildServiceProvider()
            .GetRequiredService<IPublisher>();

        // act & assertion
        func.Should().NotThrow();
    }

    /// <summary>
    /// Успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void SuccessfulConsumerRegistrationForInMemory()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddInMemoryTransport("Queue.Test", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<TestConsumer, string>()
            .Services
            .BuildServiceProvider();

        // act & assertion
        func.Should().NotThrow();
    }

    /// <summary>
    /// Успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void SuccessfulConsumerRegistrationForConcurrent()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddConcurrentTransport("Queue.Test", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<TestConsumer, string>()
            .Services
            .BuildServiceProvider();

        // act & assertion
        func.Should().NotThrow();
    }

    /// <summary>
    /// Не успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void UnsuccessfulConsumerRegistrationForInMemory()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddInMemoryTransport("Queue.Test", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<TestConsumer, string>()
            .AddAsyncConsumer<TestConsumer, string>()
            .Services
            .BuildServiceProvider();

        // act & assertion
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Для сообщения '{typeof(string).FullName}' можно добавить только один потребитель!");
    }

    /// <summary>
    /// Не успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void UnsuccessfulConsumerRegistrationForConcurrent()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddConcurrentTransport("Queue.Test", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<TestConsumer, string>()
            .AddAsyncConsumer<TestConsumer, string>()
            .Services
            .BuildServiceProvider();

        // act & assertion
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Для сообщения '{typeof(string).FullName}' можно добавить только один потребитель!");
    }

    /// <summary>
    /// Не успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void UnsuccessfulConsumerRegistrationForHybrid()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddInMemoryTransport("Queue.Test.1", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<TestConsumer, string>()
            .AddConcurrentTransport("Queue.Test.2", transportOptions =>
            {
                transportOptions.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
                transportOptions.Capacity = 200;
            })
            .AddAsyncConsumer<TestConsumer, string>()
            .Services
            .BuildServiceProvider();

        // act & assertion
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Для сообщения '{typeof(string).FullName}' можно добавить только один потребитель!");
    }

    /// <summary>
    /// Не успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void UnsuccessfulSingleRepeatedBusNameRegistration()
    {
        // arrange
        const string transportName = "Queue.Test";

        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddInMemoryTransport(transportName, transportOptions => { })
            .AddAsyncConsumer<FirstConsumer, FirstMessage>()
            .AddInMemoryTransport(transportName, transportOptions => { })
            .AddAsyncConsumer<SecondConsumer, SecondMessage>()
            .Services
            .BuildServiceProvider()
            .GetRequiredService<IPublisher>();

        // act & assertion
        func.Should()
            .Throw<ArgumentException>()
            .WithMessage($"An item with the same key has already been added. Key: {transportName}");
    }

    /// <summary>
    /// Не успешная регистрация компонентов шины.
    /// </summary>
    [Test]
    public void UnsuccessfulHybridRepeatedBusNameRegistration()
    {
        // arrange
        const string transportName = "Queue.Test";

        var func = () => new ServiceCollection()
            .AddMessageLogger()
            .AddBus(busOptions => { })
            .AddInMemoryTransport(transportName, transportOptions => { })
            .AddAsyncConsumer<FirstConsumer, FirstMessage>()
            .AddConcurrentTransport(transportName, transportOptions => { })
            .AddAsyncConsumer<SecondConsumer, SecondMessage>()
            .Services
            .BuildServiceProvider()
            .GetRequiredService<IPublisher>();

        // act & assertion
        func.Should()
            .Throw<ArgumentException>()
            .WithMessage($"An item with the same key has already been added. Key: {transportName}");
    }
}