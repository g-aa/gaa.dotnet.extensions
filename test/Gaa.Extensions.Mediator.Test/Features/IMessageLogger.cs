namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1515 // Рассмотрите возможность сделать общедоступные типы внутренними

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