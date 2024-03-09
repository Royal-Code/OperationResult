using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RoyalCode.OperationResults;

/// <summary>
/// <para>
///     Represents the result of an operation.
/// </para>
/// <para>
///     The operation result can be either successful or a failure.
/// </para>
/// </summary>
public readonly struct OperationResult
{
    /// <summary>
    /// Implicitly convert a <see cref="ResultErrors"/> error to a failure operation result.
    /// </summary>
    /// <param name="error"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult(ResultErrors error) => new(error);

    /// <summary>
    /// Implicitly convert a <see cref="ResultMessage"/> error to a failure operation result.
    /// </summary>
    /// <param name="error"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult(ResultMessage error) => new(error);

    /// <summary>
    /// Implicitly convert a <see cref="ValidableResult"/> error to a <see cref="OperationResult"/>.
    /// </summary>
    /// <param name="other"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult(ValidableResult other)
    {
        return other.TryGetError(out var errors) 
            ? new(errors) 
            : new();
    }

    /// <summary>
    /// Adds a new messages to the result collection if the result is a failure.
    /// </summary>
    /// <param name="result">The result to add the messages to</param>
    /// <param name="message">The new messages to add</param>
    /// <returns>The same instance of the collection</returns>
    /// <exception cref="InvalidOperationException">
    ///     Case the result is not a failure.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OperationResult operator +(OperationResult result, ResultMessage message)
    {
        if (!result.Failure)
            throw new InvalidOperationException("Cannot add a messages to a successful operation result.");

        result.error.Add(message);
        return result;
    }

    /// <summary>
    /// Adds a range of messages to the result collection if the result is a failure.
    /// </summary>
    /// <param name="result">The result to add the messages to</param>
    /// <param name="messages">The new messages to add</param>
    /// <returns>The same instance of the collection</returns>
    /// <exception cref="InvalidOperationException">
    ///     Case the result is not a failure.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OperationResult operator +(OperationResult result, ResultErrors messages)
    {
        if (!result.Failure)
            throw new InvalidOperationException("Cannot add a messages to a successful operation result.");

        result.error.AddRange(messages);
        return result;
    }

    /// <summary>
    /// Adds a range of messages to the result collection from other result if the both result is a failure.
    /// If the result is not a failure, a new result is created with the message.
    /// </summary>
    /// <param name="result">The result to add the messages to</param>
    /// <param name="other">The result to add</param>
    /// <returns>The same instance of the collection</returns>
    /// <exception cref="InvalidOperationException">
    ///     Case the result is not a failure.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OperationResult operator +(OperationResult result, ValidableResult other)
    {
        return other.TryGetError(out var messages)
            ? result + messages
            : result;
    }

    /// <summary>
    /// Adds a range of messages to the result collection from other result if the both result is a failure.
    /// If the result is not a failure, a new result is created with the message.
    /// </summary>
    /// <param name="result">The result to add the messages to</param>
    /// <param name="other">The result to add</param>
    /// <returns>The same instance of the collection</returns>
    /// <exception cref="InvalidOperationException">
    ///     Case the result is not a failure.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OperationResult operator +(OperationResult result, OperationResult other)
    {
        return other.TryGetError(out var messages)
            ? result + messages
            : result;
    }

    private readonly ResultErrors? error;

    /// <summary>
    /// Creates a successful operation result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OperationResult()
    {
        error = default;
    }

    /// <summary>
    /// Creates a failure operation result.
    /// </summary>
    /// <param name="error">The error of the operation result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OperationResult(ResultErrors error)
    {
        this.error = error;
    }

    /// <summary>
    /// <para>
    ///     Whether the operation result is successful.
    /// </para>
    /// <para>
    ///     When the operation result is successful, the value is not <see langword="null"/>,
    ///     and the error is <see langword="null"/>.
    /// </para>
    /// </summary>
#if NET6_0_OR_GREATER
    [MemberNotNullWhen(false, nameof(error))]
#endif
    public readonly bool Success { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => error is null; }

    /// <summary>
    /// <para>
    ///     Whether the operation result is a failure.
    /// </para>
    /// <para>
    ///     When the operation result is a failure, the value is <see langword="null"/>,
    ///     and the error is not <see langword="null"/>.
    /// </para>
    /// </summary>
#if NET6_0_OR_GREATER
    [MemberNotNullWhen(true, nameof(error))]
#endif
    public readonly bool Failure { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => error is not null ; }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure, 
    ///     then return true and set the error of the operation result.
    /// </para>
    /// </summary>
    /// <param name="error">The error of the operation result.</param>
    /// <returns>Whether the operation result is failure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetError([NotNullWhen(true)] out ResultErrors? error)
    {
        error = this.error;
        return Failure;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure,
    ///     then return true and set the error of the operation result with the converted error.
    /// </para>
    /// </summary>
    /// <typeparam name="TOtherError">The type of the converted error.</typeparam>
    /// <param name="converter">The converter to convert the error.</param>
    /// <param name="error">The converted error of the operation result.</param>
    /// <returns>Whether the operation result is failure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryConvertError<TOtherError>(
        Func<ResultErrors, TOtherError> converter,
        [NotNullWhen(true)] out TOtherError? error)
        where TOtherError : notnull
    {
        if (Failure)
        {
            error = converter(this.error);
            return true;
        }
        else
        {
            error = default;
            return false;
        }
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is successful, then return true,
    ///     otherwise set the error of the operation result, because it is failure.
    /// </para>
    /// </summary>
    /// <param name="error">The error of the operation result.</param>
    /// <returns>Whether the operation result is successful.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsSuccessOrGetError([NotNullWhen(false)] out ResultErrors? error)
    {
        error = this.error;
        return !Failure;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is successful, then return true,
    ///     otherwise set the error of the operation result with the converted error, because it is failure.
    /// </para>
    /// </summary>
    /// <typeparam name="TOtherError">The type of the converted error.</typeparam>
    /// <param name="converter">The converter to convert the error.</param>
    /// <param name="error">The converted error of the operation result.</param>
    /// <returns>Whether the operation result is successful.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsSuccessOrConvertError<TOtherError>(
        Func<ResultErrors, TOtherError> converter,
        [NotNullWhen(false)] out TOtherError? error)
        where TOtherError : notnull
    {
        if (Failure)
        {
            error = converter(this.error);
            return false;
        }
        else
        {
            error = default;
            return true;
        }
    }

    /// <summary>
    /// <para>
    ///     Create a new operation result with a the value.
    /// </para>
    /// <para>
    ///     When the operation result is a failure, the new operation result is also a failure and the value is ignored.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of the result value.</typeparam>
    /// <param name="value">The value of the operation result.</param>
    /// <returns>The new operation result.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly OperationResult<TValue> Convert<TValue>(TValue value)
        => Failure ? error : value;

    /// <summary>
    /// <para>
    ///     Create a new operation result with a new value, converted from the current value.
    /// </para>
    /// <para>
    ///     When the operation result is a failure, the new operation result is also a failure.
    ///     When the operation result is successful, the new value is the result of the conversion.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of the result, after the conversion.</typeparam>
    /// <param name="converter">The function to convert the value.</param>
    /// <returns>The new operation result.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly OperationResult<TValue> Convert<TValue>(Func<TValue> converter)
        => Failure ? error : converter();

    /// <summary>
    /// <para>
    ///     Create a new operation result with a new value, converted from the current value.
    ///     This method also takes a parameter for the conversion.
    /// </para>
    /// <para>
    ///     When the operation result is a failure, the new operation result is also a failure.
    ///     When the operation result is successful, the new value is the result of the conversion.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of the result, after the conversion.</typeparam>
    /// <typeparam name="TParam">The type of the parameter for the conversion.</typeparam>
    /// <param name="converter">The function to convert the value.</param>
    /// <param name="param">The parameter for the conversion.</param>
    /// <returns>The new operation result.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly OperationResult<TValue> Convert<TValue, TParam>(
        TParam param, Func<TParam, TValue> converter)
        => Failure ? error : converter(param);

    /// <summary>
    /// <para>
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<TResult> success, Func<ResultErrors, TResult> failure)
        => Failure ? failure(error) : success();

    /// <summary>
    /// <para>
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <typeparam name="TParam">The type of the parameter passed to the match function.</typeparam>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <param name="param">The parameter passed to the match function.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult, TParam>(
        TParam param,
        Func<TParam, TResult> success,
        Func<ResultErrors, TParam, TResult> failure)
        => Failure ? failure(error, param) : success(param);

    /// <summary>
    /// <para>
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <typeparam name="TParam">The type of the parameter passed to the match function.</typeparam>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <param name="param">The parameter passed to the match function.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult, TParam>(
        TParam param,
        Func<TParam, TResult> success,
        Func<ResultErrors, TResult> failure)
        => Failure ? failure(error) : success(param);

    /// <summary>
    /// Convert the operation result to a string.
    /// When the operation result is successful, the string will be "Success: {value}".
    /// When the operation result is a failure, the string will be "Failure: {error}".
    /// </summary>
    /// <returns>The string representation of the operation result.</returns>
    public override readonly string ToString()
        => Failure ? $"Failure: {error}" : $"Success";
}
