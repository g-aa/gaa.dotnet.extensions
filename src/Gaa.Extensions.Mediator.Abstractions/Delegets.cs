namespace Gaa.Extensions;

#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Делегат представляющий продолжение обработки запроса.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <param name="provider">Провайдер сервисов.</param>
/// <param name="request">Запрос.</param>
/// <param name="cancellationToken">Токен отмены.</param>
public delegate void Continuation<in TRequest>(
    IServiceProvider provider,
    TRequest request,
    CancellationToken cancellationToken)
    where TRequest : allows ref struct;

/// <summary>
/// Делегат представляющий продолжение обработки запроса.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
/// <param name="provider">Провайдер сервисов.</param>
/// <param name="request">Запрос.</param>
/// <param name="cancellationToken">Токен отмены.</param>
/// <returns>Ответ на запрос.</returns>
public delegate TResponse Continuation<in TRequest, out TResponse>(
    IServiceProvider provider,
    TRequest request,
    CancellationToken cancellationToken)
    where TRequest : allows ref struct
    where TResponse : allows ref struct;