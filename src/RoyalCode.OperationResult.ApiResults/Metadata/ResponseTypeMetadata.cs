using Microsoft.AspNetCore.Http.Metadata;
using System.Net.Mime;

namespace RoyalCode.OperationResults.Metadata;

internal sealed class ResponseTypeMetadata : IProducesResponseTypeMetadata
{
    public ResponseTypeMetadata(Type? type, int statusCode, params string[]? contentTypes)
    {
        Type = type;
        StatusCode = statusCode;
        ContentTypes = contentTypes ?? new[] { MediaTypeNames.Application.Json };
    }

    public ResponseTypeMetadata(int statusCode, params string[]? contentTypes)
    {
        StatusCode = statusCode;
        ContentTypes = contentTypes ?? new[] { MediaTypeNames.Application.Json };
    }

    public Type? Type { get; }
    public int StatusCode { get; }
    public IEnumerable<string> ContentTypes { get; }
}
