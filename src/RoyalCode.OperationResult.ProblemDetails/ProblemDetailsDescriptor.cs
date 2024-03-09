using RoyalCode.OperationResults.Convertion;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace RoyalCode.OperationResults;

/// <summary>
/// A class that contains all the information about a problem details.
/// </summary>
public class ProblemDetailsDescriptor
{
    /// <summary>
    /// Messages for well-known problem details.
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// The Detail field for the an aggregation of problems.
        /// </summary>
        public static string AggregateMessage { get; set; } = "Multiples problems";

        /// <summary>
        /// The Detail field for the an internal error.
        /// </summary>
        public static string InternalErrorsMessage { get; set; } = "Internal error";

        /// <summary>
        /// The Detail field for the an invalid parameters.
        /// </summary>
        public static string InvalidParametersMessage { get; set; } = "Invalid parameters";

        /// <summary>
        /// The Detail field for the an entity not found.
        /// </summary>
        public static string NotFoundMessage { get; set; } = "Entity not found";

        /// <summary>
        /// The Detail field for the an undefined error.
        /// </summary>
        public static string DefaultMessage { get; set; } = "An error has occurred";
    }

    /// <summary>
    /// Well-known codes for the problem details.
    /// </summary>
    public static class Codes
    {
        /// <summary>
        /// The key for the an aggregation of problems.
        /// </summary>
        public const string AggregateProblemsDetails = "aggregate-problems-details";
    }

    /// <summary>
    /// Titles for well-known problem details.
    /// </summary>
    public static class Titles
    {
        /// <summary>
        /// Default title for the problem details of an aggregation of problems.
        /// </summary>
        public const string AggregateProblemsDetailsTitle = "Multiples problems";

        /// <summary>
        /// Default title for the problem details of type "about:blank".
        /// </summary>
        public const string AboutBlankTitle = "See HTTP Status Code";
    }

    /// <summary>
    /// Constants for the defaults values of the problem details.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// The "about:blank" URI, when used as a problem type, 
        /// indicates that the problem has no additional semantics beyond that of the HTTP status code.
        /// </summary>
        public const string AboutBlank = "about:blank";

        /// <summary>
        /// Default type for the problem details of status code 400, Bad Request.
        /// </summary>
        public const string GenericErrorType = "https://www.rfc-editor.org/rfc/rfc9110.html#name-400-bad-request";

        /// <summary>
        /// Default type for the problem details of status code 404, Not Found.
        /// </summary>
        public const string NotFoundType = "https://www.rfc-editor.org/rfc/rfc9110.html#name-404-not-found";

        /// <summary>
        /// Default type for the problem details of status code 400, Bad Request.
        /// </summary>
        public const string InvalidParametersType = "https://www.rfc-editor.org/rfc/rfc9110.html#name-400-bad-request";

        /// <summary>
        /// Default type for the problem details of status code 422, Unprocessable Content.
        /// </summary>
        public const string ValidationType = "https://www.rfc-editor.org/rfc/rfc9110.html#name-422-unprocessable-content";

        /// <summary>
        /// Default type for the problem details of status code 500, Internal Server Error.
        /// </summary>
        public const string ApplicationErrorType = "https://www.rfc-editor.org/rfc/rfc9110.html#name-500-internal-server-error";
    }

    /// <summary>
    /// The factory to create the default descriptions of the problem details for the generic errors.
    /// </summary>
    public static Func<Dictionary<string, ProblemDetailsDescription>> DescriptionFactory { get; set; } = () => new()
    {
        {
            GenericErrorCodes.NotFound,
            new ProblemDetailsDescription(GenericErrorCodes.NotFound,
                Types.NotFoundType,
                ProblemDetailsExtended.Titles.NotFoundTitle,
                """
                The 404 (Not Found) status code indicates that the origin server did not find a current representation
                for the target resource or is not willing to disclose that one exists. 
                """,
                HttpStatusCode.NotFound)
        },
        {
            GenericErrorCodes.InvalidParameters,
            new ProblemDetailsDescription(GenericErrorCodes.InvalidParameters,
                Types.InvalidParametersType,
                ProblemDetailsExtended.Titles.InvalidParametersTitle,
                """
                The 400 (Bad Request) status code indicates that the server cannot or will not process the request
                due to something that is perceived to be a client error.
                This particular error occurs because the parameters sent in by the client are invalid.
                """,
                HttpStatusCode.BadRequest)
        },
        {
            GenericErrorCodes.GenericError,
            new ProblemDetailsDescription(GenericErrorCodes.GenericError,
                Types.GenericErrorType,
                ProblemDetailsExtended.Titles.GenericErrorTitle,
                """
                The 400 (Bad Request) status code indicates that the server cannot or will not process the request
                due to something that is perceived to be a client error.
                """,
                HttpStatusCode.BadRequest)
        },
        {
            GenericErrorCodes.Validation,
            new ProblemDetailsDescription(GenericErrorCodes.Validation,
                Types.ValidationType,
                "Errors have occurred in the validation of the input parameters.",
                """
                The 422 (Unprocessable Content) status code indicates that the server understands the content type 
                of the request content, and the syntax of the request content is correct, 
                but it was unable to process the contained instructions.
                This occurs because of validations on the input parameters,
                where application or business rules are violated.
                """,
                HttpStatusCode.UnprocessableEntity)
        },
        {
            GenericErrorCodes.ApplicationError,
            new ProblemDetailsDescription(GenericErrorCodes.ApplicationError,
                Types.ApplicationErrorType,
                ProblemDetailsExtended.Titles.ApplicationErrorTitle,
                """
                The 500 (Internal Server Error) status code indicates that the server 
                encountered an unexpected condition that prevented it from fulfilling the request.
                """,
                HttpStatusCode.InternalServerError)
        },
        {
            Codes.AggregateProblemsDetails,
            new ProblemDetailsDescription(Codes.AggregateProblemsDetails,
                Titles.AggregateProblemsDetailsTitle,
                """
                This type of problem describes that there were several problems, and they are of different types. 
                An additional property, called 'inner-problems' will contain the various problems.
                """)
        }
    };

    private readonly Dictionary<string, ProblemDetailsDescription> descriptions = DescriptionFactory();

    /// <summary>
    /// <para>
    ///     Try to get the descriptionsToAdd of a problem details by its code.
    /// </para>
    /// </summary>
    /// <param name="code">The code of the problem details.</param>
    /// <param name="description">The descriptionsToAdd of the problem details.</param>
    /// <returns>True if the descriptionsToAdd was found, otherwise false.</returns>
    public bool TryGetDescription(string code, [NotNullWhen(true)] out ProblemDetailsDescription? description)
    {
        return descriptions.TryGetValue(code, out description);
    }

    /// <summary>
    /// <para>
    ///     Adds a new problem details description.
    /// </para>
    /// </summary>
    /// <param name="description">The description of the problem details.</param>
    /// <returns>Same instance of <see cref="ProblemDetailsDescriptor"/>.</returns>
    public ProblemDetailsDescriptor Add(ProblemDetailsDescription description)
    {
        descriptions.Add(description.Code, description);
        return this;
    }

    /// <summary>
    /// <para>
    ///     Adds many problem details descriptionsToAdd.
    /// </para>
    /// </summary>
    /// <param name="descriptions">A collection of descriptionsToAdd of problem details.</param>
    /// <returns>Same instance of <see cref="ProblemDetailsDescriptor"/>.</returns>
    public ProblemDetailsDescriptor AddRange(IEnumerable<ProblemDetailsDescription> descriptions)
    {
        foreach (var description in descriptions)
        {
            this.descriptions.Add(description.Code, description);
        }

        return this;
    }

    /// <summary>
    /// <para>
    ///     Adds many problem details descriptionsToAdd from a JSON string.
    /// </para>
    /// </summary>
    /// <param name="json">A JSON string with the descriptionsToAdd of problem details.</param>
    /// <returns>Same instance of <see cref="ProblemDetailsDescriptor"/>.</returns>
    /// <exception cref="ProblemDetailsDescriptorDeserializationException">
    ///     If the JSON string is invalid.
    /// </exception>
    public ProblemDetailsDescriptor AddFromJson(string json)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            var descriptionsToAdd = JsonSerializer.Deserialize<IEnumerable<ProblemDetailsDescription>>(json, jsonOptions);
            if (descriptionsToAdd is not null)
            {
                AddRange(descriptionsToAdd);
            }
            return this;
        }
        catch(Exception ex)
        {
            throw new ProblemDetailsDescriptorDeserializationException(json, ex);
        }
    }

    /// <summary>
    /// <para>
    ///     Adds many problem details descriptionsToAdd from a JSON file.
    /// </para>
    /// </summary>
    /// <param name="path">The path of the JSON file with the descriptionsToAdd of problem details.</param>
    /// <returns>Same instance of <see cref="ProblemDetailsDescriptor"/>.</returns>
    /// <exception cref="ProblemDetailsDescriptorDeserializationException">
    ///     If the JSON file is invalid.
    /// </exception>
    public ProblemDetailsDescriptor AddFromJsonFile(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            return AddFromJson(json);
        }
        catch (ProblemDetailsDescriptorDeserializationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ProblemDetailsDescriptorDeserializationException(string.Empty, ex);
        }
    }

    internal void DescribeGenericErrorsWithAboutBlank()
    {
        var genericErrorsDescriptions = descriptions
            .Where(kv => GenericErrorCodes.Contains(kv.Key))
            .Select(kv => kv.Value);

        foreach (var description in genericErrorsDescriptions)
        {
            description.Type = Types.AboutBlank;
            description.Title = Titles.AboutBlankTitle;
        }
    }
}
