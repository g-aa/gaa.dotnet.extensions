using Microsoft.AspNetCore.Authorization;

namespace Gaa.Extensions.AspNetCore.Authorization;

/// <summary>
/// Реализует <see cref="IAuthorizationHandler"/> и <see cref="IAuthorizationRequirement"/> с требованием,
/// чтобы текущий запрос содержал любое значение из <see cref="IRequiredAuthorizationMetadata"/> для указанного требования.
/// </summary>
public sealed class AnyClaimsAuthorizationRequirement
    : AuthorizationHandler<AnyClaimsAuthorizationRequirement>, IAuthorizationRequirement
{
    private readonly string _scopeType;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AnyClaimsAuthorizationRequirement"/>.
    /// </summary>
    /// <param name="claimType">Наименование типа области видимости.</param>
    public AnyClaimsAuthorizationRequirement(string claimType)
    {
        _scopeType = claimType;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{nameof(AnyClaimsAuthorizationRequirement)}:Request with any value for the claim.type:{_scopeType} from the {nameof(IRequiredAuthorizationMetadata)}";
    }

    /// <inheritdoc />
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AnyClaimsAuthorizationRequirement requirement)
    {
        if (context.User == null)
        {
            return Task.CompletedTask;
        }

        var acceptedValues = context.GetAcceptedValues<IRequiredAuthorizationMetadata>();
        if (acceptedValues == null || acceptedValues.Count == 0)
        {
            context.Succeed(this);
            return Task.CompletedTask;
        }

        var claims = context.User.FindAll(requirement._scopeType).ToList();
        if (claims.Count == 0)
        {
            return Task.CompletedTask;
        }

        if (acceptedValues.Intersect(claims.Select(s => s.Value)).Any())
        {
            context.Succeed(this);
        }

        return Task.CompletedTask;
    }
}