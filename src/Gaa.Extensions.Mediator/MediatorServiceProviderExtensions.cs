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
    /// <param name="provider">Провайдер сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediator(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IMediator>();
    }

    /// <summary>
    /// Предоставляет <see cref="IMediator"/>.
    /// </summary>
    /// <param name="scope">Область видимости для сервисов.</param>
    /// <returns>Медиатор, посредник.</returns>
    public static IMediator GetMediator(this IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<IMediator>();
    }
}