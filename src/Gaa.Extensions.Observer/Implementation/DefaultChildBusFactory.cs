using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика дочерних шин по умолчанию.
/// </summary>
/// <remarks>Кэширует созданные шины.</remarks>
internal sealed class DefaultChildBusFactory : IChildBusFactory<DefaultChildBus>
{
    private readonly Lock _lock;

    private readonly IServiceProvider _provider;

    private readonly BusOptions _options;

    private readonly Dictionary<string, DefaultChildBus> _cachedBuses;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultChildBusFactory"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public DefaultChildBusFactory(IServiceProvider provider)
    {
        _lock = new();
        _provider = provider;
        _options = _provider.GetRequiredService<IOptions<BusOptions>>().Value;
        _cachedBuses = new(_options.Options.Count);
    }

    /// <inheritdoc />
    public DefaultChildBus GetOrCreate(string name)
    {
        return _cachedBuses.TryGetValue(name, out var childBus)
            ? childBus
            : Create(name);
    }

    private DefaultChildBus Create(string name)
    {
        lock (_lock)
        {
            DefaultChildBus? newChildBus;
            if (_cachedBuses.TryGetValue(name, out newChildBus))
            {
                return newChildBus;
            }

            var childOptions = _options.Options.FirstOrDefault(o => o.Name == name);
            if (childOptions == null)
            {
                throw new InvalidOperationException($"Неудалось найти настройки для шины '{name}'.");
            }

            var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
            newChildBus = new DefaultChildBus(loggerFactory, childOptions);
            _cachedBuses.Add(name, newChildBus);
            return newChildBus;
        }
    }
}