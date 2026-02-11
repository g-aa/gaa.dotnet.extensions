namespace Gaa.Extensions;

/// <summary>
/// Медиатор, посредник.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest;

    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    TResponse Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>;

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest;

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>;
}