using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст для конфигурирования <see cref="ITransport"/>.
/// </summary>
public sealed class TransportConfigurationBuilder
{
    private readonly string _transportName;

    private readonly IServiceCollection _services;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="TransportConfigurationBuilder"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="services">Коллекция сервисов.</param>
    internal TransportConfigurationBuilder(string transportName, IServiceCollection services)
    {
        _transportName = transportName;
        _services = services;
    }

    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services => _services;

    /// <summary>
    /// Регистрирует асинхронный потребитель вида <see cref="IAsyncConsumer{TMessage}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="TConsumer">Тип потребителя сообщений.</typeparam>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public TransportConfigurationBuilder AddAsyncConsumer<TConsumer, TMessage>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TConsumer : class, IAsyncConsumer<TMessage>
        where TMessage : notnull
    {
        return Add<TMessage, IAsyncConsumer<TMessage>, TConsumer>(lifetime);
    }

    /// <summary>
    /// Регистрирует компоненты транспортной шины в памяти в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="configureOptions">Настройки транспортной шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public TransportConfigurationBuilder InMemoryTransport(
        string transportName,
        Action<InMemoryTransportOptions> configureOptions) => Services.InMemoryTransport(transportName, configureOptions);

    private TransportConfigurationBuilder Add<TMessage, TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConsumer>(
        ServiceLifetime lifetime)
        where TMessage : notnull
        where TInterface : class
        where TConsumer : class, TInterface
    {
        if (_services.Any(e => e.ServiceType == typeof(TInterface)))
        {
            var messageName = typeof(TMessage).FullName;
            throw new InvalidOperationException($"Для сообщения '{messageName}' можно добавить только один потребитель!");
        }

        _services.Add<TInterface, TConsumer>(lifetime);
        _services.Configure<BusOptions>(options =>
        {
            options.Subscriptions[_transportName].Add(typeof(TMessage));
        });

        return this;
    }
}