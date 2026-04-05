namespace Gaa.Extensions;

#pragma warning disable CA1040 // Avoid empty interfaces

/// <summary>
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IRequestHandler<in TRequest, out TResponse> : IBaseRequestHandler<TRequest>
    where TRequest : IRequest<TResponse>, allows ref struct
    where TResponse : allows ref struct
{
    /// <summary>
    /// Выполняет обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    TResponse Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IRequestHandler<in TRequest> : IBaseRequestHandler<TRequest>
    where TRequest : IRequest, allows ref struct
{
    /// <summary>
    /// Выполняет обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    void Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Базовый интерфейс обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IBaseRequestHandler<in TRequest>
    where TRequest : allows ref struct;