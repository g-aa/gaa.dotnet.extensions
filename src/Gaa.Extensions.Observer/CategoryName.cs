using Microsoft.Extensions.Logging;

namespace Gaa.Extensions;

/// <summary>
/// Категории журналов для <see cref="ILogger"/>.
/// </summary>
internal static class CategoryName
{
    /// <summary>
    /// Категория для публикатора данных <see cref="BusPublisher"/>.
    /// </summary>
    internal const string Publisher = "Gaa.Extensions.Bus.Publishing";

    /// <summary>
    /// Категория для исполнителя фоновых задач <see cref="BackgroundTaskExecutionService"/>.
    /// </summary>
    internal const string Executor = "Gaa.Extensions.Bus.Executing";
}