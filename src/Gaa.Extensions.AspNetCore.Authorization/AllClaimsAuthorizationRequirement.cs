using Microsoft.AspNetCore.Authorization;

namespace Gaa.Extensions.AspNetCore.Authorization;

/// <summary>
/// Реализует <see cref="IAuthorizationHandler"/> и <see cref="IAuthorizationRequirement"/> с требованием,
/// чтобы текущий запрос содержал все значение из <see cref="IRequiredAuthorizationMetadata"/> для указанного требования.
/// </summary>
public sealed class AllClaimsAuthorizationRequirement
    : AuthorizationHandler<AllClaimsAuthorizationRequirement>, IAuthorizationRequirement
{
    private readonly string _scopeType;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AllClaimsAuthorizationRequirement"/>.
    /// </summary>
    /// <param name="claimType">Наименование типа области видимости.</param>
    public AllClaimsAuthorizationRequirement(string claimType)
    {
        _scopeType = claimType;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{nameof(AllClaimsAuthorizationRequirement)}:Request with all value for the claim.type:{_scopeType} from the {nameof(IRequiredAuthorizationMetadata)}";
    }

    /// <inheritdoc />
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AllClaimsAuthorizationRequirement requirement)
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

        if (!acceptedValues.Except(claims.Select(s => s.Value)).Any())
        {
            context.Succeed(this);
        }

        return Task.CompletedTask;
    }
}