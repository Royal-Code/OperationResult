using FluentValidation.Results;
using FluentValidation;
using System.Net;

namespace RoyalCode.OperationResults;

/// <summary>
/// Extensions methods for convert <see cref="ValidationResult"/> to <see cref="OperationResult"/>.
/// </summary>
public static class ValidationsOperationResultExtensions
{
    /// <summary>
    /// The default status code for errors.
    /// </summary>
    public static HttpStatusCode ErrorStatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;

    /// <summary>
    /// Converts to a <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="result">FluentValidation result.</param>
    /// <returns>Operation Result.</returns>
    public static OperationResult ToOperationResult(this ValidationResult result)
    {
        return result.IsValid 
            ? new() 
            : result.Errors.ToResultErrors();
    }

    /// <summary>
    /// Converts to a <see cref="OperationResult{TValue}"/>.
    /// </summary>
    /// <param name="result">FluentValidation result</param>
    /// <param name="value">A value to be included into the result</param>
    /// <returns>Operation Result.</returns>
    public static OperationResult<TValue> ToOperationResult<TValue>(this ValidationResult result, TValue value)
    {
        return result.IsValid
            ? value
            : result.Errors.ToResultErrors();
    }

    /// <summary>
    /// Convert a list of <see cref="ValidationFailure"/> to a <see cref="ResultErrors"/>
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static ResultErrors ToResultErrors(this IList<ValidationFailure> errors)
    {
        var result = new ResultErrors();

        for (var i = 0; i < errors.Count; i++)
        {
            var error = errors[i];
            switch (error.Severity)
            {
                case Severity.Error:
                    result += ResultMessage.Error(error.ErrorMessage, error.PropertyName, error.ErrorCode, ErrorStatusCode);
                    break;
                case Severity.Warning:
                case Severity.Info:
                    break;
                default:
                    result += ResultMessage.Error(error.ErrorMessage, error.PropertyName, error.ErrorCode, ErrorStatusCode);
                    break;
            }
        }

        return result;
    }
}
