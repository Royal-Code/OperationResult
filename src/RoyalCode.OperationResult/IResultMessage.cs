using System.Net;
using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults;

/// <summary>
/// <para>
///     Interface for the operation results messages.
/// </para>
/// </summary>
public interface IResultMessage
{
    /// <summary>
    /// The text of the message. The object is to display the message to users.
    /// </summary>
    string Text { get; }

    /// <summary>
    /// Property that originated the message, 
    /// or property that is intended for the message. 
    /// When not linked to any property, it will be null.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
    string? Property { get; }

    /// <summary>
    /// Some kind of code that can identify the type of message, error, validation, rule.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Code { get; }

    /// <summary>
    /// <para>
    ///     Contains the values of status codes defined for HTTP.
    /// </para>
    /// <para>
    ///     This is a optional property.
    /// </para>
    /// <para>
    ///     Used to help convert operation results into web API's responses.
    /// </para>
    /// <para>
    ///     It can be used to return the status code of the response to the client,
    ///     and can be assigned from the status code of the response.
    /// </para>
    /// </summary>
    [JsonIgnore] 
    HttpStatusCode? Status { get; set; }

    /// <summary>
    /// Exception related to the message.
    /// </summary>
    [JsonIgnore] 
    Exception? Exception { get; }

    /// <summary>
    /// Additional information from the message.
    /// Ids of objects related to the message can be used.
    /// </summary>
    [JsonExtensionData]
    IDictionary<string, object>? AdditionalInformation { get; }
}
