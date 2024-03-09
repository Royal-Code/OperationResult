using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// A class that represents the details of an error.
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Creates a new instance of <see cref="ErrorDetails"/> class.
    /// </summary>
    /// <param name="detail">The detail of the error.</param>
    public static implicit operator ErrorDetails(string detail) => new(detail);

    /// <summary>
    /// Compute the <see cref="ErrorDetails"/> from the <see cref="IResultMessage"/>.
    /// </summary>
    /// <param name="message">The <see cref="IResultMessage"/> to compute.</param>
    /// <returns>The <see cref="ErrorDetails"/> computed.</returns>
    public static ErrorDetails From(IResultMessage message)
    {
        var errorDetails = new ErrorDetails(message.Text)
        {
            Extensions = message.AdditionalInformation
        };
        
        if (message.Property is not null)
        {
            errorDetails.Extensions ??= new Dictionary<string, object>();
            errorDetails.Extensions.Add("property", message.Property);
        }

        return errorDetails;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorDetails"/> class.
    /// </summary>
    /// <param name="detail">The detail of the error.</param>
    public ErrorDetails(string detail)
    {
        Detail = detail;
    }

    /// <summary>
    /// Describes the issue in detail.
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    /// Additional information about the invalid parameter.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object>? Extensions { get; set; }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ErrorDetails details &&
               Detail == details.Detail &&
               EqualityComparer<IDictionary<string, object>?>.Default.Equals(Extensions, details.Extensions);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Detail, Extensions);
    }
}