using RoyalCode.OperationResults;
using RoyalCode.OperationResults.Convertion;
using System.Net.Http.Json;
using System.Text.Json;

namespace System.Net.Http;

#if NETSTANDARD2_1
#pragma warning disable IDE0079 // Remove unnecessary suppression
#endif

/// <summary>
/// <para>
///     Extension methods for deserialize <see cref="OperationResult" /> from <see cref="HttpResponseMessage"/>.
/// </para>
/// </summary>
public static class HttpOperationResultExtensions
{
    /// <summary>
    /// <para>
    ///     Get <see cref="OperationResult" /> from <see cref="HttpResponseMessage"/>.
    /// </para>
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/>.</param>
    /// <param name="token">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="OperationResult"/>.</returns>
    public static async Task<OperationResult> ToOperationResultAsync(
        this HttpResponseMessage response, CancellationToken token = default)
    {
        if (response.IsSuccessStatusCode)
        {
            // on success, when there is no value,
            // returns a success OperationResult with no message
            return new();
        }
        else
        {
            return await response.ReadErrorStatus(token);
        }
    }

    /// <summary>
    /// <para>
    ///     Get <see cref="OperationResult{TValue}" /> from <see cref="HttpResponseMessage"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="response">The <see cref="HttpResponseMessage"/>.</param>
    /// <param name="options">
    ///     The <see cref="JsonSerializerOptions"/> for the <typeparamref name="TValue"/>, 
    ///     used when status code is success.
    /// </param>
    /// <param name="token">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="OperationResult{TValue}"/>.</returns>
    public static async Task<OperationResult<TValue>> ToOperationResultAsync<TValue>(
        this HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken token = default)
    {
        if (response.IsSuccessStatusCode)
        {
            // on success, with value, the value must be deserialized
            var value = await response.Content.ReadFromJsonAsync<TValue>(options, token);

            return value!;
        }
        else
        {
            return await response.ReadErrorStatus(token);
        }
    }

    private static Task<ResultErrors> ReadErrorStatus(this HttpResponseMessage response, CancellationToken token)
    {
        var mediaType = response.Content.Headers.ContentType?.MediaType;
        return mediaType switch
        {
            // check the content
            "application/json" => response.ReadResultErrors(token),
            "application/problem+json" => response.ReadProblemDetails(token),
            _ => response.ReadNonJsonContent(token)
        };
    }

#if NETSTANDARD2_1
    [Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell", "S1172:Unused method parameters should be removed",
        Justification = "Used when target is net6+")]
#endif
    private static async Task<ResultErrors> ReadNonJsonContent(
        this HttpResponseMessage response, CancellationToken token = default)
    {
        // when is not json, try reads the content as string, if the response has content
        string? text = null;
        if (response.Content.Headers.ContentLength > 0)
        {
#if NET6_0_OR_GREATER
            text = await response.Content.ReadAsStringAsync(token);
#else
            text = await response.Content.ReadAsStringAsync();
#endif
        }

        // create a message with the status code and the content as message
        return text is not null 
            ? ResultMessage.Error(text, response.StatusCode) 
            : ResultMessage.Error(response.ReasonPhrase ?? response.StatusCode.ToString(), response.StatusCode);
    }

    private static async Task<ResultErrors> ReadResultErrors(
        this HttpResponseMessage response, CancellationToken token)
    {
        // in case of errors, a collection of messages must be deserialized
        var messages = await response.Content.ReadFromJsonAsync(
            ResultErrorsSerialization.GetResultMessagesTypeInfo(),
            token);

        var result = new ResultErrors(messages ?? Enumerable.Empty<ResultMessage>());

        if (result.Count == 0)
        {
            // if there is no message, add a default message
            result.Add(new ResultMessage(response.ReasonPhrase ?? response.StatusCode.ToString(),
                null, null, response.StatusCode));
        }
        else
        {
            var status = response.StatusCode;
            // provides the status code of the response for each message
            foreach (var message in result)
            {
                message.Status = status;
            }
        }

        return result;
    }

    private static async Task<ResultErrors> ReadProblemDetails(
        this HttpResponseMessage response, CancellationToken token)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync(
            ProblemDetailsSerializer.DefaultProblemDetailsExtended,
            token);

        return problemDetails!.ToResultErrors();
    }
}
