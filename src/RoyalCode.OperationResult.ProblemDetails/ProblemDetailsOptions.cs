using Microsoft.Extensions.Logging;

namespace RoyalCode.OperationResults;

/// <summary>
/// Options for convert Operation Results to Problem Details.
/// </summary>
public class ProblemDetailsOptions
{
    private bool completed;

    /// <summary>
    /// Default value for the base address to describe the problem details, and use for build the type from the code.
    /// </summary>
    public const string DefaultBaseAddress = "tag:problemdetails/.problems";

    /// <summary>
    /// The base address to describe the problem details, and use for build the type from the code.
    /// </summary>
    public string BaseAddress { get; set; } = DefaultBaseAddress;

    /// <summary>
    /// Additional part to add in the type of the problem details.
    /// It stay between the base address and the code.
    /// </summary>
    public string TypeComplement { get; set; } = "#";

    /// <summary>
    /// The descriptor of the problem details.
    /// </summary>
    public ProblemDetailsDescriptor Descriptor { get; } = new();

    /// <summary>
    /// <para>
    ///     A list of json files with the problem details.
    /// </para>
    /// <para>
    ///     Post configure the options (this), the files will be loaded and the problem details will be added.
    /// </para>
    /// </summary>
    public string[]? DescriptionFiles { get; set; }

    /// <summary>
    /// Determine how the generic error codes will be described. Used for generate the problem details.
    /// </summary>
    public GenericErrorsDescriptions HowToDescribeGenericErrors { get; set; }

    /// <summary>
    /// Complete the options configuration, adding the problem details from the files.
    /// </summary>
    internal void Complete(ILogger logger)
    {
        if (completed)
        { 
            // log waring of options already completed
            logger.LogWarning("The options configuration is already completed.");

            return;
        }

        completed = true;

        if (HowToDescribeGenericErrors == GenericErrorsDescriptions.AboutBlank)
            Descriptor.DescribeGenericErrorsWithAboutBlank();

        if (DescriptionFiles is null)
            return;

        foreach (var file in DescriptionFiles)
        {
            // try load the file
            try
            {
                Descriptor.AddFromJsonFile(file);
                // log info of success loaded file
                logger.LogInformation("Loaded problem details from file '{file}'.", file);
            }
            catch (Exception ex)
            {
                // log the error
                logger.LogError(ex, "Error loading problem details from file '{file}'.", file);
            }
        }
    }
}

/// <summary>
/// Define how the generic error codes will be described. Used for generate the problem details.
/// </summary>
public enum GenericErrorsDescriptions
{
    /// <summary>
    /// <para>
    ///     Describe the generic erros with the RFC 9457.
    /// </para>
    /// <para>
    ///     The problem details types will be links to the RFC 9110 error codes.
    /// </para>
    /// </summary>
    RfcHttpStatusCode = 0,

    /// <summary>
    /// <para>
    ///     Describe the generic erros as simple problems, where the http status code is enough to describe the error.
    /// </para>
    /// <para>
    ///     The problem details types will be about:blank, as recommended by the RFC 9457.
    ///     See more: <see href="https://www.rfc-editor.org/rfc/rfc9457#section-4.2.1" />.
    /// </para>
    /// </summary>
    AboutBlank = 1,
}