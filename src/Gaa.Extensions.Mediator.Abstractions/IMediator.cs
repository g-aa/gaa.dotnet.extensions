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
    void RequiredSend<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest, allows ref struct;

    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    TResponse RequiredSend<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>, allows ref struct
        where TResponse : allows ref struct;

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    Task RequiredSendAsync<TRequest>(
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
    Task<TResponse> RequiredSendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>;

    /// <summary>
    /// Публикует синхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    /// <remarks>Если обработчик запроса не будет найден, запрос будет проигнорирован.</remarks>
    void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest, allows ref struct;

    /// <summary>
    /// Публикует асинхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    /// <remarks>Если обработчик запроса не будет найден, запрос будет проигнорирован.</remarks>
    Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest;
}