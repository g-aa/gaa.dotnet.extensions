using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public readonly ref struct AsyncRequestHandlerConfigurationBuilder<TRequest>
    where TRequest : notnull
{
    private readonly IServiceCollection _services;

    private readonly ServiceLifetime _lifetime;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestHandlerConfigurationBuilder{TRequest}"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл пред. и пост обработчиков запроса.</param>
    internal AsyncRequestHandlerConfigurationBuilder(
        IServiceCollection services,
        ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IAsyncRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public AsyncRequestHandlerConfigurationBuilder<TRequest> AddAsyncPreProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProcessor>()
        where TProcessor : class, IAsyncRequestPreProcessor<TRequest>
    {
        _services.Add<IAsyncRequestPreProcessor<TRequest>, TProcessor>(_lifetime);
        return this;
    }
}

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public readonly ref struct AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IServiceCollection _services;

    private readonly ServiceLifetime _lifetime;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestHandlerConfigurationBuilder{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл пред. и пост обработчиков запроса.</param>
    internal AsyncRequestHandlerConfigurationBuilder(
        IServiceCollection services,
        ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IAsyncRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse> AddAsyncPreProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProcessor>()
        where TProcessor : class, IAsyncRequestPreProcessor<TRequest>
    {
        _services.Add<IAsyncRequestPreProcessor<TRequest>, TProcessor>(_lifetime);
        return this;
    }

    /// <summary>
    /// Регистрирует постпроцессор вида <see cref="IAsyncRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TProcessor">Тип постпроцессора запросов.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse> AddAsyncPostProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProcessor>()
        where TProcessor : class, IAsyncRequestPostProcessor<TRequest, TResponse>
    {
        _services.Add<IAsyncRequestPostProcessor<TRequest, TResponse>, TProcessor>(_lifetime);
        return this;
    }
}