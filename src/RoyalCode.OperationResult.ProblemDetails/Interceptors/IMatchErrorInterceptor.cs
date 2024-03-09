namespace RoyalCode.OperationResults.Interceptors;

/// <summary>
/// A interceptor for the <see cref="ResultErrors"/>.
/// </summary>
public interface IMatchErrorInterceptor
{
    /// <summary>
    /// Interceptor for the <see cref="ResultErrors"/> when is being executed as a result.
    /// </summary>
    /// <param name="errors">The <see cref="ResultErrors"/> to be executed.</param>
    void WritingResultErrors(ResultErrors errors);
}
