using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст <see cref="IPublisher"/> для конфигурирования.
/// </summary>
public sealed class BusConfigurationBuilder
{
    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services { get; init; } = null!;

    /// <summary>
    /// Регистрирует асинхронный потребитель вида <see cref="IAsyncConsumer{TMessage}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="TConsumer">Тип потребителя сообщений.</typeparam>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public BusConfigurationBuilder AddAsyncConsumer<TConsumer, TMessage>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TConsumer : class, IAsyncConsumer<TMessage>
        where TMessage : notnull
    {
        return Add<TMessage, IAsyncConsumer<TMessage>, TConsumer>(lifetime);
    }

    private BusConfigurationBuilder Add<TMessage, TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConsumer>(
        ServiceLifetime lifetime)
        where TMessage : notnull
        where TInterface : class
        where TConsumer : class, TInterface
    {
        if (Services.Any(e => e.ServiceType == typeof(TInterface)))
        {
            var messageName = typeof(TMessage).FullName;
            throw new InvalidOperationException($"Для сообщения '{messageName}' можно добавить только один потребитель!");
        }

        Services.Add<TInterface, TConsumer>(lifetime);
        return this;
    }
}