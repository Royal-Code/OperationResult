using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// Converts the <see cref="ResultErrors"/> to <see cref="ProblemDetails"/>.
/// </summary>
public static class ProblemDetailsConverter
{
    /// <summary>
    /// Convert the <paramref name="result"/> to <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="result">The result to be converted.</param>
    /// <param name="options">The options to be used in the conversion.</param>
    /// <returns>A new instance of <see cref="ProblemDetails"/>.</returns>
    public static ProblemDetails ToProblemDetails(
       this ResultErrors result, ProblemDetailsOptions options)
    {
        if (result.Count == 1)
        {
            var message = result[0];
            return message.ToProblemDetails(options);
        }

        var builder = new ProblemDetailsBuilder();
        foreach (var message in result)
        {
            AddMessage(message, builder);
        }

        if (builder.Code is null)
            return null!;

        // get the description
        options.Descriptor.TryGetDescription(builder.Code, out var description);

        var status = description?.Status.HasValue ?? false
            ? (int)description.Status.Value
            : builder.GetStatusCode();

        // create the problem details
        var problemDetails = new ProblemDetails
        {
            Type = description?.Type ?? builder.Code.ToProblemDetailsType(options),
            Title = description?.Title ?? builder.Code,
            Detail = builder.GetDetail(),
            Status = status,
        };

        // add the extensions
        builder.WriteExtensions(problemDetails, options);

        return problemDetails;
    }

    /// <summary>
    /// Convert one message to a problem details.
    /// </summary>
    /// <param name="message">The result message</param>
    /// <param name="options">The options for the conversion.</param>
    /// <returns>A new instance of <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/>.</returns>
    public static ProblemDetails ToProblemDetails(
       this IResultMessage message, ProblemDetailsOptions options)
    {
        var code = message.GetCodeForType();

        // get the description
        options.Descriptor.TryGetDescription(code, out var description);

        int? status = description?.Status.HasValue ?? false
            ? (int)description.Status.Value
            : message.Status.HasValue
                ? (int)message.Status
                : 400;

        ProblemDetails problem = new()
        {
            Type = description?.Type ?? code.ToProblemDetailsType(options),
            Title = description?.Title ?? code,
            Detail = message.Text,
            Status = status
        };

        if (message.AdditionalInformation is not null)
            foreach (var (key, value) in message.AdditionalInformation)
                problem.Extensions.Add(key, value);

        if (message.Property is not null)
            problem.Extensions.Add("property", message.Property);

        return problem;
    }

    private static void AddMessage(IResultMessage message, ProblemDetailsBuilder builder)
    {
        bool isGenericError = message.Code is null || GenericErrorCodes.Contains(message.Code);
        var code = message.GetCodeForType();

        builder.SetCode(code, isGenericError);

        if (isGenericError)
        {
            // if the code is of invalid parameters, or validation, then add the details
            if (code == GenericErrorCodes.InvalidParameters
                || code == GenericErrorCodes.Validation)
            {
                builder.AddInvalidParameter(new InvalidParameterDetails(message.Text)
                {
                    Name = message.Property,
                    Extensions = message.AdditionalInformation
                }, code == GenericErrorCodes.Validation);
            }
            // if the code is not found
            else if (code == GenericErrorCodes.NotFound)
            {
                builder.AddNotFound(new NotFoundDetails(message.Text)
                {
                    Property = message.Property,
                    Extensions = message.AdditionalInformation
                });
            }
            // if not, add error, it can be a generic error or a application error (internal error)
            else
            {
                builder.AddErrorMessage(ErrorDetails.From(message));
            }
        }
        else
        {
            builder.AddCustomProblem(message);
        }
    }

    /// <summary>
    /// Convert a <see cref="IResultMessage.Code"/> 
    /// to the value for <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails.Type"/>.
    /// </summary>
    /// <param name="code">The code of the message.</param>
    /// <param name="options">The options for the conversion.</param>
    /// <returns>A string with the value for <see cref="ProblemDetails.Type"/>.</returns>
    private static string ToProblemDetailsType(this string code, ProblemDetailsOptions options)
    {
        if (string.IsNullOrEmpty(code))
            return options.BaseAddress;

        return $"{options.BaseAddress}{options.TypeComplement}{code}";
    }

    /// <summary>
    /// Get the type for the problem details from the message code or get the default code.
    /// </summary>
    /// <param name="message">The result message.</param>
    /// <returns>The type for the problem details.</returns>
    private static string GetCodeForType(this IResultMessage message)
    {
        return message.Code ?? (message.Status is not null
            ? ((int)message.Status.Value).ToString()
            : message.Property is null
                ? GenericErrorCodes.GenericError
                : GenericErrorCodes.InvalidParameters);
    }
}
