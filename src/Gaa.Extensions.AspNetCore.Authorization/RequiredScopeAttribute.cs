namespace Gaa.Extensions.AspNetCore.Authorization;

/// <summary>
/// Атрибут с требуемыми значениями областей видимости.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequiredScopeAttribute
    : Attribute, IRequiredAuthorizationMetadata
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequiredScopeAttribute"/>.
    /// </summary>
    /// <param name="acceptedValues">Требуемые значения для области видимости.</param>
    public RequiredScopeAttribute(params string[] acceptedValues)
    {
        AcceptedValues = acceptedValues
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string> AcceptedValues { get; }
}