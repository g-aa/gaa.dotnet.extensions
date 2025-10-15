namespace Gaa.Extensions;

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IAsyncRequestPreProcessor<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет предварительную обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ProcessAsync(TRequest request, CancellationToken cancellationToken);
}