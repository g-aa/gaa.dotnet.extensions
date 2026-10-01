using Microsoft.Extensions.Logging;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Категории журналов для <see cref="ILogger"/>.
/// </summary>
internal static class CategoryName
{
    /// <summary>
    /// Категория логирования для транспортной шины в памяти.
    /// </summary>
    internal const string InMemory = "Gaa.Extensions.Observer.Transport.InMemory";

    /// <summary>
    /// Категория логирования для конкурентной транспортной шины в памяти.
    /// </summary>
    internal const string Concurrent = "Gaa.Extensions.Observer.Transport.Concurrent";
}