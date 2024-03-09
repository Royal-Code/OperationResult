using RoyalCode.OperationResults.Convertion.Internals;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// <para>
///     A Problem Details with extensions. See <see cref="ProblemDetails"/> for more information.
/// </para>
/// <para>
///     Additional properties are added to this class to support the extensions, and they are:
/// </para>
/// <list type="bullet">
///     <item>
///         <term><see cref="InvalidParameters"/></term> that contains the invalid parameters details, 
///         like the parameter name, the error message (reason) and additional information.
///     </item>
///     <item>
///         <term><see cref="NotFoundDetails"/></term> that contains the not found details,
///         like the property name and the error message and additional information.
///     </item>
///     <item>
///         <term><see cref="Errors"/></term> that contains messages for generic errors or application errors.
///     </item>
///     <item>
///         <term><see cref="InnerProblemDetails"/></term> that contains inner <see cref="ProblemDetails"/>
///         for aggregate exceptions.
///     </item>
/// </list>
/// </summary>
public class ProblemDetailsExtended : ProblemDetails
{
    /// <summary>
    /// Contains the constants for the extension fields used by the operation results.
    /// </summary>
    public static class Fields 
    {
        /// <summary>
        /// Extension field for the <see cref="ProblemDetails"/> that contains the aggregate details.
        /// </summary>
        public const string AggregateExtensionField = "inner_details";

        /// <summary>
        /// Extension field for the <see cref="ProblemDetails"/> that contains the invalid parameters details.
        /// </summary>
        public const string InvalidParametersExtensionField = "invalid_params";

        /// <summary>
        /// Extension field for the <see cref="ProblemDetails"/> that contains generic errors or application errors.
        /// </summary>
        public const string ErrorsExtensionField = "errors";

        /// <summary>
        /// Extension field for the <see cref="ProblemDetails"/> that contains the not found details.
        /// </summary>
        public const string NotFoundExtensionField = "not_found";
    }

    /// <summary>
    /// Contains the constants for the default titles used by the operation results.
    /// </summary>
    public static class Titles
    {
        /// <summary>
        /// The default title for the generic errors.
        /// </summary>
        public const string GenericErrorTitle = "An error has occurred";

        /// <summary>
        /// The default title for the not found errors.
        /// </summary>
        public const string NotFoundTitle = "Entity not found";

        /// <summary>
        /// The default title for the invalid parameters errors.
        /// </summary>
        public const string InvalidParametersTitle = "The input parameters are invalid";

        /// <summary>
        /// The default title for the validation errors.
        /// </summary>
        public const string ValidationTitle = "Errors have occurred in the validation of the input parameters.";

        /// <summary>
        /// The default title for the application errors.
        /// </summary>
        public const string ApplicationErrorTitle = "An error has occurred";
    }

    /// <summary>
    /// <para>
    ///     A human-readable explanation specific to this occurrence of the problem.
    /// </para>
    /// <para>
    ///     Invalid parameters errors are added to this property.
    /// </para>
    /// </summary>
    [JsonPropertyName(Fields.InvalidParametersExtensionField)]
    public IEnumerable<InvalidParameterDetails>? InvalidParameters { get; set; }

    /// <summary>
    /// <para>
    ///     A human-readable explanation specific to this occurrence of the problem.
    /// </para>
    /// <para>
    ///     Not found errors are added to this property.
    /// </para>
    /// </summary>
    [JsonPropertyName(Fields.NotFoundExtensionField)]
    public IEnumerable<NotFoundDetails>? NotFoundDetails { get; set; }

    /// <summary>
    /// <para>
    ///     A human-readable explanation specific to this occurrence of the problem.
    /// </para>
    /// <para>
    ///     Internal errors are added to this property.
    /// </para>
    /// </summary>
    [JsonPropertyName(Fields.ErrorsExtensionField)]
    public IEnumerable<ErrorDetails>? Errors { get; set; }

    /// <summary>
    /// <para>
    ///     Inner problem details. When a problem details is wrapped by another problem details,
    ///     the inner problem details are added to this property.
    /// </para>
    /// </summary>
    [JsonPropertyName(Fields.AggregateExtensionField)]
    public IEnumerable<ProblemDetails>? InnerProblemDetails { get; set; }

