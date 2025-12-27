namespace Gaa.Extensions;

/// <summary>
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IAsyncRequestHandler<in TRequest>
    where TRequest : IAsyncRequest
{
    /// <summary>
    /// Выполняет обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IAsyncRequestHandler<in TRequest, TResponse>
    where TRequest : IAsyncRequest<TResponse>
{
    /// <summary>
    /// Выполняет обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}