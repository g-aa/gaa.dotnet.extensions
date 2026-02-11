namespace Gaa.Extensions;

/// <summary>
/// Препроцессор запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IAsyncRequestPreProcessor<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет обработку запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ProcessAsync(TRequest request, CancellationToken cancellationToken);
}