
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("RoyalCode.OperationResult.Tests")]

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}

internal static class JSON
{
    internal static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
}