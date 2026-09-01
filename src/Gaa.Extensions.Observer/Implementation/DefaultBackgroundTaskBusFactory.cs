using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика дочерних шин по умолчанию.
/// </summary>
/// <remarks>Кэширует созданные шины.</remarks>
internal sealed class DefaultBackgroundTaskBusFactory : IBackgroundTaskBusFactory
{
    private readonly Lock _lock;

    private readonly IServiceProvider _provider;

    private readonly BusOptions _options;

    private readonly Dictionary<string, DefaultBackgroundTaskBus> _cachedBuses;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBackgroundTaskBusFactory"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public DefaultBackgroundTaskBusFactory(IServiceProvider provider)
    {
        _lock = new();
        _provider = provider;
        _options = _provider.GetRequiredService<IOptions<BusOptions>>().Value;
        _cachedBuses = new(_options.Options.Count);
    }

    /// <inheritdoc />
    public IBackgroundTaskBus GetOrCreate(string busName)
    {
        return _cachedBuses.TryGetValue(busName, out var bus)
            ? bus
            : Create(busName);
    }

    private DefaultBackgroundTaskBus Create(string name)
    {
        lock (_lock)
        {
            DefaultBackgroundTaskBus? newBus;
            if (_cachedBuses.TryGetValue(name, out newBus))
            {
                return newBus;
            }

            var childOptions = _options.Options.FirstOrDefault(o => o.Name == name);
            if (childOptions == null)
            {
                throw new InvalidOperationException($"Неудалось найти настройки для шины '{name}'.");
            }

            var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
            newBus = new DefaultBackgroundTaskBus(loggerFactory, childOptions);
            _cachedBuses.Add(name, newBus);
            return newBus;
        }
    }
}