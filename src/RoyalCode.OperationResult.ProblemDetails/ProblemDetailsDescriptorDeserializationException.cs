namespace RoyalCode.OperationResults;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly

/// <summary>
/// Exception thrown when a collection of <see cref="ProblemDetailsDescription"/> cannot be deserialized.
/// </summary>
public sealed class ProblemDetailsDescriptorDeserializationException : Exception

{
    private const string MessagePattern = 
        "Failed to deserialize a collection of ProblemDetailsDescription, type '{0}', message: {1}, JSON:\n{2}";

    /// <summary>
    /// Creates a new instance of the exception.
    /// </summary>
    /// <param name="json">The JSON string used to deserialization.</param>
    /// <param name="innerException">The original exception.</param>
    public ProblemDetailsDescriptorDeserializationException(string json, Exception innerException)
        : base(string.Format(MessagePattern, innerException.GetType().Name, innerException.Message , json), innerException)
    { }
}