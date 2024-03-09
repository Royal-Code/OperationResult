
using System.Diagnostics.CodeAnalysis;

namespace RoyalCode.OperationResults;

/// <summary>
/// <para>
///     A parser of exceptions to result messages.
/// </para>
/// <para>
///     Add a parser to <see cref="ExceptionsParsers"/> to use it.
/// </para>
/// </summary>
public interface IExceptionParser
{
    /// <summary>
    /// <para>
    ///     Try to parse the exception to a result message.
    /// </para>
    /// </summary>
    /// <param name="ex">The exception occured into a operation message.</param>
    /// <param name="message">The output message.</param>
    /// <returns>True if the exception was parsed.</returns>
    bool TryParse(Exception ex, [NotNullWhen(true)] out ResultMessage? message);

    /// <summary>
    /// <para>
    ///     Try to create a exception from a result message.
    /// </para>
    /// </summary>
    /// <param name="message">The message to create the exception.</param>
    /// <param name="ex">The output exception.</param>
    /// <returns>True if the exception was created.</returns>
    bool TryCreate(IResultMessage message, [NotNullWhen(true)] out Exception? ex);
}