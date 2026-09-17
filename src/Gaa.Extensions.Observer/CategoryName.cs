using Microsoft.Extensions.Logging;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Категории журналов для <see cref="ILogger"/>.
/// </summary>
internal static class CategoryName
{
    /// <summary>
    /// Категория логирования для шин по умолчанию.
    /// </summary>
    internal const string DefaultBus = "Gaa.Extensions.Observer.Default.Bus";
}