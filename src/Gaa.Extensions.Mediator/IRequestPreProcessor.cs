namespace Gaa.Extensions;

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IRequestPreProcessor<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет предварительную обработку на запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    void Process(TRequest request, CancellationToken cancellationToken);
}