    /// <summary>
    /// <para>
    ///     Convert the <see cref="ProblemDetailsExtended"/> to a <see cref="ResultErrors"/>.
    /// </para>
    /// </summary>
    /// <returns>
    ///     A new instance of <see cref="ResultErrors"/> with the <see cref="ResultMessage"/> created from the <see cref="ProblemDetailsExtended"/>.
    /// </returns>
    public ResultErrors ToResultErrors()
    {
        var errors = new ResultErrors();

        bool ignoreDetails = false;

        if (InvalidParameters is not null)
        {
            // if status is 400 then use invalid parameters, otherwise use validation
            var useValidation = Status != 400;

            foreach (var invalidParameter in InvalidParameters)
            {
                var message = useValidation
                    ? ResultMessage.ValidationError(invalidParameter.Reason, invalidParameter.Name ?? string.Empty)
                    : ResultMessage.InvalidParameter(invalidParameter.Reason, invalidParameter.Name ?? string.Empty);
                if (invalidParameter.Extensions is not null)
                    foreach(var extension in invalidParameter.Extensions)
                        message.WithInformation(extension.Key, ReadJsonValue(extension.Value) ?? string.Empty);

                errors += message;
            }

            if (Title == Titles.InvalidParametersTitle || Title == Titles.ValidationTitle)
            {
                ignoreDetails = true;
            }
        }

        if (NotFoundDetails is not null)
        {
            foreach (var notFoundDetail in NotFoundDetails)
            {
                var message = ResultMessage.NotFound(notFoundDetail.Message, notFoundDetail.Property ?? string.Empty);
                if (notFoundDetail.Extensions is not null)
                    foreach(var extension in notFoundDetail.Extensions)
                        message.WithInformation(extension.Key, ReadJsonValue(extension.Value) ?? string.Empty);

                errors += message;
            }

            if (Title == Titles.NotFoundTitle)
                ignoreDetails = true;
        }

        if (Errors is not null)
        {
            bool isInternalError = Status == 500;

            foreach (var errorDetails in Errors)
            {
                var message = isInternalError
                    ? ResultMessage.ApplicationError(errorDetails.Detail)
                    : ResultMessage.Error(GenericErrorCodes.GenericError, errorDetails.Detail, HttpStatusCode.BadRequest);

                if (errorDetails.Extensions is not null)
                    foreach (var extension in errorDetails.Extensions)
                        message.WithInformation(extension.Key, ReadJsonValue(extension.Value) ?? string.Empty);

                errors += message;
            }

            if (Title == Titles.ApplicationErrorTitle || Title == Titles.GenericErrorTitle)
                ignoreDetails = true;
        }

        if (InnerProblemDetails is not null)
        {
            foreach (var innerProblemDetail in InnerProblemDetails)
            {
                errors += ToResultMessage(innerProblemDetail);
            }

            ignoreDetails = true;
        }

        if (ignoreDetails)
        {
            if (Extensions?.Count > 0)
            {
                var message = (ResultMessage)errors[0];
                foreach (var extension in Extensions)
                    message.WithInformation(extension.Key, ReadJsonValue(extension.Value) ?? string.Empty);
            }
        }
        else
        {
            errors += ToResultMessage(this);
        }

        return errors;
    }

    private static ResultMessage ToResultMessage(ProblemDetails details)
    {
        // creates the result message
        var message = ResultMessage.Error(
            code: details.Type ?? GenericErrorCodes.ApplicationError, 
            text: details.Detail ?? GenericErrorCodes.ApplicationError,
            status: details.Status.HasValue ? (HttpStatusCode)details.Status.Value : null,
            ex: null);
        
        // add the additional information
        if (details.Extensions is not null)
            foreach(var extension in details.Extensions)
                message.WithInformation(extension.Key, ReadJsonValue(extension.Value) ?? string.Empty);

        return message;
    }

    private static object? ReadJsonValue(object? obj)
    {
        if (obj is null)
            return null;

        if (obj is JsonElement jsonElement)
            return ReadJsonElement(jsonElement);

        return obj;
    }

    private static object? ReadJsonElement(JsonElement jsonElement)
    {
        // convert the json element to a object of the correct type
        return jsonElement.ValueKind switch
        {
            JsonValueKind.String => jsonElement.GetString(),
            JsonValueKind.Number => jsonElement.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Array => jsonElement.EnumerateArray().Select(ReadJsonElement).ToArray(),
            JsonValueKind.Object => ReadJsonObject(jsonElement),
            _ => jsonElement.GetRawText(),
        };
    }

    private static object ReadJsonObject(JsonElement jsonElement)
    {
        // convert the json element to a object of the correct type
        var obj = new Dictionary<string, object?>();
        foreach (var property in jsonElement.EnumerateObject())
        {
            obj.Add(property.Name, ReadJsonElement(property.Value));
        }

        return obj;
    }
}
