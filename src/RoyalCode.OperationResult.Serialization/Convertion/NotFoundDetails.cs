using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// <para>
///     The details of a not found problem.
/// </para>
/// </summary>
public class NotFoundDetails
{
    /// <summary>
    /// Creates a new instance of <see cref="NotFoundDetails"/> with the specified message.
    /// </summary>
    /// <param name="message">The message of the problem.</param>
    public NotFoundDetails(string message)
    {
        Message = message;
    }

    /// <summary>
    /// A message describing the problem of what is not found.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// The property or parameter name related to the problem.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Property { get; set; }

    /// <summary>
    /// Additional information about the problem.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object>? Extensions { get; set; }
}
