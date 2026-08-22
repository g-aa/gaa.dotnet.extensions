#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фоновая задача.
/// </summary>
public interface IBackgroundTask
{
    /// <summary>
    /// Предьлное время обрабтки сообщения.
    /// </summary>
    TimeSpan? ExecutionTimeLimit { get; }

    /// <summary>
    /// Выполняет логику задачи.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken);
}