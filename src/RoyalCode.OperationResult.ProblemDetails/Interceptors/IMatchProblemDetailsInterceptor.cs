using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults.Interceptors;

/// <summary>
/// A interceptor for the <see cref="ProblemDetails"/>.
/// </summary>
public interface IMatchProblemDetailsInterceptor
{
    /// <summary>
    /// Interceptor for the <see cref="ProblemDetails"/> when is being writed as a result.
    /// </summary>
    /// <param name="problemDetails">The <see cref="ProblemDetails"/> to be writed.</param>
    void WritingProblemDetails(ProblemDetails problemDetails, ResultErrors errors);
}