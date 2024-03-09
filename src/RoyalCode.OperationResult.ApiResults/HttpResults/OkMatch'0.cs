#if NET7_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using RoyalCode.OperationResults.Metadata;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RoyalCode.OperationResults.HttpResults;

/// <summary>
/// <para>
///     A <see cref="IResult"/> for <see cref="OperationResult"/> match for the success or error case.
/// </para>
/// <para>
///     When success, returns a <see cref="Ok"/>.
///     When error, returns a <see cref="MatchErrorResult"/>.
/// </para>
/// </summary>
public sealed class OkMatch : IResult, INestedHttpResult, IEndpointMetadataProvider
{
    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OkMatch(OperationResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="ValidableResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="ValidableResult"/> to be converted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OkMatch(ValidableResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="Ok"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OkMatch(Ok result) => new(result);

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OkMatch(MatchErrorResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="ResultMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OkMatch(ResultMessage message) => new(new MatchErrorResult(message));

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="errors"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OkMatch(ResultErrors errors) => new(new MatchErrorResult(errors));

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="OperationResult"/> match.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    public OkMatch(OperationResult result)
    {
        Result = result.Match(
            static () => Results.Ok(),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="Ok"/> match.
    /// </summary>
    /// <param name="result"></param>
    public OkMatch(Ok result)
    {
        Result = result;
    }

    /// <summary>
    /// Creates a new <see cref="OkMatch"/> for the <see cref="MatchErrorResult"/> match.
    /// </summary>
    /// <param name="result"></param>
    public OkMatch(MatchErrorResult result)
    {
        Result = result;
    }

    /// <inheritdoc/>
    public IResult Result { get; }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ResponseTypeMetadata(StatusCodes.Status200OK));
        MatchErrorResult.PopulateMetadata(method, builder);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task ExecuteAsync(HttpContext httpContext) => Result.ExecuteAsync(httpContext);
}

#endif