namespace Gaa.Extensions;

/// <summary>
/// Обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
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
public interface IRequestHandler<in TRequest, out TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Выполняет обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    TResponse Handle(TRequest request, CancellationToken cancellationToken);
}