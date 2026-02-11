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
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediatorLite(this IServiceProvider provider)
    {
        return (IMediator)provider.GetRequiredService(typeof(IMediator));
    }

    /// <summary>
    /// Предоставляет <see cref="IMediator"/>.
    /// </summary>
    /// <param name="scope">Область видимости для сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediatorLite(this IServiceScope scope)
    {
        return (IMediator)scope.ServiceProvider.GetRequiredService(typeof(IMediator));
    }
}