using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// Extension class to get the header of the result type from the request context.
/// </summary>
public static class HeaderExtensions
{
    /// <summary>
    /// The header name of the result type.
    /// </summary>
    public const string ErrorTypeHeaderName = "Error-ResponseType";

    /// <summary>
    /// Try get the header of the result type from the request context.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
    /// <param name="resultType">The result type.</param>
    /// <returns>True if the header was found, otherwise false.</returns>
    public static bool TryGetResultTypeHeader(this HttpContext httpContext,
        [NotNullWhen(true)] out string? resultType)
    {
        if (httpContext.Request.Headers.TryGetValue(ErrorTypeHeaderName, out var resultHeader))
        {
            resultType = resultHeader!;
            return true;
        }

        resultType = null;
        return false;
    }
}
