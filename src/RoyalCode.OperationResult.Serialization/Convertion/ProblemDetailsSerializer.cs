using RoyalCode.OperationResults.Convertion.Internals;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace RoyalCode.OperationResults.Convertion;

/// <summary>
/// A <see cref="JsonSerializerContext"/> for the <see cref="ProblemDetailsExtended"/>.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ProblemDetailsExtended))]
[JsonSerializable(typeof(InvalidParameterDetails))]
[JsonSerializable(typeof(NotFoundDetails))]
[JsonSerializable(typeof(ErrorDetails))]
[JsonSerializable(typeof(ProblemDetails))]
public partial class ProblemDetailsSerializer : JsonSerializerContext
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// The default <see cref="JsonTypeInfo{T}"/> for <see cref="ProblemDetailsExtended"/>.
    /// </summary>
    public static JsonTypeInfo<ProblemDetailsExtended> DefaultProblemDetailsExtended 
        => Default.ProblemDetailsExtended;

    /// <summary>
    /// Options to deserialize using Web defaults.
    /// </summary>
    public static JsonSerializerOptions JsonSerializerOptions => jsonSerializerOptions;
}