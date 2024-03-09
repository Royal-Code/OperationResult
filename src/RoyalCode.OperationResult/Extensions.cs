#if NET6_0_OR_GREATER
using System.Diagnostics;
#endif

using System.Diagnostics.CodeAnalysis;

namespace RoyalCode.OperationResults;

/// <summary>
/// Extension methods for results and errors.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// It ensures that the errors is success, otherwise it fires a exception.
    /// </summary>
    /// <param name="result">The errors.</param>
    /// <returns>The same instance of <paramref name="result"/>.</returns>
    /// <exception cref="Exception">
    ///     Case the errors is not success.
    /// </exception>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#endif
    public static OperationResult EnsureSuccess(this OperationResult result)
    {
        if (result.TryGetError(out var errors))
            throw errors.CreateException();

        return result;
    }

    /// <summary>
    /// It ensures that the errors is success, otherwise it fires a exception.
    /// </summary>
    /// <param name="result">The errors.</param>
    /// <returns>The same instance of <paramref name="result"/>.</returns>
    /// <exception cref="Exception">
    ///     Case the errors is not success.
    /// </exception>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#endif
    public static OperationResult<TValue> EnsureSuccess<TValue>(this OperationResult<TValue> result)
    {
        if (result.TryGetError(out var errors))
            throw errors.CreateException();

        return result;
    }

    /// <summary>
    /// It ensures that the errors is success, otherwise it fires a exception.
    /// </summary>
    /// <param name="result">The errors.</param>
    /// <returns>The same instance of <paramref name="result"/>.</returns>
    /// <exception cref="Exception">
    ///     Case the errors is not success.
    /// </exception>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#endif
    public static ValidableResult EnsureSuccess(this ValidableResult result)
    {
        if (result.TryGetError(out var errors))
            throw errors.CreateException();

        return result;
    }

    /// <summary>
    /// Try get the value from the result, if it is success, otherwise it throws a exception.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The operation result.</param>
    /// <returns>The value.</returns>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#endif
    public static TValue GetValueOrThrow<TValue>(this OperationResult<TValue> result)
    {
        if (result.IsSuccessAndGet(out var value, out var errors))
            return value;

        throw errors.CreateException();
    }

    /// <summary>
    /// Check if the result is success then return true with the the value,
    /// otherwise return false with the exception.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The operation result.</param>
    /// <param name="exception">The exception, if the result is failure.</param>
    /// <param name="value">The value, if the result is success.</param>
    /// <returns>True if the result is success, otherwise false.</returns>
    public static bool GetValueOrException<TValue>(this OperationResult<TValue> result,
        [NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out Exception? exception)
    {
        if (result.IsSuccessAndGet(out var v, out var errors))
        {
            value = v;
            exception = null;
            return true;
        }

        value = default;
        exception = errors.CreateException();
        return false;
    }

    /// <summary>
    /// <para>
    ///     Check if the result is a failure, then create a exception from the errors.
    /// </para>
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="exception">The exception, if the result is failure.</param>
    /// <returns>True if the result is failure, otherwise false.</returns>
    public static bool TryGetException(this OperationResult result, [NotNullWhen(true)] out Exception? exception)
    {
        if (result.TryGetError(out var errors))
        {
            exception = errors.CreateException();
            return true;
        }

        exception = null;
        return false;
    }

    /// <summary>
    /// <para>
    ///     Check if the result is a failure, then create a exception from the errors.
    /// </para>
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="exception">The exception, if the result is failure.</param>
    /// <returns>True if the result is failure, otherwise false.</returns>
    public static bool TryGetException(this ValidableResult result, [NotNullWhen(true)] out Exception? exception)
    {
        if (result.TryGetError(out var errors))
        {
            exception = errors.CreateException();
            return true;
        }

        exception = null;
        return false;
    }

    /// <summary>
    /// <para>
    ///     Generate an exception from the message. In case the message contains an original exception, 
    ///     then that exception will be returned, else the property <see cref="IResultMessage.Property"/>
    ///     will be checked if is not null, then an <see cref="ArgumentException"/> will be generated, otherwise
    ///     a <see cref="InvalidOperationException"/> will be generated.
    /// </para>
    /// </summary>
    /// <param name="message">A errors message to extract or generate an exception.</param>
    /// <returns>
    /// <para>
    ///     An instance of an exception for the message.
    /// </para>
    /// </returns>
    public static Exception ToException(this IResultMessage message)
    {
        return message.Exception
            ?? message.TryCreateException()
            ?? (message.Property is null
                ? new InvalidOperationException(message.Text)
                : new ArgumentException(message.Text, message.Property));
    }

    private static Exception? TryCreateException(this IResultMessage message)
    {
        ExceptionsParsers.TryCreate(message, out var exception);
        return exception;
    }

    /// <summary>
    /// Creates a exception from the result errors.
    /// </summary>
    /// <param name="errors">The result errors.</param>
    /// <returns>An exception.</returns>
    public static Exception CreateException(this ResultErrors errors)
    {
        if (errors.Count == 1)
            return errors[0].ToException();

        var exceptions = errors
            .Select(m => m.ToException())
            .ToList();

        return new AggregateException("Multiple exceptions have occurred, check the internal exceptions to see the details.", exceptions);
    }

    /// <summary>
    /// Get the http status code from the errors.
    /// </summary>
    /// <param name="errors">The operation errors.</param>
    /// <returns>
    ///     The http status code, if any message has an http status code, and if so, returns that has the highest value.
    /// </returns>
    public static int GetHttpStatus(this ResultErrors? errors)
    {
        if (errors is null || errors.Count == 0)
            return 200;

        int httpStatus = 0;
        foreach (var status in errors.Where(m => m.Status is not null).Select(m => (int)m.Status!.Value))
        {
            if (httpStatus == 0
                || (status == 400 && httpStatus == 404) // bad request is better than not found
                || (status == 409 && httpStatus < 500)) // conflict is better than other errors, except server errors
            {
                httpStatus = status;
                continue;
            }

            if (httpStatus >= status)
                continue;

            httpStatus = status;
        }

        return httpStatus == 0 ? 400 : httpStatus;
    }
}
