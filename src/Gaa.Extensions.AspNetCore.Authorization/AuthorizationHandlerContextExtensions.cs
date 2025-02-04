using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Gaa.Extensions.AspNetCore.Authorization;

/// <summary>
/// Методы расширения <see cref="AuthorizationHandlerContext"/>.
/// </summary>
internal static class AuthorizationHandlerContextExtensions
{
    /// <summary>
    /// Предоставляет перечень требуемых значений из <see cref="IRequiredAuthorizationMetadata"/> для текущего запроса.
    /// </summary>
    /// <typeparam name="T">Тип метаданных.</typeparam>
    /// <param name="context">Контекст авторизации.</param>
    /// <returns>Перечень требуемых значений.</returns>
    internal static IReadOnlyCollection<string>? GetAcceptedValues<T>(
        this AuthorizationHandlerContext context)
        where T : class, IRequiredAuthorizationMetadata
    {
        // The resource is either the HttpContext or the Endpoint directly
        // when used with the authorization middleware.
        var endpoint = context.Resource switch
        {
            HttpContext httpContext => httpContext.GetEndpoint(),
            Endpoint ep => ep,
            _ => null,
        };

        return endpoint?.Metadata
          .GetOrderedMetadata<T>()
          .SelectMany(data => data.AcceptedValues)
          .ToList();
    }
}