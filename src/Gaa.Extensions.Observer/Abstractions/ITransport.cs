#pragma warning disable IDE0130 // Namespace does not match folder structure

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Транспортная шина.
/// </summary>
public interface ITransport
{
    /// <summary>
    /// Наименование шины.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Добавляет транспортное сообщение в шину.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Контекст сообщения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task PublishAsync<TMessage>(MessageContext<TMessage> message, CancellationToken cancellationToken)
        where TMessage : notnull;

    /// <summary>
    /// Запускает транспортную шину на выполнение.
    /// </summary>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="stoppingToken">Токен останавливающий выполнение операции.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task RunAsync(IServiceScopeFactory scopeFactory, CancellationToken stoppingToken);

    /// <summary>
    /// Закрывает транспортную шину.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    void Close(CancellationToken cancellationToken);
}