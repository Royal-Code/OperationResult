using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace RoyalCode.OperationResults;

/// <summary>
/// A collection of results that can be deserialized.
/// </summary>
public static class ResultErrorsSerialization
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// The <see cref="JsonTypeInfo"/> for <see cref="IEnumerable{ResultMessage}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JsonTypeInfo<IEnumerable<ResultMessage>> GetResultMessagesTypeInfo()
        => SerializationContext.Default.ResultMessageCollection;

    /// <summary>
    /// Get the <see cref="JsonTypeInfo"/> for <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="_">Used for extension methods.</param>
    /// <returns>
    ///     The <see cref="JsonTypeInfo"/> for <see cref="ResultErrors"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JsonTypeInfo<ResultErrors> GetJsonTypeInfo(this ResultErrors _)
        => SerializationContext.Default.ResultErrors;

    /// <summary>
    /// Get the <see cref="JsonSerializerOptions"/> for <see cref="ResultErrors"/>.
    /// </summary>
    /// <param name="_">Used for extension methods.</param>
    /// <returns>The <see cref="JsonSerializerOptions"/> for <see cref="ResultErrors"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JsonSerializerOptions GetJsonSerializerOptions(this ResultErrors _) => jsonSerializerOptions;
}