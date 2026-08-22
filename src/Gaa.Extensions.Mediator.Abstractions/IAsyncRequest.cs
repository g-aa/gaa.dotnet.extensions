namespace Gaa.Extensions.Mediator;

/// <summary>
/// Маркер интерфейс запроса.
/// </summary>
public interface IAsyncRequest;

/// <summary>
/// Маркер интерфейс запроса с ответом.
/// </summary>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IAsyncRequest<out TResponse>;