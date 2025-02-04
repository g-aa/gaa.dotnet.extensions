namespace Gaa.Extensions.AspNetCore.Authorization;

/// <summary>
/// Требуемые метаданные для авторизации.
/// </summary>
public interface IRequiredAuthorizationMetadata
{
    /// <summary>
    /// Перечень требуемых значений.
    /// </summary>
    IReadOnlyCollection<string> AcceptedValues { get; }
}