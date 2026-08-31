using Microsoft.Extensions.Logging;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Категории журналов для <see cref="ILogger"/>.
/// </summary>
internal static class CategoryName
{
    /// <summary>
    /// Категория для <see cref="DefaultChildBus"/>, <see cref="DefaultBusExecutor"/>.
    /// </summary>
    internal const string DefaultBus = "Gaa.Extensions.Observer.Default.Bus";
}