#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http.HttpResults;
#endif
using RoyalCode.OperationResults;
using RoyalCode.OperationResults.HttpResults;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// Extensions for adapt <see cref="OperationResult"/> to <see cref="IResult"/>.
/// </summary>
public static partial class ApiResults
{
#if NET6_0

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="IResult"/>.
    /// </summary>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static IResult ToResult(this IResultExtensions _, OperationResult result)
    {
        return result.Match(
            Results.NoContent,
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="IResult"/>.
    /// </summary>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static IResult ToResult(this IResultExtensions _, OperationResult result, string createdPath)
    {
        return result.Match(
            createdPath,
            static path => Results.Created(path, null),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="IResult"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static IResult ToResult<T>(this IResultExtensions _, OperationResult<T> result)
    {
        return result.Match(
            static value => Results.Ok(value),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="IResult"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <param name="formatPathWithValue">Indicates if the <paramref name="createdPath"/> should be formatted with the value of the result.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static IResult ToResult<T>(this IResultExtensions _,
        OperationResult<T> result, string createdPath, bool formatPathWithValue = false)
    {
        return result.Match(
            (createdPath, formatPathWithValue),
            static (value, tuple) => Results.Created(tuple.formatPathWithValue ? string.Format(tuple.createdPath, value) : tuple.createdPath, value),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="IResult"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPathFunction">A function to create the path for created responses.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static IResult ToResult<T>(this IResultExtensions _, 
        OperationResult<T> result, Func<T, string> createdPathFunction)
    {
        return result.Match(
            createdPathFunction,
            static (value, func) => Results.Created(func(value), value),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="IResult"/>.
    /// </summary>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static IResult ToResult(this IResultExtensions _, ValidableResult result)
    {
        return result.Match(
            Results.NoContent,
            static error => new MatchErrorResult(error));
    }

#else

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="NoContent"/> or <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static Results<NoContent, MatchErrorResult> ToResult(this IResultExtensions _, OperationResult result)
    {
        return result.Match<Results<NoContent, MatchErrorResult>>(
            static () => TypedResults.NoContent(),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="Created"/> or <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static Results<Created, MatchErrorResult> ToResult(this IResultExtensions _,
        OperationResult result, string createdPath)
    {
        return result.Match<Results<Created, MatchErrorResult>, string>(
            createdPath,
            static path => TypedResults.Created(path),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="Ok{TValue}"/> or <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="IResult"/> implementation for the response.</returns>
    public static Results<Ok<T>, MatchErrorResult> ToResult<T>(
        this IResultExtensions _, OperationResult<T> result)
    {
        return result.Match<Results<Ok<T>, MatchErrorResult>>(
            static value => TypedResults.Ok(value),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="Created{T}"/> or <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <param name="formatPathWithValue">Indicates if the <paramref name="createdPath"/> should be formatted with the value of the result.</param>
    /// <returns>The <see cref="IResult"/> implementation for the response.</returns>
    public static Results<Created<T>, MatchErrorResult> ToResult<T>(this IResultExtensions _,
        OperationResult<T> result, string createdPath, bool formatPathWithValue = false)
    {
        return result.Match<Results<Created<T>, MatchErrorResult>, (string path, bool format)>(
            (createdPath, formatPathWithValue),
            static (value, tuple) => TypedResults.Created(tuple.format ? string.Format(tuple.path, value) : tuple.path, value),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{T}"/> to <see cref="Created{T}"/> or <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPathFunction">A function to create the path for created responses.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static Results<Created<T>, MatchErrorResult> ToResult<T>(this IResultExtensions _,
        OperationResult<T> result, Func<T, string> createdPathFunction)
    {
        return result.Match<Results<Created<T>, MatchErrorResult>, Func<T, string>>(
            createdPathFunction,
            static (value, func) => TypedResults.Created(func(value), value),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="ValidableResult{T}"/> to <see cref="NoContent"/> or <see cref="MatchErrorResult"/>.
    /// </summary>
    /// <param name="_">Used for extension.</param>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="IResult"/> for the response.</returns>
    public static Results<NoContent, MatchErrorResult> ToResult(this IResultExtensions _, ValidableResult result)
    {
        return result.Match<Results<NoContent, MatchErrorResult>>(
            static () => TypedResults.NoContent(),
            static error => new MatchErrorResult(error));
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{TValue}"/> to <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch{T}"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPathFunction">A function to create the path for created responses.</param>
    /// <returns>The <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch{T}"/> for the response.</returns>
    public static CreatedMatch<T> CreatedMatch<T>(this OperationResult<T> result, Func<T, string> createdPathFunction)
    {
        return new CreatedMatch<T>(result, createdPathFunction);
    }

    /// <summary>
    /// Convert the <see cref="OperationResult{TValue}"/> to <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch{T}"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <param name="formatPathWithValue">Indicates if the <paramref name="createdPath"/> should be formatted with the value of the result.</param>
    /// <returns>The <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch{T}"/> for the response.</returns>
    public static CreatedMatch<T> CreatedMatch<T>(this OperationResult<T> result, string createdPath, bool formatPathWithValue = false)
    {
        return new CreatedMatch<T>(result, createdPath, formatPathWithValue);
    }

    /// <summary>
    /// Convert the <see cref="OperationResult"/> to <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch"/>.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <returns>The <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch"/> for the response.</returns>
    public static CreatedMatch CreatedMatch(this OperationResult result, string createdPath)
    {
        return new CreatedMatch(result, createdPath);
    }

    /// <summary>
    /// Convert the <see cref="OperationResult"/> to <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The operation result.</param>
    /// <returns>The <see cref="RoyalCode.OperationResults.HttpResults.CreatedMatch"/> for the response.</returns>
    public static OkMatch<T> OkMatch<T>(this OperationResult<T> result)
    {
        return new OkMatch<T>(result);
    }

#endif
}