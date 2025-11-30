namespace Gaa.Extensions;

/// <summary>
/// Фоновая задача.
/// </summary>
public interface IBackgroundTask
{
    /// <summary>
    /// Выполняет логику задачи.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken);
}