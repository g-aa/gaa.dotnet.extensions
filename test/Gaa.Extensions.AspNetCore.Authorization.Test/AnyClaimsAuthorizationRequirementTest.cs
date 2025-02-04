using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;

namespace Gaa.Extensions.AspNetCore.Authorization.Test;

/// <summary>
/// Набор тестов для <see cref="AnyClaimsAuthorizationRequirement"/>.
/// </summary>
[TestFixture]
public class AnyClaimsAuthorizationRequirementTest
{
    /// <summary>
    /// Успешная авторизация с конечной точкой равной null.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulHandleAsyncWithNullEndpoint()
    {
        // arrange
        var authorizationHandler = new AnyClaimsAuthorizationRequirement("scope");
        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement> { authorizationHandler, },
            new ClaimsPrincipal(),
            null);

        // act
        await authorizationHandler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }

    /// <summary>
    /// Успешная авторизация с пустыми метаданными конечной точки.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulHandleAsyncWithEmptyEndpointMetadata()
    {
        // arrange
        var authorizationHandler = new AnyClaimsAuthorizationRequirement("scope");
        var endpoint = new Endpoint(null, new EndpointMetadataCollection(), "test-endpoint");
        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement> { authorizationHandler, },
            new ClaimsPrincipal(),
            endpoint);

        // act
        await authorizationHandler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }

    /// <summary>
    /// Успешная авторизация для требования <see cref="AnyClaimsAuthorizationRequirement"/>.
    /// </summary>
    /// <param name="claimType">Тип утверждения.</param>
    /// <param name="acceptedValues">Требуемые значения.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("scope", "api:read,api:write")]
    [TestCase("SCOPE", "api:read,api:write")]
    public async Task SuccessfulHandleAsync(string claimType, string acceptedValues)
    {
        // arrange
        var authorizationHandler = new AnyClaimsAuthorizationRequirement("scope");
        var metadata = new EndpointMetadataCollection(
            new RequiredScopeAttribute("api:read"),
            new RequiredScopeAttribute("api:write", "api:update"));

        var endpoint = new Endpoint(null, metadata, "test-endpoint");
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                acceptedValues.Split(',').Select(value => new Claim(claimType, value)),
                "test-user"));

        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement> { authorizationHandler, },
            user,
            endpoint);

        // act
        await authorizationHandler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }

    /// <summary>
    /// Неуспешная авторизация с <see cref="ClaimsPrincipal"/> пользователя равным null.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task UnsuccessfulHandleAsyncWithNullUser()
    {
        // arrange
        var authorizationHandler = new AnyClaimsAuthorizationRequirement("scope");
        var endpoint = new Endpoint(null, new EndpointMetadataCollection(), "test-endpoint");
        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement> { authorizationHandler, },
            null!,
            endpoint);

        // act
        await authorizationHandler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeFalse();
    }

    /// <summary>
    /// Неуспешная авторизация для требования <see cref="AnyClaimsAuthorizationRequirement"/>.
    /// </summary>
    /// <param name="claimType">Тип утверждения.</param>
    /// <param name="acceptedValues">Требуемые значения.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("scope", "")]
    [TestCase("role", "api:read,api:write")]
    public async Task UnsuccessSuccessfulHandleAsync(string claimType, string acceptedValues)
    {
        // arrange
        var authorizationHandler = new AnyClaimsAuthorizationRequirement("scope");
        var metadata = new EndpointMetadataCollection(
            new RequiredScopeAttribute("api:read"),
            new RequiredScopeAttribute("api:write", "api:update"));

        var endpoint = new Endpoint(null, metadata, "test-endpoint");
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                acceptedValues.Split(',').Select(value => new Claim(claimType, value)),
                "test-user"));

        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement> { authorizationHandler, },
            user,
            endpoint);

        // act
        await authorizationHandler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeFalse();
    }
}