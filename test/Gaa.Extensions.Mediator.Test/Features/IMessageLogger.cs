namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Регистратор сообщений.
/// </summary>
public interface IMessageLogger
{
    /// <summary>
    /// Зарегистрировать сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    void Log(string message);
}