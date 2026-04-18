using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceProvider"/>.
/// </summary>
public static class MediatorServiceProviderExtensions
{
    /// <summary>
    /// Предоставляет <see cref="IMediator"/>.
    /// </summary>
    /// <param name="scope">Область видимости для сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediator(this IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    /// <summary>
    /// Предоставляет <see cref="IMediator"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediator(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IMediator>();
    }

    /// <summary>
    /// Предоставляет сервис типа <typeparamref name="TService"/> на основе типа <typeparamref name="TBaseService"/>.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TBaseService">Базовый тип сервиса.</typeparam>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Экземпляр сервиса.</returns>
    internal static TService? GetService<TService, TBaseService>(this IServiceProvider provider)
    {
        return (TService?)provider.GetService(typeof(TBaseService));
    }

    /// <summary>
    /// Предоставляет сервис типа <typeparamref name="TService"/> на основе типа <typeparamref name="TBaseService"/>.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TBaseService">Базовый тип сервиса.</typeparam>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Экземпляр сервиса.</returns>
    internal static TService GetRequiredService<TService, TBaseService>(this IServiceProvider provider)
        where TService : notnull
    {
        return (TService)provider.GetRequiredService(typeof(TBaseService));
    }

    /// <summary>
    /// Предоставляет коллекцию сервисов.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Коллекция с экземплярами сервисов.</returns>
    internal static TService[] GetServices<TService>(this IServiceProvider provider)
        where TService : notnull
    {
        return (TService[])provider.GetRequiredService(typeof(IEnumerable<TService>));
    }
}