using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст для конфигурирования <see cref="IPublisher"/>.
/// </summary>
public sealed class BusConfigurationBuilder
{
    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services { get; init; } = null!;

    /// <summary>
    /// Регистрирует компоненты <see cref="IChildBus"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="busName">Наименование дочерней шины.</param>
    /// <param name="configureOptions">Настройки конфигурации дочерней шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public ChildBusConfigurationBuilder AddChildBus(
        string busName,
        Action<ChildBusOptions> configureOptions) => Services.AddChildBus(busName, configureOptions);
}