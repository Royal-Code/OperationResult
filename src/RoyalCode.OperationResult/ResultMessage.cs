using System.Collections.Immutable;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults;

#pragma warning disable S3427 // Method overloads with default parameter values should not overlap

/// <summary>
/// Component for the message of an operation result.
/// </summary>
public class ResultMessage : IResultMessage
{
    /// <summary>
    /// Implicitly convert an error to a failure operation result.
    /// </summary>
    /// <param name="error"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ResultErrors(ResultMessage error) => new ResultErrors().With(error);

    /// <summary>
    /// <para>
    ///     Creates a new message of error.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Error(string code, string text)
    {
        return new ResultMessage(text, null, code, null, null);
    }

    /// <summary>
    /// <para>
    ///     Creates a new message of error.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="status">The HTTP status code, optional.</param>
    /// <param name="ex">The exception, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Error(string code, string text, HttpStatusCode? status = null, Exception? ex = null)
    {
        return new ResultMessage(text, null, code, status, ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a new message of error.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related, optional.</param>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="status">The HTTP status code, optional.</param>
    /// <param name="ex">The exception, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Error(string? code, string text,
        string? property = null, HttpStatusCode? status = null, Exception? ex = null)
    {
        return new ResultMessage(text, property, code, status, ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a new message of error.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="status">The HTTP status code, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Error(string text, HttpStatusCode? status = null)
    {
        return new ResultMessage(text, null, null, status, null);
    }

    /// <summary>
    /// <para>
    ///     Creates a new message of error.
    /// </para>
    /// </summary>
    /// <param name="ex">The exception that generate the message.</param>
    /// <param name="property">The related property, optional.</param>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="status">The HTTP status code, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Error(Exception ex, string? property = null, string? code = null, HttpStatusCode? status = null)
    {
        if (ex is null)
            throw new ArgumentNullException(nameof(ex));

        if (ExceptionsParsers.TryParse(ex, out var message))
        {
            if (property is not null)
                message.Property = property;
            if (code is not null)
                message.Code = code;
            if (status is not null)
                message.Status = status;

            message.Status ??= HttpStatusCode.InternalServerError;

            return message;
        }

        var text = ExceptionsParsers.GetExceptionMessage(ex);

        return new ResultMessage(text, property, code, status ?? HttpStatusCode.InternalServerError, ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the code from <see cref="GenericErrorCodes.NotFound"/>
    ///     and HTTP status NotFound 404.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage NotFound(string text, string? property)
    {
        return new ResultMessage(text, property, GenericErrorCodes.NotFound, HttpStatusCode.NotFound);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with a specified code
    ///     and HTTP status NotFound 404.
    /// </para>
    /// </summary>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage NotFound(string code, string text, string? property)
    {
        return new ResultMessage(text, property, code, HttpStatusCode.NotFound);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with a specified code and HTTP status Forbidden 403.
    /// </para>
    /// </summary>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Forbidden(string code, string text, string? property = null)
    {
        return new ResultMessage(text, property, code, HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with a specified code and HTTP status Conflict 409.
    /// </para>
    /// </summary>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage Conflict(string code, string text, string? property = null)
    {
        return new ResultMessage(text, property, code, HttpStatusCode.Conflict);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the code from <see cref="GenericErrorCodes.InvalidParameters"/> 
    ///     and HTTP status BadRequest 400.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage InvalidParameter(string text, string property)
    {
        return new ResultMessage(text, property, GenericErrorCodes.InvalidParameters, HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the code from <see cref="GenericErrorCodes.Validation"/> 
    ///     and HTTP status UnprocessableEntity 422.
    /// </para>
    /// </summary>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related.</param>
    /// <param name="ex">The exception, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage ValidationError(string text, string property, Exception? ex = null)
    {
        return new ResultMessage(text, property, GenericErrorCodes.Validation, HttpStatusCode.UnprocessableEntity, ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the specified code and HTTP status UnprocessableEntity 422.
    /// </para>
    /// </summary>
    /// <param name="code">Some kind of code that can identify the type of message or error.</param>
    /// <param name="text">The message text.</param>
    /// <param name="property">The property related.</param>
    /// <param name="ex">The exception, optional.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage ValidationError(string code, string text, string? property, Exception? ex = null)
    {
        return new ResultMessage(text, property, code, HttpStatusCode.UnprocessableEntity, ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the code from <see cref="GenericErrorCodes.Validation"/> 
    ///     and HTTP status UnprocessableEntity 422.
    /// </para>
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    /// </para>
    /// </returns>
    public static ResultMessage ValidationError(Exception ex)
    {
        if (ex is null)
            throw new ArgumentNullException(nameof(ex));

        if (ExceptionsParsers.TryParse(ex, out var message))
            return message;

        string? property = null;
        if (ex is ArgumentException aex)
            property = aex.ParamName;

        return new ResultMessage(ex.Message,
            property,
            GenericErrorCodes.Validation,
            HttpStatusCode.UnprocessableEntity,
            ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the code from <see cref="GenericErrorCodes.ApplicationError"/>
    ///     and HTTP status InternalServerError 500.
    /// </para>
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <param name="text">The message text, optional, when not informed will be the exception message.</param>
    /// <returns>
    /// <para>
    ///     New instance of message.
    ///</para>
    /// </returns>
    public static ResultMessage ApplicationError(Exception ex, string? text = null)
    {
        if (ex is null)
            throw new ArgumentNullException(nameof(ex));

        if (ExceptionsParsers.TryParse(ex, out var message))
        {
            if (text is not null)
                message.Text = text;

            message.Status ??= HttpStatusCode.InternalServerError;

            return message;
        }

        string? property = null;
        if (ex is ArgumentException aex)
            property = aex.ParamName;
        
        text ??= ExceptionsParsers.GetExceptionMessage(ex);

        return new ResultMessage(text, 
            property,
            GenericErrorCodes.ApplicationError,
            HttpStatusCode.InternalServerError,
            ex);
    }

    /// <summary>
    /// <para>
    ///     Creates a error message with the code from <see cref="GenericErrorCodes.ApplicationError"/>
    ///     and HTTP status InternalServerError 500.
    /// </para>
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <returns>
    ///     New instance of message.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <paramref name="message"/> is null or whitespace.
    /// </exception>
    public static ResultMessage ApplicationError(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));

        return new ResultMessage(message, null, GenericErrorCodes.ApplicationError, HttpStatusCode.InternalServerError);
    }

    private LinkedList<KeyValuePair<string, object>>? additionalInformation;

    /// <summary>
    /// <para>
    ///     Creates a new result message.
    /// </para>
    /// <para>
    ///     Used for deserialization only.
    /// </para>
    /// </summary>
    /// <param name="text">The text of the message. The object is to display the message to users.</param>
    /// <param name="property">Property that originated the message.</param>
    /// <param name="code">Some kind of code that can identify the type of message or the message itself.</param>
    [JsonConstructor]
    public ResultMessage(
        string text,
        string? property = null,
        string? code = null)
    {
        Text = text;
        Property = property;
        Code = code;
        if (code is not null)
            Status = code switch
            {
                GenericErrorCodes.NotFound => HttpStatusCode.NotFound,
                GenericErrorCodes.InvalidParameters => HttpStatusCode.BadRequest,
                GenericErrorCodes.Validation => HttpStatusCode.UnprocessableEntity,
                GenericErrorCodes.ApplicationError => HttpStatusCode.InternalServerError,
                _ => null
            };
    }

    internal ResultMessage(
        string text,
        string? property,
        string? code,
        HttpStatusCode? status,
        Exception? exception = null)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Property = property;
        Code = code;
        Status = status;
        Exception = exception;
    }

    /// <inheritdoc/>
    public string Text { get; internal set; }

    /// <inheritdoc/>
    public string? Property { get; internal set; }

    /// <inheritdoc/>
    public string? Code { get; internal set; }

    /// <inheritdoc/>
    public Exception? Exception { get; internal set; }

    /// <inheritdoc/>
    public HttpStatusCode? Status { get; set; }

    /// <inheritdoc/>
    public IDictionary<string, object>? AdditionalInformation
        => additionalInformation?.ToImmutableDictionary();

    /// <summary>
    /// Returns the text.
    /// </summary>
    public override string ToString() => Text;

    /// <summary>
    /// <para>
    ///     Adds extra information to the message.
    /// </para>
    /// <para>
    ///     This method adds data to the <see cref="AdditionalInformation"/> property.
    /// </para>
    /// <para>
    ///     If the key already exists, the value will be overwritten.
    /// </para>
    /// </summary>
    /// <param name="key">Additional information key.</param>
    /// <param name="value">Additional information value.</param>
    /// <returns>The same instance of the message for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    ///     if the <paramref name="key"/> is null, empty or contains only white spaces.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     if the <paramref name="value"/> is null.
    /// </exception>
    public ResultMessage WithInformation(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));

        if (value is null)
            throw new ArgumentNullException(nameof(value));

        additionalInformation ??= new();

        if (key.Equals("property", StringComparison.OrdinalIgnoreCase))
            Property = value.ToString();

        var kvp = additionalInformation.FirstOrDefault(i => i.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        if (kvp.Key is not null)
            additionalInformation.Remove(kvp);

        additionalInformation.AddLast(new KeyValuePair<string, object>(key, value));

        return this;
    }
}