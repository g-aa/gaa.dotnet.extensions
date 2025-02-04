using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.AspNetCore.Authorization.Test;

/// <summary>
/// Набор тестов для <see cref="AuthorizationPolicyBuilderExtensions"/>.
/// </summary>
public class AuthorizationPolicyBuilderTest
{
    private ServiceProvider _serviceProvider;

    private IAuthorizationPolicyProvider _policyProvider;

    /// <summary>
    /// Настраивает тестовое окружение.
    /// </summary>
    [OneTimeSetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                "any-claims",
                builder => builder.RequireAnyClaims("role"))
            .AddPolicy(
                "any-scopes",
                builder => builder.RequireAnyScopes())
            .AddPolicy(
                "all-claims",
                builder => builder.RequireAllClaims("role"))
            .AddPolicy(
                "all-scopes",
                builder => builder.RequireAllScopes());

        _serviceProvider = services.BuildServiceProvider();
        _policyProvider = _serviceProvider
            .GetRequiredService<IAuthorizationPolicyProvider>();
    }

    /// <summary>
    /// Заключение.
    /// </summary>
    [OneTimeTearDown]
    public void Down()
    {
        _serviceProvider?.Dispose();
    }

    /// <summary>
    /// Успешная регистрация пользовательской политики в в провайдере политик.
    /// </summary>
    /// <param name="policyName">Наименование политики.</param>
    /// <param name="requirementType">Тип требования.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("any-claims", typeof(AnyClaimsAuthorizationRequirement))]
    [TestCase("any-scopes", typeof(AnyClaimsAuthorizationRequirement))]
    [TestCase("all-claims", typeof(AllClaimsAuthorizationRequirement))]
    [TestCase("all-scopes", typeof(AllClaimsAuthorizationRequirement))]
    public async Task SuccessfulAddCustomPolicy(string policyName, Type requirementType)
    {
        // act
        var policy = await _policyProvider.GetPolicyAsync(policyName);
        var requirement = policy?.Requirements.FirstOrDefault(e => e.GetType() == requirementType);

        // assert
        requirement.Should().NotBeNull();
    }
}