#pragma warning disable IDE0130 // Namespace does not match folder structure

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст выполнения для сообщения.
/// </summary>
internal interface IMessageExecutionContext
{
    /// <summary>
    /// Выполняет логику.
    /// </summary>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ExecuteAsync(IServiceScopeFactory scopeFactory, CancellationToken cancellationToken);
}