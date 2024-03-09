using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults;

/// <summary>
/// A errors of <see cref="IResultMessage"/>s.
/// </summary>
public class ResultErrors : List<IResultMessage>
{
    /// <summary>
    /// Default capacity of the errors.
    /// </summary>
    public static int DefaultCapacity { get; set; } = 1;

    /// <summary>
    /// Adds a new message to the errors and returns the errors.
    /// </summary>
    /// <param name="errors">The errors to add the message to</param>
    /// <param name="message">The new message to add</param>
    /// <returns>The same instance of the errors</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultErrors operator +(ResultErrors errors, IResultMessage message)
    {
        errors.Add(message);
        return errors;
    }

    /// <summary>
    /// Adds a range of messages to the errors and returns the errors.
    /// </summary>
    /// <param name="errors">The errors to add the messages to</param>
    /// <param name="other">The messages to add</param>
    /// <returns>The same instance of the errors</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultErrors operator +(ResultErrors errors, IEnumerable<IResultMessage> other)
    {
        errors.AddRange(other);
        return errors;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ResultErrors"/>
    /// </summary>
    public ResultErrors() : base (DefaultCapacity) { }

    /// <summary>
    /// Creates a new instance of <see cref="ResultErrors"/> with the messages provided.
    /// </summary>
    /// <param name="messages">The errors of messages to add to the new instance.</param>
    [JsonConstructor]
    public ResultErrors(IEnumerable<ResultMessage> messages) : base(messages) { }

    /// <summary>
    /// Adds a new message to the errors and returns the errors.
    /// </summary>
    /// <param name="message">The new message to add</param>
    /// <returns>The same instance of the errors</returns>
    public ResultErrors With(IResultMessage message)
    {
        Add(message);
        return this;
    }

    /// <summary>
    /// Adds a range of messages to the errors and returns the errors.
    /// </summary>
    /// <param name="messages">The messages to add</param>
    /// <returns>The same instance of the errors</returns>
    public ResultErrors With(IEnumerable<IResultMessage> messages)
    {
        AddRange(messages);
        return this;
    }

    /// <summary>
    /// <para>
    ///     Returns a string representation of the errors.
    /// </para>
    /// <para>
    ///     The string will be a semicolon-separated list of the <see cref="IResultMessage"/> of each message in the errors.
    /// </para>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => string.Join("; ", this);
}
