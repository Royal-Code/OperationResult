
using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults;

/// <summary>
/// Enum to determine the result type returned by the API when an error occurs.
/// </summary>
public enum ErrorResultTypes
{
    /// <summary>
    /// The API will return the <see cref="OperationResult"/> when an error occurs.
    /// </summary>
    AlwaysOperationResult,

    /// <summary>
    /// The API will return the <see cref="ProblemDetails"/> when an error occurs.
    /// </summary>
    AlwaysProblemDetails,

    /// <summary>
    /// The API can return the <see cref="OperationResult"/> or <see cref="ProblemDetails"/> when an error occurs,
    /// and the default result is <see cref="OperationResult"/>.
    /// </summary>
    OperationResultAsDefault,

    /// <summary>
    /// The API can return the <see cref="OperationResult"/> or <see cref="ProblemDetails"/> when an error occurs,
    /// and the default result is <see cref="ProblemDetails"/>.
    /// </summary>
    ProblemDetailsAsDefault
}