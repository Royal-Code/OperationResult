
using RoyalCode.OperationResults;
using System;
using System.Net;

namespace ResultMessageAndResultErrors;

public class CreatingMessages
{
    public ResultMessage CreateMessage_Simplest()
    {
        // The simplest way to create an error, with just one message.
        var error = ResultMessage.Error("Error message");

        return error;
    }

    public ResultMessage CreateMessage_WithHttpStatusCode()
    {
        // Additionally, we can define the code for an HTTP response.
        var error = ResultMessage.Error("Error message", HttpStatusCode.BadRequest);

        return error;
    }

    public ResultMessage CreateMessage_WithErrorCode()
    {
        // We can define the code for the error.
        // The error code can be used to identify the error in the client.
        var error = ResultMessage.Error("error-code", "Error message");

        return error;
    }

    public ResultMessage CreateMessage_WithErrorCodePlus()
    {
        var exception = new Exception("Exception message");

        // Additionally, we can define the code for an HTTP response and an exception.
        var error = ResultMessage.Error("error-code", "Error message", HttpStatusCode.BadRequest, exception);

        return error;
    }

    public ResultMessage CreateMessage_WithErrorCodeAndProperty()
    {
        // We can define the code for the error and a property.
        // The property informs the client which property of the model the error refers to.
        var error = ResultMessage.Error("error-code", "Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_WithErrorCodeAndPropertyPlus()
    {
        var exception = new Exception("Exception message");

        // Additionally, we can define the code for an HTTP response, an exception and a property.
        var error = ResultMessage.Error("error-code", "Error message", "PropertyName", HttpStatusCode.BadRequest, exception);

        return error;
    }

    public ResultMessage CreateMessage_FromException()
    {
        var exception = new Exception("Exception message");

        // We can create an error from an exception.
        var error = ResultMessage.Error(exception);

        return error;
    }

    public ResultMessage CreateMessage_FromExceptionPlus()
    {
        var exception = new Exception("Exception message");

        // Additionally, we can define more properties, like: error code, property name and HTTP status code.
        var error = ResultMessage.Error(exception, "PropertyName", "error-code", HttpStatusCode.BadRequest);

        return error;
    }

    public ResultMessage CreateMessage_NotFound()
    {
        // We can create a not found error.
        // The will be a "404" string, and the http status code will be 404 "NotFound".
        var error = ResultMessage.NotFound("Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_NotFoundWithCode()
    {
        // Additionally, we can define the code for the error.
        // The error code can be used to identify the error in the client.
        var error = ResultMessage.NotFound("error-code", "Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_InvalidParameter()
    {
        // We can create an invalid parameter error.
        // The will be a "400" string, and the http status code will be 400 "BadRequest".
        var error = ResultMessage.InvalidParameter("Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_ValidationError()
    {
        // We can create a validation error.
        // The will be a "422" string, and the http status code will be 422 "UnprocessableEntity".
        var error = ResultMessage.ValidationError("Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_ValidationErrorPlus()
    {
        var exception = new Exception("Exception message");

        // Additionally, we can define the code for the error and an exception.
        // The error code can be used to identify the error in the client.
        var error = ResultMessage.ValidationError("error-code", "Error message", "PropertyName", exception);

        return error;
    }

    public ResultMessage CreateMessage_Forbbiden()
    {
        // We can create a forbidden error.
        // For errors of this type, the error code must be informed.
        // The http status code will be 403 "Forbidden".
        var error = ResultMessage.Forbidden("error-code", "Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_Conflict()
    {
        // We can create a conflict error.
        // For errors of this type, the error code must be informed.
        // The http status code will be 409 "Conflict".
        var error = ResultMessage.Conflict("error-code", "Error message", "PropertyName");

        return error;
    }

    public ResultMessage CreateMessage_ApplicationError()
    {
        var exception = new Exception("Exception message");

        // We can create an application error from an exception.
        // The will be a "500" string, and the http status code will be 500 "InternalServerError".
        var error = ResultMessage.ApplicationError(exception);

        return error;
    }

    public ResultMessage CreateMessage_ApplicationErrorWithMessage()
    {
        var exception = new Exception("Exception message");

        // Additionally, we can define the message for the error.
        var error = ResultMessage.ApplicationError(exception, "Error Message");

        return error;
    }

    public ResultMessage CreateMessage_ApplicationErrorFromMessage()
    {
        // Finally, we can create an application error from a message.
        // The will be a "500" string, and the http status code will be 500 "InternalServerError".
        var error = ResultMessage.ApplicationError("Error Message");

        return error;
    }
}
