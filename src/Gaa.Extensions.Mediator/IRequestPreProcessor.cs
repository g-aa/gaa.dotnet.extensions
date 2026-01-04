namespace Gaa.Extensions;

/// <summary>
/// Препроцессор запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public interface IRequestPreProcessor<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет обработку запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    void Process(TRequest request, CancellationToken cancellationToken);
}