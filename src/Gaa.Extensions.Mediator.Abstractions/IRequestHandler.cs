namespace Gaa.Extensions;

#pragma warning disable CA1040 // Avoid empty interfaces

/// <summary>
/// Базовый интерфейс обработчика запросов.
/// </summary>
public interface IRequestHandler;

/// <summary>
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IRequestHandler<in TRequest> : IRequestHandler
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
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IRequestHandler<in TRequest, out TResponse> : IRequestHandler
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