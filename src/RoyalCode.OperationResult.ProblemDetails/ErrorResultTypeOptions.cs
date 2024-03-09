
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults;

/// <summary>
/// Options used to determine if the default result is <see cref="OperationResult"/> or <see cref="ProblemDetails"/>.
/// </summary>
public static class ErrorResultTypeOptions
{
    /// <summary>
    /// Set the default result type.
    /// </summary>
    public static void SetResultType(ErrorResultTypes resultType)
    {
        switch (resultType)
        {
            case ErrorResultTypes.AlwaysOperationResult:
                IsFlexible = false;
                IsProblemDetailsDefault = false;
                break;
            case ErrorResultTypes.AlwaysProblemDetails:
                IsFlexible = false;
                IsProblemDetailsDefault = true;
                break;
            case ErrorResultTypes.OperationResultAsDefault:
                IsFlexible = true;
                IsProblemDetailsDefault = false;
                break;
            case ErrorResultTypes.ProblemDetailsAsDefault:
                IsFlexible = true;
                IsProblemDetailsDefault = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null);
        }
    }

    /// <summary>
    /// Determines if the API clients can decide the result type, informing the header <see cref="HeaderExtensions.ErrorTypeHeaderName"/>.
    /// </summary>
    public static bool IsFlexible { get; private set; } = false;

    /// <summary>
    /// Determines if the default result is <see cref="OperationResult"/> or <see cref="ProblemDetails"/>.
    /// </summary>
    public static bool IsProblemDetailsDefault { get; private set; } = false;
}
