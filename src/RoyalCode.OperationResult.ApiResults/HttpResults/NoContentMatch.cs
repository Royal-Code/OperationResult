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
///     When success, returns a <see cref="NoContent"/>.
///     When error, returns a <see cref="MatchErrorResult"/>.
/// </para>
/// </summary>
public sealed class NoContentMatch : IResult, INestedHttpResult, IEndpointMetadataProvider
{
    /// <summary>
    /// Creates a new <see cref="NoContentMatch"/> for the <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NoContentMatch(OperationResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="NoContentMatch"/> for the <see cref="ValidableResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="ValidableResult"/> to be converted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NoContentMatch(ValidableResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="NoContentMatch"/> for the <see cref="NoContent"/>.
    /// </summary>
    /// <param name="result">The <see cref="NoContent"/> to be converted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NoContentMatch(NoContent result) => new(result);

    /// <summary>
    /// Creates a new <see cref="NoContentMatch"/> for the <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NoContentMatch(MatchErrorResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="NoContentMatch"/> for the <see cref="ResultMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NoContentMatch(ResultMessage message) => new(new MatchErrorResult(message));

    /// <summary>
    /// Creates a new <see cref="NoContentMatch"/> for the <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="errors"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NoContentMatch(ResultErrors errors) => new(new MatchErrorResult(errors));

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    public NoContentMatch(OperationResult result)
    {
        Result = result.Match<IResult>(
            TypedResults.NoContent,
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="OperationResult{T}"/>.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult{T}"/> to be converted.</param>
    public NoContentMatch(ValidableResult result)
    {
        Result = result.Match<IResult>(
            TypedResults.NoContent,
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="OkMatch{T}"/> for the <see cref="NoContent"/> match.
    /// </summary>
    /// <param name="result"></param>
    public NoContentMatch(NoContent result)
    {
        Result = result;
    }

    /// <summary>
    /// Creates a new <see cref="OkMatch{T}"/> for the <see cref="MatchErrorResult"/> match.
    /// </summary>
    /// <param name="result"></param>
    public NoContentMatch(MatchErrorResult result)
    {
        Result = result;
    }

    /// <inheritdoc/>
    public IResult Result { get; }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ResponseTypeMetadata(StatusCodes.Status204NoContent));
        MatchErrorResult.PopulateMetadata(method, builder);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task ExecuteAsync(HttpContext httpContext) => Result.ExecuteAsync(httpContext);
}

#endif