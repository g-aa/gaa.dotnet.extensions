using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Gaa.Extensions.AspNetCore.Authorization.Test;

/// <summary>
/// Набор тестов для <see cref="AuthorizationHandlerContextExtensions"/>.
/// </summary>
[TestFixture]
public class AuthorizationHandlerContextTest
{
    /// <summary>
    /// Неуспешный тест на получение требуемых значений из <see cref="IRequiredAuthorizationMetadata"/>.
    /// </summary>
    /// <remarks>Для <see cref="Endpoint"/> ровному NULL.</remarks>
    [Test]
    public void UnsuccessfulGetAcceptedValuesFromNullEndpoint()
    {
        // arrange
        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement>(),
            new ClaimsPrincipal(),
            null);

        // act
        var result = context.GetAcceptedValues<IRequiredAuthorizationMetadata>();

        // assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Неуспешный тест на получение требуемых значений из <see cref="IRequiredAuthorizationMetadata"/>.
    /// </summary>
    /// <remarks>Из <see cref="HttpContext"/>.</remarks>
    [Test]
    public void UnsuccessfulGetAcceptedValuesFromHttpContext()
    {
        // arrange
        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement>(),
            new ClaimsPrincipal(),
            new DefaultHttpContext());

        // act
        var result = context.GetAcceptedValues<IRequiredAuthorizationMetadata>();

        // assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Успешный тест на получение требуемых значений из <see cref="IRequiredAuthorizationMetadata"/>.
    /// </summary>
    /// <param name="valueCount">Счетчик требуемых значений.</param>
    /// <param name="acceptedValues">Требуемые значения.</param>
    /// <remarks>Из нескольких <see cref="RequiredScopeAttribute"/>.</remarks>
    [TestCase(0, "")]
    [TestCase(1, "api:read")]
    [TestCase(2, "api:read,api:write")]
    [TestCase(3, "api:read,api:write", "api:update")]
    public void SuccessfulGetAcceptedValues(int valueCount, params string[] acceptedValues)
    {
        // arrange
        var metadata = acceptedValues.Select(e => new RequiredScopeAttribute(e.Split(','))).ToList();
        var endpoint = new Endpoint(null, new EndpointMetadataCollection(metadata), "test-endpoint");
        var context = new AuthorizationHandlerContext(
            new List<IAuthorizationRequirement>(),
            new ClaimsPrincipal(),
            endpoint);

        // act
        var result = context.GetAcceptedValues<IRequiredAuthorizationMetadata>();

        // assert
        result.Should().NotBeNull().And.HaveCount(valueCount);
    }
}