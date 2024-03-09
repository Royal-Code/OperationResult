
namespace RoyalCode.OperationResults;

/// <summary>
/// <para>
///     Generic errors code.
/// </para>
/// <para>
///     These are generic error types,
///     where it is not necessary to specify details about the error that occurred and the rules involved.
/// </para>
/// <para>
///     In cases of specific errors, specific and unique codes must be used for each type of error.
/// </para>
/// </summary>
public static class GenericErrorCodes
{
    /// <summary>
    /// <para>
    ///     Generic Error.
    /// </para>
    /// </summary>
    public const string GenericError = "4XX";

    /// <summary>
    /// <para>
    ///     Errors where the parameters entered are invalid.
    /// </para>
    /// <para>
    ///     It should be used on syntax errors, where the service cannot "understand" the input values.
    /// </para>
    /// </summary>
    public const string InvalidParameters = "400";

    /// <summary>
    /// <para>
    ///     Errors where the input parameters are not valid by some application rule or domain.
    /// </para>
    /// <para>
    ///     It should be used on semantic errors,
    ///     where the request is understood but the dataset is not valid by some rule.
    /// </para>
    /// </summary>
    public const string Validation = "422";

    /// <summary>
    /// Errors of some entity/registry not found.
    /// </summary>
    public const string NotFound = "404";

    /// <summary>
    /// Application error, exception, which is not a validation error.
    /// </summary>
    public const string ApplicationError = "500";

    /// <summary>
    /// Verify if the code is a generic error code.
    /// </summary>
    /// <param name="code">The code to be verified.</param>
    /// <returns>True if the code is a generic error code.</returns>
    public static bool Contains(string code)
    {
        return code == GenericError
            || code == InvalidParameters
            || code == Validation
            || code == NotFound
            || code == ApplicationError;
    }

    /// <summary>
    /// Check if the new code has more priority than the current code.
    /// </summary>
    /// <remarks>
    ///     The priority is:
    ///     Minimum: NotFound
    ///     Maximum: ApplicationError
    ///     Validation have more priority than InvalidParameter and GenericError
    ///     GerericError have more priority than NotFound
    ///     Others codes have more priority than Validation, but less than ApplicationError.
    /// </remarks>
    /// <param name="currentCode">The current code.</param>
    /// <param name="newCode">The new code.</param>
    /// <returns>True if the new code has more priority than the current code.</returns>
    public static bool HaveMorePriority(string currentCode, string newCode)
    {
        if (currentCode == newCode)
            return false;

        if (currentCode == ApplicationError || newCode == NotFound)
            return false;

        if (currentCode == Validation && newCode == InvalidParameters)
            return false;

        if (newCode == GenericError && currentCode != NotFound)
            return false;

        return true;
    }
}
