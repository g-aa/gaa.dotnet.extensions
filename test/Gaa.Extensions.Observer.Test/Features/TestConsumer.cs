namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812

/// <summary>
/// Тестовый потребитель.
/// </summary>
internal sealed class TestConsumer
    : IAsyncConsumer<string>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="TestConsumer"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public TestConsumer(IMessageLogger log) => _log = log;

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<string> context)
    {
        _log.Log($"Получено сообщение: {context.Message}.");
        return Task.CompletedTask;
    }
}