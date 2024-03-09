using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// Builder for <see cref="ProblemDetails"/> used by <see cref="ProblemDetailsConverter"/>.
/// </summary>
public class ProblemDetailsBuilder
{
    private List<InvalidParameterDetails>? invalidParameterErrors;
    private List<NotFoundDetails>? notFoundErrors;
    private List<ErrorDetails>? errors;
    private List<IResultMessage>? customProblems;
    private Dictionary<string, object>? extensions;
    private bool withRulesValidationErrors = false;

    /// <summary>
    /// The Code used to identify the problem and generate the <see cref="ProblemDetails.Type"/>.
    /// </summary>
    public string? Code { get; private set; }

    /// <summary>
    /// Determines if the problem is an aggregate of other problems.
    /// </summary>
    public bool Aggregate { get; private set; }

    /// <summary>
    /// Determines if the problem is a generic error (<see cref="GenericErrorCodes"/>).
    /// </summary>
    public bool GenericError { get; private set; }

    /// <summary>
    /// Gets the Http status code for the problem.
    /// </summary>
    /// <returns>An integer representing the Http status code.</returns>
    public int GetStatusCode()
    {
        if (customProblems is not null)
        {
            if (customProblems.Count == 1 && customProblems[0].Status.HasValue)
                return (int)customProblems[0].Status!.Value;

            int status = StatusCodes.Status400BadRequest;
            foreach (var message in customProblems.Where(static m => m.Status.HasValue))
            {
                var messageStatus = (int)message.Status!.Value;
                if (messageStatus > status)
                    status = messageStatus;
            }
            return status;
        }

        if (errors is not null)
            return StatusCodes.Status500InternalServerError;

        if (invalidParameterErrors is not null)
            return withRulesValidationErrors 
                ? StatusCodes.Status422UnprocessableEntity 
                : StatusCodes.Status400BadRequest;

        if (notFoundErrors is not null)
            return StatusCodes.Status404NotFound;

        return StatusCodes.Status400BadRequest;
    }

    /// <summary>
    /// Gets the value for the field <see cref="ProblemDetails.Detail"/>.
    /// </summary>
    /// <returns>A string that describes the error</returns>
    public string GetDetail()
    {
        if (Aggregate)
            return ProblemDetailsDescriptor.Messages.AggregateMessage;

        if (customProblems is not null)
            return customProblems[0].Text ?? string.Empty;

        if (errors is not null)
            return ProblemDetailsDescriptor.Messages.InternalErrorsMessage;

        if (invalidParameterErrors is not null)
            return ProblemDetailsDescriptor.Messages.InvalidParametersMessage;

        if (notFoundErrors is not null)
            return ProblemDetailsDescriptor.Messages.NotFoundMessage;

        return ProblemDetailsDescriptor.Messages.DefaultMessage;
    }

    /// <summary>
    /// Add the extensions fields and values to the <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="problemDetails">The problem details to be modified.</param>
    /// <param name="options">The options for the problem details conversion.</param>
    public void WriteExtensions(ProblemDetails problemDetails,
        ProblemDetailsOptions options)
    {
        var pdext = problemDetails.Extensions;

        if (customProblems is not null)
        {
            if (Aggregate)
            {
                pdext[ProblemDetailsExtended.Fields.AggregateExtensionField] = customProblems
                    .Select(m => m.ToProblemDetails(options))
                    .ToList();
            }
            else if (customProblems[0].AdditionalInformation is not null)
            {
                foreach (var (key, value) in customProblems[0].AdditionalInformation!)
                    pdext[key] = value;
            }
        }
        if (invalidParameterErrors is not null)
            pdext[ProblemDetailsExtended.Fields.InvalidParametersExtensionField] = invalidParameterErrors;

        if (notFoundErrors is not null)
            pdext[ProblemDetailsExtended.Fields.NotFoundExtensionField] = notFoundErrors;

        if (errors is not null)
            pdext[ProblemDetailsExtended.Fields.ErrorsExtensionField] = errors;

        if (extensions is not null)
            foreach (var (key, value) in extensions)
                pdext[key] = value;
    }

    /// <summary>
    /// Try to set the problem details code.
    /// </summary>
    /// <param name="code">The message code.</param>
    /// <param name="isGenericError">If the code is an generic error code.</param>
    public void SetCode(string code, bool isGenericError)
    {
        if (Aggregate is true)
            return;

        if (Code is null)
        {
            Code = code;
            GenericError = isGenericError;
            return;
        }

        if (isGenericError is false)
        {
            if (GenericError is false)
            {
                Aggregate = true;
                Code = ProblemDetailsDescriptor.Codes.AggregateProblemsDetails;
            }
            else
            {
                Code = code;
                GenericError = false;
            }
        }
        else if (GenericError is true && GenericErrorCodes.HaveMorePriority(Code, code))
        {
            Code = code;
        }
    }

    /// <summary>
    /// Add a invalid parameter error to the problem details.
    /// </summary>
    /// <param name="details">The invalid parameter details.</param>
    /// <param name="validationError">If the error is a validation error.</param>
    public void AddInvalidParameter(InvalidParameterDetails details, bool validationError)
    {
        invalidParameterErrors ??= [];
        invalidParameterErrors.Add(details);
        if (validationError)
            withRulesValidationErrors = true;
    }

    /// <summary>
    /// Add a not found error to the problem details.
    /// </summary>
    /// <param name="notFoundDetails">The not found details.</param>
    public void AddNotFound(NotFoundDetails notFoundDetails)
    {
        notFoundErrors ??= [];
        notFoundErrors.Add(notFoundDetails);
    }

    /// <summary>
    /// Add a internal error to the problem details.
    /// </summary>
    /// <param name="errorDetails">The erro details.</param>
    public void AddErrorMessage(ErrorDetails errorDetails)
    {
        errors ??= [];
        errors.Add(errorDetails);
    }

    /// <summary>
    /// <para>
    ///     Add a custom problem to the problem details.
    /// </para>
    /// <para>
    ///     A custom problem is a message with a specific Code, and will have a specific Type.
    /// </para>
    /// </summary>
    /// <param name="message">The custom problem message.</param>
    public void AddCustomProblem(IResultMessage message)
    {
        customProblems ??= [];
        customProblems.Add(message);
    }

    /// <summary>
    /// Additional information to be added to the problem details.
    /// </summary>
    /// <param name="additionalInformation">Additional information, in the form of key-value pairs.</param>
    public void AddExtension(IEnumerable<KeyValuePair<string, object>> additionalInformation)
    {
        extensions ??= [];

        foreach (var kvp in additionalInformation)
        {
            extensions[kvp.Key] = kvp.Value;
        }
    }
}
