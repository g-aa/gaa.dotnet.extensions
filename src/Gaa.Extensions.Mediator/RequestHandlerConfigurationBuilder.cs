using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public readonly ref struct RequestHandlerConfigurationBuilder<TRequest>
    where TRequest : notnull, allows ref struct
{
    private readonly IServiceCollection _services;

    private readonly ServiceLifetime _lifetime;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestHandlerConfigurationBuilder{TRequest}"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл пред. и пост обработчиков запроса.</param>
    internal RequestHandlerConfigurationBuilder(
        IServiceCollection services,
        ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public RequestHandlerConfigurationBuilder<TRequest> AddPreProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProcessor>()
        where TProcessor : class, IRequestPreProcessor<TRequest>
    {
        _services.Add<IRequestPreProcessor<TRequest>, TProcessor>(_lifetime);
        return this;
    }
}

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public readonly ref struct RequestHandlerConfigurationBuilder<TRequest, TResponse>
    where TRequest : notnull, allows ref struct
    where TResponse : notnull, allows ref struct
{
    private readonly IServiceCollection _services;

    private readonly ServiceLifetime _lifetime;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestHandlerConfigurationBuilder{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл пред. и пост обработчиков запроса.</param>
    internal RequestHandlerConfigurationBuilder(
        IServiceCollection services,
        ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public RequestHandlerConfigurationBuilder<TRequest, TResponse> AddPreProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProcessor>()
        where TProcessor : class, IRequestPreProcessor<TRequest>
    {
        _services.Add<IRequestPreProcessor<TRequest>, TProcessor>(_lifetime);
        return this;
    }

    /// <summary>
    /// Регистрирует постпроцессор вида <see cref="IRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TProcessor">Тип постпроцессора запросов.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public RequestHandlerConfigurationBuilder<TRequest, TResponse> AddPostProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProcessor>()
        where TProcessor : class, IRequestPostProcessor<TRequest, TResponse>
    {
        _services.Add<IRequestPostProcessor<TRequest, TResponse>, TProcessor>(_lifetime);
        return this;
    }
}