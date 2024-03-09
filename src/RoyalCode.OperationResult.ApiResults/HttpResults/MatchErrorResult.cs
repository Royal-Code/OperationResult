using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RoyalCode.OperationResults.Convertion;
using RoyalCode.OperationResults.Interceptors;
using RoyalCode.OperationResults.Metadata;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;


namespace RoyalCode.OperationResults.HttpResults;

/// <summary>
/// <para>
///     Minimal API Result for <see cref="ResultErrors"/>.
/// </para>
/// <para>
///     Used for create a result from the <see cref="OperationResult"/> match for the error case.
/// </para>
/// </summary>
public class MatchErrorResult
#if NET7_0_OR_GREATER
    : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<ResultErrors>
#else
    : IResult
#endif
{
    /// <summary>
    /// Creates a new <see cref="MatchErrorResult"/> for the <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="errors">The <see cref="ResultErrors"/> to be converted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MatchErrorResult(ResultErrors errors) => new(errors);

    /// <summary>
    /// Creates a new <see cref="MatchErrorResult"/> for the <see cref="ResultMessage"/>.
    /// </summary>
    /// <param name="message">The <see cref="ResultMessage"/> to be converted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MatchErrorResult(ResultMessage message) => new(message);

    private readonly ResultErrors errors;

    /// <summary>
    /// Creates a new instance of <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="errors">The <see cref="ResultErrors"/> to be converted.</param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="errors"/> is <see langword="null"/>.
    /// </exception>
    public MatchErrorResult(ResultErrors errors)
    {
        this.errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    /// <summary>
    /// Creates a new instance of <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="message">The <see cref="ResultMessage"/> to be converted.</param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="message"/> is <see langword="null"/>.
    /// </exception>
    public MatchErrorResult(ResultMessage message)
    {
        errors = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <inheritdoc />
    public int? StatusCode => errors.GetHttpStatus();

    /// <inheritdoc />
    public object? Value => errors;

#if NET7_0_OR_GREATER

    /// <inheritdoc />
    ResultErrors? IValueHttpResult<ResultErrors>.Value => errors;

#endif

    /// <inheritdoc />
    public Task ExecuteAsync(HttpContext httpContext)
    {
        if (ErrorResultTypeOptions.IsFlexible)
        {
            httpContext.TryGetResultTypeHeader(out var resultType);
            return resultType switch
            {
                nameof(ProblemDetails) => WriteProblemDetails(httpContext),
                nameof(OperationResult) => WriteOperationResult(httpContext),
                _ => WriteDefault(httpContext)
            };
        }
        else
        {
            return WriteDefault(httpContext);
        }
    }

    /// <summary>
    /// <para>
    ///     Check if the default result is <see cref="OperationResult"/> or <see cref="ProblemDetails"/> 
    ///     and write the result.
    /// </para>
    /// <para>
    ///     For determinate the default result, 
    ///     use <see cref="ErrorResultTypeOptions.IsProblemDetailsDefault"/> static property.
    /// </para>
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public Task WriteDefault(HttpContext httpContext)
        => ErrorResultTypeOptions.IsProblemDetailsDefault
            ? WriteProblemDetails(httpContext)
            : WriteOperationResult(httpContext);

    /// <summary>
    /// Write the <see cref="ProblemDetails"/> result.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public Task WriteProblemDetails(HttpContext httpContext)
    {
        var options = httpContext.RequestServices.GetRequiredService<IOptions<ProblemDetailsOptions>>().Value;
        var problemDetails = errors.ToProblemDetails(options);
        JsonSerializerOptions? serializerOptions = null;

        MatchInterceptors.WritingProblemDetails(httpContext, problemDetails, errors);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status400BadRequest;
        return httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            serializerOptions,
            "application/problem+json",
            httpContext.RequestAborted);
    }

    /// <summary>
    /// Write the <see cref="OperationResult"/> result.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public Task WriteOperationResult(HttpContext httpContext)
    {
        MatchInterceptors.WritingResultErrors(httpContext, errors);

        httpContext.Response.StatusCode = errors.GetHttpStatus();

#if NET7_0_OR_GREATER
        return httpContext.Response.WriteAsJsonAsync(
            errors,
            errors.GetJsonTypeInfo(),
            MediaTypeNames.Application.Json,
            httpContext.RequestAborted);
#else
        return httpContext.Response.WriteAsJsonAsync(
            errors,
            errors.GetJsonSerializerOptions(),
            MediaTypeNames.Application.Json,
            httpContext.RequestAborted);
#endif
    }

    /// <inheritdoc />
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        Type type;
        string[] content;

        if (ErrorResultTypeOptions.IsFlexible || !ErrorResultTypeOptions.IsProblemDetailsDefault)
        {
            type = typeof(ResultErrors);
            content = new[] { MediaTypeNames.Application.Json };

            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status400BadRequest, content));
            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status404NotFound, content));
            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status409Conflict, content));
            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status422UnprocessableEntity, content));
        }

        if (ErrorResultTypeOptions.IsFlexible || ErrorResultTypeOptions.IsProblemDetailsDefault)
        {
            type = typeof(ProblemDetails);
            content = new[] { "application/problem+json" };

            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status400BadRequest, content));
            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status404NotFound, content));
            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status409Conflict, content));
            builder.Metadata.Add(
                new ResponseTypeMetadata(type, StatusCodes.Status422UnprocessableEntity, content));
        }
    }
}