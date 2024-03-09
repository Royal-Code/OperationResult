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
///     When success, returns a <see cref="Created"/>.
///     When error, returns a <see cref="MatchErrorResult"/>.
/// </para>
/// </summary>
public sealed class CreatedMatch : IResult, INestedHttpResult, IEndpointMetadataProvider
{
    /// <summary>
    /// Creates a new <see cref="CreatedMatch"/> for the <see cref="Created"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch(Created result) => new(result);

    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch(MatchErrorResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="CreatedMatch"/> for the <see cref="ResultMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch(ResultMessage message) => new(new MatchErrorResult(message));

    /// <summary>
    /// Creates a new <see cref="CreatedMatch"/> for the <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="errors"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch(ResultErrors errors) => new(new MatchErrorResult(errors));

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    /// <param name="createdPath">The location of the created resource.</param>
    public CreatedMatch(OperationResult result, string createdPath)
    {
        Result = result.Match<IResult, string>(
            createdPath,
            TypedResults.Created,
            static (error, uri) => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="ValidableResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="ValidableResult"/> to be converted.</param>
    /// <param name="createdPath">The location of the created resource.</param>
    public CreatedMatch(ValidableResult result, string createdPath)
    {
        Result = result.Match<IResult, string>(
            createdPath,
            TypedResults.Created,
            static (error, uri) => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="CreatedMatch"/> for the <see cref="Created"/> match.
    /// </summary>
    /// <param name="result">The <see cref="Created"/> to be converted.</param>
    public CreatedMatch(Created result)
    {
        Result = result;
    }

    /// <summary>
    /// Creates a new <see cref="CreatedMatch"/> for the <see cref="MatchErrorResult"/> match.
    /// </summary>
    /// <param name="result">The <see cref="MatchErrorResult"/> to be converted.</param>
    public CreatedMatch(MatchErrorResult result)
    {
        Result = result;
    }

    /// <inheritdoc/>
    public IResult Result { get; }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ResponseTypeMetadata(StatusCodes.Status201Created));
        MatchErrorResult.PopulateMetadata(method, builder);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task ExecuteAsync(HttpContext httpContext) => Result.ExecuteAsync(httpContext);
}

#endif