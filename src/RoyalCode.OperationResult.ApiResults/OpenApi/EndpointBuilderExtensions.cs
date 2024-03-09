#if NET7_0_OR_GREATER

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Any;
using RoyalCode.OperationResults;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IEndpointConventionBuilder"/>.
/// </summary>
public static class EndpointBuilderExtensions
{
    private const string ErrorMessage = $"{nameof(OpenApiOperation)} not found in metadata. Use the method {nameof(WithErrorResponseTypeHeader)} after the method WithOpenApi";

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TBuilder WithErrorResponseTypeHeader<TBuilder>(this TBuilder builder) 
        where TBuilder : IEndpointConventionBuilder
    {
        builder.Finally(b =>
        {
            var operation = b.Metadata.OfType<OpenApiOperation>().FirstOrDefault()
                ?? throw new InvalidOperationException(ErrorMessage);

            var example1 = new OpenApiExample()
            {
                Description = "Returns the error type 'OperationResult'",
                Value = new OpenApiString(nameof(OperationResult)),
            };

            var example2 = new OpenApiExample()
            {
                Description = "Returns the error type 'ProblemDetails'",
                Value = new OpenApiString(nameof(ProblemDetails)),
            };

            var parameter = new OpenApiParameter()
            {
                In = ParameterLocation.Header,
                AllowEmptyValue = true,
                Name = HeaderExtensions.ErrorTypeHeaderName,
            };
            parameter.Examples.Add(nameof(OperationResult), example1);
            parameter.Examples.Add(nameof(ProblemDetails), example2);

            operation?.Parameters.Add(parameter);
        });
        return builder;
    }
}

#endif