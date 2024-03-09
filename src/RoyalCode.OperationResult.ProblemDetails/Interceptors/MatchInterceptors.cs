using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace RoyalCode.OperationResults.Interceptors;

/// <summary>
/// Internal class to manage the interceptors for the <see cref="ResultErrors"/> and <see cref="ProblemDetails"/>.
/// </summary>
public static class MatchInterceptors
{
    private static HasInterceptors hasErrorInterceptors;
    private static HasInterceptors hasProblemDetailsInterceptors;

    /// <summary>
    /// Invoked when the <see cref="ResultErrors"/> is being writed.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="errors">The <see cref="ResultErrors"/> to be writed.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WritingResultErrors(HttpContext httpContext, ResultErrors errors)
    {
        IEnumerable<IMatchErrorInterceptor> interceptors;

        switch (hasErrorInterceptors)
        {
            case HasInterceptors.Unknown:
                hasErrorInterceptors = TryLoad(httpContext.RequestServices, out interceptors);
                break;
            case HasInterceptors.True:
                interceptors = httpContext.RequestServices.GetRequiredService<IEnumerable<IMatchErrorInterceptor>>();
                break;
            default:
                return;
        }

        foreach (var interceptor in interceptors)
            try
            {
                interceptor.WritingResultErrors(errors);
            }
            catch (Exception ex)
            {
                var logger = httpContext.RequestServices.GetService<ILogger<IMatchErrorInterceptor>>()!;
                logger.LogError(ex, "Error executing the interceptor {Interceptor}",
                    interceptor.GetType().FullName);
            }
    }

    /// <summary>
    /// Invoked when the <see cref="ProblemDetails"/> is being writed.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="problemDetails">The <see cref="ProblemDetails"/> to be writed.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WritingProblemDetails(HttpContext httpContext, ProblemDetails problemDetails, ResultErrors errors)
    {
        IEnumerable<IMatchProblemDetailsInterceptor> interceptors;

        switch (hasProblemDetailsInterceptors)
        {
            case HasInterceptors.Unknown:
                hasProblemDetailsInterceptors = TryLoad(httpContext.RequestServices, out interceptors);
                break;
            case HasInterceptors.True:
                interceptors = httpContext.RequestServices.GetRequiredService<IEnumerable<IMatchProblemDetailsInterceptor>>();
                break;
            default:
                return;
        }

        foreach (var interceptor in interceptors)
            try
            {
                interceptor.WritingProblemDetails(problemDetails, errors);
            }
            catch (Exception ex)
            {
                var logger = httpContext.RequestServices.GetService<ILogger<IMatchErrorInterceptor>>()!;
                logger.LogError(ex, "Error executing the interceptor {Interceptor}",
                    interceptor.GetType().FullName);
            }
    }

    private static HasInterceptors TryLoad<T>(IServiceProvider requestServices, out IEnumerable<T> interceptors)
    {
        // get the interceptors from the request services
        interceptors = requestServices.GetService<IEnumerable<T>>() ?? Enumerable.Empty<T>();

        // check if has interceptors
        return interceptors.Any() ? HasInterceptors.True : HasInterceptors.False;
    }

    private enum HasInterceptors : byte
    {
        Unknown = 0,
        True = 1,
        False = 2
    }
}