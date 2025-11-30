using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст <see cref="IBus"/> для конфигурирования.
/// </summary>
public sealed class BusConfigurationContext
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
    /// <returns>Контекст конфигурирования.</returns>
    /// <remarks>Потребитель регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public BusConfigurationContext AddAsyncConsumer<TConsumer, TMessage>()
        where TConsumer : class, IAsyncConsumer<TMessage>
        where TMessage : notnull
    {
        IsSingleUse<IAsyncConsumer<TMessage>, TMessage>();
        Services.AddTransient<IAsyncConsumer<TMessage>, TConsumer>();
        return this;
    }

    private void IsSingleUse<TConsumer, TMessage>()
        where TConsumer : class
        where TMessage : notnull
    {
        if (Services.Any(e => e.ServiceType == typeof(TConsumer)))
        {
            var messageName = typeof(TMessage).FullName;
            throw new InvalidOperationException($"Для сообщения '{messageName}' можно добавить только один потребитель!");
        }
    }
}