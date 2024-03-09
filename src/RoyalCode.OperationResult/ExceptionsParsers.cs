
using System.Diagnostics.CodeAnalysis;

namespace RoyalCode.OperationResults;

/// <summary>
/// <para>
///     Static class to manage custom exceptions parsers.
/// </para>
/// <para>
///     The parsers added here will be used by the <see cref="ResultMessage"/> class, 
///     when creating a message from an exception.
/// </para>
/// </summary>
public static class ExceptionsParsers
{
    private static List<IExceptionParser>? parsers;

    /// <summary>
    /// <para>
    ///     Determines if a generic message or the exception message 
    ///     will be used as the <see cref="ResultMessage.Text"/>
    ///     when creating a message from an exception.
    /// </para>
    /// </summary>
    public static bool UseGenericMessageForExceptions { get; set; } = true;

    /// <summary>
    /// <para>
    ///     Add a parser to the list of parsers.
    /// </para>
    /// </summary>
    /// <param name="parser">A exception parser.</param>
    public static void AddParser(IExceptionParser parser)
    {
        parsers ??= new List<IExceptionParser>();
        parsers.Add(parser);
    }

    /// <summary>
    /// <para>
    ///     Remove a parser from the list of parsers.
    /// </para>
    /// </summary>
    /// <param name="parser"></param>
    public static void RemoveParser(IExceptionParser parser)
    {
        parsers?.Remove(parser);
    }

    /// <summary>
    /// <para>
    ///     Try to parse the exception to a result message.
    /// </para>
    /// </summary>
    /// <param name="ex">The exception occured into a operation message.</param>
    /// <param name="message">The output message.</param>
    /// <returns>True if the exception was parsed.</returns>
    public static bool TryParse(Exception ex, [NotNullWhen(true)] out ResultMessage? message)
    {
        if (parsers is not null)
            foreach (var parser in parsers)
                if (parser.TryParse(ex, out message))
                    return true;

        message = null;
        return false;
    }

    /// <summary>
    /// <para>
    ///     Try to create a exception from a result message.
    /// </para>
    /// </summary>
    /// <param name="message">The result message.</param>
    /// <param name="exception">The output exception.</param>
    /// <returns>True if the exception was created.</returns>
    public static bool TryCreate(IResultMessage message, [NotNullWhen(true)] out Exception? exception)
    {
        if (parsers is not null)
            foreach (var parser in parsers)
                if (parser.TryCreate(message, out exception))
                    return true;

        exception = null;
        return false;
    }
    
    internal static string GetExceptionMessage(Exception ex)
    {
        if (UseGenericMessageForExceptions)
            return R.GenericExceptionMessage;

        return ex.Message;
    }
}
