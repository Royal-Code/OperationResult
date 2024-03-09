using RoyalCode.OperationResults;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// Extensions for adapt <see cref="OperationResult"/> to <see cref="ObjectResult"/>.
/// </summary>
public static class MvcResults
{
    /// <summary>
    /// Convert a <see cref="OperationResult"/> to an <see cref="OperationMatchObjectResult"/>
    /// to be used in a MVC controller as a return type.
    /// </summary>
    /// <param name="result">The <see cref="OperationResult"/> to be converted.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <returns>The <see cref="OperationMatchObjectResult"/> for the response.</returns>
    public static OperationMatchObjectResult ToActionResult(this OperationResult result,
        string? createdPath = null)
        => new(result, createdPath);

    /// <summary>
    /// Convert a <see cref="OperationResult{TValue}"/> to an <see cref="OperationMatchObjectResult{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The <see cref="OperationResult{TValue}"/> to be converted.</param>
    /// <returns>The <see cref="OperationMatchObjectResult{TValue}"/> for the response.</returns>
    public static OperationMatchObjectResult<TValue> ToActionResult<TValue>(this OperationResult<TValue> result)
        => new(result);

    /// <summary>
    /// Convert a <see cref="OperationResult{TValue}"/> to an <see cref="OperationMatchObjectResult{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The <see cref="OperationResult{TValue}"/> to be converted.</param>
    /// <param name="createdPath">The path for created responses.</param>
    /// <param name="formatPathWithValue">Indicates if the <paramref name="createdPath"/> should be formatted with the value of the result.</param>
    /// <returns>The <see cref="OperationMatchObjectResult{TValue}"/> for the response.</returns>
    public static OperationMatchObjectResult<TValue> ToActionResult<TValue>(this OperationResult<TValue> result,
        string createdPath, bool formatPathWithValue = false)
        => new(result, createdPath, formatPathWithValue);

    /// <summary>
    /// Convert a <see cref="OperationResult{TValue}"/> to an <see cref="OperationMatchObjectResult{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The <see cref="OperationResult{TValue}"/> to be converted.</param>
    /// <param name="createdPathProvider">A function that provides the path for created responses.</param>
    /// <returns>The <see cref="OperationMatchObjectResult{TValue}"/> for the response.</returns>
    public static OperationMatchObjectResult<TValue> ToActionResult<TValue>(this OperationResult<TValue> result,
        Func<TValue, string> createdPathProvider)
        => new(result, createdPathProvider);

    /// <summary>
    /// Convert a <see cref="ValidableResult"/> to an <see cref="ValidableMatchObjectResult"/>
    /// to be used in a MVC controller as a return type.
    /// </summary>
    /// <param name="result">The <see cref="ValidableResult"/> to be converted.</param>
    /// <returns>The <see cref="ValidableMatchObjectResult"/> for the response.</returns>
    public static ValidableMatchObjectResult ToActionResult(this ValidableResult result)
        => new(result);
}