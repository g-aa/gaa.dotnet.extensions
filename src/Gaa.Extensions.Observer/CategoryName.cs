using Microsoft.Extensions.Logging;

namespace Gaa.Extensions;

/// <summary>
/// Категории журналов для <see cref="ILogger"/>.
/// </summary>
internal static class CategoryName
{
    /// <summary>
    /// Категория для <see cref="BackgroundTaskQueue"/>, <see cref="DefaultBusExecutor"/>.
    /// </summary>
    internal const string DefaultBus = "Gaa.Extensions.Default.Bus";
}