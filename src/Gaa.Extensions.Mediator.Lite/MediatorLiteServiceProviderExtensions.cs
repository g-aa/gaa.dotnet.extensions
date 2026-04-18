using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceProvider"/>.
/// </summary>
public static class MediatorLiteServiceProviderExtensions
{
    /// <summary>
    /// Предоставляет <see cref="IMediator"/>.
    /// </summary>
    /// <param name="scope">Область видимости для сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediatorLite(this IServiceScope scope)
    {
        return (IMediator)scope.ServiceProvider.GetRequiredService(typeof(IMediator));
    }

    /// <summary>
    /// Предоставляет <see cref="IMediator"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediatorLite(this IServiceProvider provider)
    {
        return (IMediator)provider.GetRequiredService(typeof(IMediator));
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
}