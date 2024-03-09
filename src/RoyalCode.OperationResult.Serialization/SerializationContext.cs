
using System.Text.Json.Serialization;

namespace RoyalCode.OperationResults;

/// <summary>
/// Serialization context for the operation results types.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(IResultMessage), TypeInfoPropertyName = "AbstractResultMessage")]
[JsonSerializable(typeof(IEnumerable<ResultMessage>), TypeInfoPropertyName = "ResultMessageCollection")]
[JsonSerializable(typeof(ResultErrors))]
internal partial class SerializationContext : JsonSerializerContext { }
