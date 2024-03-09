using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// A class that represents the details of an invalid parameter.
/// </summary>
public class InvalidParameterDetails
{
    /// <summary>
    /// Creates a new instance of <see cref="InvalidParameterDetails"/> class.
    /// </summary>
    /// <param name="reason">The reason for the invalid parameter.</param>
    public InvalidParameterDetails(string reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// The name of the parameter or property.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    /// <summary>
    /// A message that describes the reason for the invalid parameter.
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Additional information about the invalid parameter.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object>? Extensions { get; set; }
}
