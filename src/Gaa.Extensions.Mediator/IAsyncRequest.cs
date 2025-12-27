namespace Gaa.Extensions;

#pragma warning disable CA1040 // Avoid empty interfaces
#pragma warning disable S2326  // Unused type parameters should be removed

/// <summary>
/// Маркер интерфейс запроса.
/// </summary>
public interface IAsyncRequest
{
}

/// <summary>
/// Маркер интерфейс запроса с ответом.
/// </summary>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public interface IAsyncRequest<out TResponse>
{
}