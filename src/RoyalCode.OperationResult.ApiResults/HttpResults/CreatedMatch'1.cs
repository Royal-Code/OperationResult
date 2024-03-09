#if NET7_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using RoyalCode.OperationResults.Metadata;
using System.Net.Mime;
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
/// <typeparam name="T">The returned value type when success.</typeparam>
public sealed class CreatedMatch<T> : IResult, INestedHttpResult, IEndpointMetadataProvider
{
    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="Created{TValue}"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch<T>(Created<T> result) => new(result);

    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="result"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch<T>(MatchErrorResult result) => new(result);

    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="ResultMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch<T>(ResultMessage message) => new(new MatchErrorResult(message));

    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="errors"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CreatedMatch<T>(ResultErrors errors) => new(new MatchErrorResult(errors));

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    /// <param name="createdPath">The location of the created resource.</param>
    /// <param name="formatPathWithValue">Indicates if the <paramref name="createdPath"/> should be formatted with the value of the result.</param>
    public CreatedMatch(OperationResult<T> result, string createdPath, bool formatPathWithValue = false)
    {
        Result = result.Match<IResult>(
            value => TypedResults.Created(formatPathWithValue ? string.Format(createdPath, value) : createdPath, value),
            error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> for the <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    /// <param name="createdPathFunction">The function to create the location of the created resource.</param>
    public CreatedMatch(OperationResult<T> result, Func<T, string> createdPathFunction)
    {
        Result = result.Match<IResult>(
            value => TypedResults.Created(createdPathFunction(value), value),
            error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="Created{TValue}"/> match.
    /// </summary>
    /// <param name="result">The <see cref="Created{TValue}"/> to be converted.</param>
    public CreatedMatch(Created<T> result)
    {
        Result = result;
    }

    /// <summary>
    /// Creates a new <see cref="CreatedMatch{T}"/> for the <see cref="MatchErrorResult"/> match.
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
        builder.Metadata.Add(new ResponseTypeMetadata(typeof(T), StatusCodes.Status201Created, MediaTypeNames.Application.Json));
        MatchErrorResult.PopulateMetadata(method, builder);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task ExecuteAsync(HttpContext httpContext) => Result.ExecuteAsync(httpContext);
}

#endif