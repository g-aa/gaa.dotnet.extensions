#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст выполнения для сообщения.
/// </summary>
internal interface IMessageExecutionContext
{
    /// <summary>
    /// Выполняет логику.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken);
}