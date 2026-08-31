using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст для конфигурирования <see cref="IChildBus"/>.
/// </summary>
public sealed class ChildBusConfigurationBuilder
{
    private readonly string _busName;

    private readonly IServiceCollection _services;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ChildBusConfigurationBuilder"/>.
    /// </summary>
    /// <param name="busName">Наименование дочерней шины.</param>
    /// <param name="services">коллекция сервисов.</param>
    internal ChildBusConfigurationBuilder(string busName, IServiceCollection services)
    {
        _busName = busName;
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
    public ChildBusConfigurationBuilder AddAsyncConsumer<TConsumer, TMessage>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TConsumer : class, IAsyncConsumer<TMessage>
        where TMessage : notnull
    {
        return Add<TMessage, IAsyncConsumer<TMessage>, TConsumer>(lifetime);
    }

    /// <summary>
    /// Регистрирует компоненты <see cref="IChildBus"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="busName">Наименование дочерней шины.</param>
    /// <param name="configureOptions">Настройки конфигурации дочерней шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public ChildBusConfigurationBuilder AddChildBus(
        string busName,
        Action<ChildBusOptions> configureOptions) => _services.AddChildBus(busName, configureOptions);

    private ChildBusConfigurationBuilder Add<TMessage, TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConsumer>(
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
            options.Subscriptions[_busName].Add(typeof(TMessage));
        });

        return this;
    }
}