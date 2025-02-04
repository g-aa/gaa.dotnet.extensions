using Microsoft.AspNetCore.Authorization;

namespace Gaa.Extensions.AspNetCore.Authorization;

/// <summary>
/// Методы расширения <see cref="AuthorizationPolicyBuilder"/>.
/// </summary>
public static class AuthorizationPolicyBuilderExtensions
{
    /// <summary>
    /// Добавляет в конструктор политик авторизации проверку:<br/>
    /// любое значение требования из <see cref="IRequiredAuthorizationMetadata"/>.
    /// </summary>
    /// <param name="builder">Конструктор политик авторизации.</param>
    /// <param name="claimType">Тип утверждения.</param>
    /// <returns>Модифицированный конструктор политик авторизации.</returns>
    public static AuthorizationPolicyBuilder RequireAnyClaims(
        this AuthorizationPolicyBuilder builder,
        string claimType)
    {
        builder.Requirements.Add(new AnyClaimsAuthorizationRequirement(claimType));
        return builder;
    }

    /// <summary>
    /// Добавляет в конструктор политик авторизации проверку:<br/>
    /// любая требуемая область видимости из <see cref="RequiredScopeAttribute"/>.
    /// </summary>
    /// <param name="builder">Конструктор политик авторизации.</param>
    /// <returns>Модифицированный конструктор политик авторизации.</returns>
    public static AuthorizationPolicyBuilder RequireAnyScopes(
        this AuthorizationPolicyBuilder builder)
    {
        builder.Requirements.Add(new AnyClaimsAuthorizationRequirement("scope"));
        return builder;
    }

    /// <summary>
    /// Добавляет в конструктор политик авторизации проверку:<br/>
    /// все значение требования из <see cref="IRequiredAuthorizationMetadata"/>.
    /// </summary>
    /// <param name="builder">Конструктор политик авторизации.</param>
    /// <param name="claimType">Тип утверждения.</param>
    /// <returns>Модифицированный конструктор политик авторизации.</returns>
    public static AuthorizationPolicyBuilder RequireAllClaims(
        this AuthorizationPolicyBuilder builder,
        string claimType)
    {
        builder.Requirements.Add(new AllClaimsAuthorizationRequirement(claimType));
        return builder;
    }

    /// <summary>
    /// Добавляет в конструктор политик авторизации проверку:<br/>
    /// все требуемые области видимости из <see cref="RequiredScopeAttribute"/>.
    /// </summary>
    /// <param name="builder">Конструктор политик авторизации.</param>
    /// <returns>Модифицированный конструктор политик авторизации.</returns>
    public static AuthorizationPolicyBuilder RequireAllScopes(
        this AuthorizationPolicyBuilder builder)
    {
        builder.Requirements.Add(new AllClaimsAuthorizationRequirement("scope"));
        return builder;
    }
}