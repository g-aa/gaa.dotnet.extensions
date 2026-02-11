namespace Gaa.Extensions;

/// <summary>
/// Постпроцессор запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IAsyncRequestPostProcessor<in TRequest, in TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет обработку запроса и ответа.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="response">Ответ на запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ProcessAsync(TRequest request, TResponse response, CancellationToken cancellationToken);
}