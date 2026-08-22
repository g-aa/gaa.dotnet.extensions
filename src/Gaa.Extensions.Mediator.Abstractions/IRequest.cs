namespace Gaa.Extensions.Mediator;

/// <summary>
/// Маркер интерфейс запроса.
/// </summary>
public interface IRequest;

/// <summary>
/// Маркер интерфейс запроса с ответом.
/// </summary>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IRequest<out TResponse>
    where TResponse : allows ref struct;