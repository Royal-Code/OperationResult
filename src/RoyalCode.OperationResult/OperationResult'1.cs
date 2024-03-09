
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
/// <typeparam name="TValue">The type of the value of the operation result.</typeparam>
public readonly struct OperationResult<TValue>
{
    /// <summary>
    /// Implicitly convert a value to a successful operation result.
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult<TValue>(TValue value) => new(value);

    /// <summary>
    /// Implicitly convert a <see cref="ResultErrors"/> error to a failure operation result.
    /// </summary>
    /// <param name="error"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult<TValue>(ResultErrors error) => new(error);

    /// <summary>
    /// Implicitly convert a <see cref="ResultMessage"/> error to a failure operation result.
    /// </summary>
    /// <param name="error"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult<TValue>(ResultMessage error) => new(error);

    /// <summary>
    /// Implicitly convert a <see cref="OperationResult{TValue}"/> error to a <see cref="OperationResult"/>
    /// </summary>
    /// <param name="other"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OperationResult(OperationResult<TValue> other)
    {
        return other.TryGetError(out var errors)
            ? new(errors)
            : new();
    }

    /// <summary>
    /// Adds a new message to the result collection if the result is a failure.
    /// </summary>
    /// <param name="result">The result to add the message to</param>
    /// <param name="message">The new message to add</param>
    /// <returns>The same instance of the collection</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OperationResult<TValue> operator +(OperationResult<TValue> result, ResultMessage message)
    {
        if (!result.Failure)
            throw new InvalidOperationException("Cannot add a message to a successful operation result.");

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
    public static OperationResult<TValue> operator +(OperationResult<TValue> result, ResultErrors messages)
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
    public static OperationResult<TValue> operator +(OperationResult<TValue> result, ValidableResult other)
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
    public static OperationResult<TValue> operator +(OperationResult<TValue> result, OperationResult other)
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
    public static OperationResult<TValue> operator +(OperationResult<TValue> result, OperationResult<TValue> other)
    {
        return other.TryGetError(out var messages)
            ? result + messages
            : result;
    }

    /// <summary>
    /// Extracts actual result value.
    /// </summary>
    /// <param name="result">The result object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator TValue?(in OperationResult<TValue> result) => result.value;

    private readonly TValue? value;
    private readonly ResultErrors? error;

    /// <summary>
    /// Creates a successful operation result.
    /// </summary>
    /// <param name="value">The value of the operation result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OperationResult(TValue value)
    {
        this.value = value;
        error = default;
    }

    /// <summary>
    /// Creates a failure operation result.
    /// </summary>
    /// <param name="error">The error of the operation result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OperationResult(ResultErrors error)
    {
        value = default;
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
    [MemberNotNullWhen(true, nameof(value))]
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
    [MemberNotNullWhen(false, nameof(value))]
#endif
    public readonly bool Failure { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => error is not null; }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure, then return true,
    ///     otherwise set the value of the operation result, because it is successful.
    /// </para>
    /// </summary>
    /// <param name="value">The value of the operation result.</param>
    /// <returns>Whether the operation result is failure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFailureOrGetValue([NotNullWhen(false)] out TValue? value)
    {
        value = this.value;
        return Failure;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure, then return true,
    ///     otherwise set the value of the operation result with the converted value, because it is successful.
    /// </para>
    /// </summary>
    /// <typeparam name="TOther">The type of the converted value.</typeparam>
    /// <param name="converter">The converter to convert the value.</param>
    /// <param name="value">The converted value of the operation result.</param>
    /// <returns>Whether the operation result is failure.</returns> 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFailureOrConvertValue<TOther>(
        Func<TValue, TOther> converter, [NotNullWhen(false)] out TOther? value)
        where TOther : notnull
    {
        if (Failure)
        {
            value = default;
            return true;
        }

        value = converter(this.value);
        return Failure;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure, then return true,
    ///     otherwise set the value of the operation result with the converted value, because it is successful.
    /// </para>
    /// </summary>
    /// <typeparam name="TOther">The type of the converted value.</typeparam>
    /// <typeparam name="TParam">The type of the parameter for the conversion.</typeparam>
    /// <param name="param">The parameter for the conversion.</param>
    /// <param name="converter">The converter to convert the value.</param>
    /// <param name="value">The converted value of the operation result.</param>
    /// <returns>Whether the operation result is failure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFailureOrConvertValue<TOther, TParam>(
        TParam param, Func<TValue, TParam, TOther> converter, [NotNullWhen(false)] out TOther? value)
        where TOther : notnull
    {
        if (Failure)
        {
            value = default;
            return true;
        }

        value = converter(this.value, param);
        return Failure;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure, then return true.
    ///     When is failure, set the <paramref name="error"/> of the operation result, so the <paramref name="value"/> is <see langword="null"/>.
    ///     When is successful, set the <paramref name="value"/> of the operation result, so the <paramref name="error"/> is <see langword="null"/>.
    /// </para>
    /// </summary>
    /// <param name="error">The error of the operation result, that is not <see langword="null"/> when the operation result is failure.</param>
    /// <param name="value">The value of the operation result, that is not <see langword="null"/> when the operation result is successful.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFailureAndGet([NotNullWhen(true)] out ResultErrors? error, [NotNullWhen(false)] out TValue? value)
    {
        value = this.value;
        error = this.error;
        return Failure;
    }

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
    ///     Check if the operation result is successful, then return true.
    ///     When is successful, set the <paramref name="value"/> of the operation result, so the <paramref name="error"/> is <see langword="null"/>.
    ///     When is failure, set the <paramref name="error"/> of the operation result, so the <paramref name="value"/> is <see langword="null"/>.
    /// </para>
    /// </summary>
    /// <param name="value">The value of the operation result, that is not <see langword="null"/> when the operation result is successful.</param>
    /// <param name="error">The error of the operation result, that is not <see langword="null"/> when the operation result is failure.</param>
    /// <returns></returns>
    public readonly bool IsSuccessAndGet([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out ResultErrors? error)
    {
        value = this.value;
        error = this.error;
        return Success;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is successful,
    ///     then return true and set the value of the operation result.
    /// </para>
    /// </summary>
    /// <param name="value">The value of the operation result.</param>
    /// <returns>Whether the operation result is successful.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = this.value;
        return Success;
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is successful,
    ///     then return true and set the value of the operation result with the converted value.
    /// </para>
    /// </summary>
    /// <typeparam name="TOther">The type of the converted value.</typeparam>
    /// <param name="converter">The converter to convert the value.</param>
    /// <param name="value">The converted value of the operation result.</param>
    /// <returns>Whether the operation result is successful.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryConvertValue<TOther>(
        Func<TValue, TOther> converter,
        [NotNullWhen(true)] out TOther? value)
        where TOther : notnull
    {
        if (Success)
        {
            value = converter(this.value);
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    /// <summary>
    /// <para>
    ///     Check if the operation result is successful,
    ///     then return true and set the value of the operation result with the converted value.
    /// </para>
    /// </summary>
    /// <typeparam name="TOther">The type of the converted value.</typeparam>
    /// <typeparam name="TParam">The type of the parameter for the conversion.</typeparam>
    /// <param name="param">The parameter for the conversion.</param>
    /// <param name="converter">The converter to convert the value.</param>
    /// <param name="value">The converted value of the operation result.</param>
    /// <returns>Whether the operation result is successful.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryConvertValue<TOther, TParam>(
        TParam param,
        Func<TValue, TParam, TOther> converter,
        [NotNullWhen(true)] out TOther? value)
        where TOther : notnull
    {
        if (Success)
        {
            value = converter(this.value, param);
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    /// <summary>
    /// <para>
    ///     Create a new operation result with a new value, converted from the current value.
    /// </para>
    /// <para>
    ///     When the operation result is a failure, the new operation result is also a failure.
    ///     When the operation result is successful, the new value is the result of the conversion.
    /// </para>
    /// </summary>
    /// <typeparam name="TOther">The other type of the result, after the conversion.</typeparam>
    /// <param name="converter">The function to convert the value.</param>
    /// <returns>The new operation result.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly OperationResult<TOther> Convert<TOther>(Func<TValue, TOther> converter)
        => Failure ? error : converter(value);

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
    /// <typeparam name="TOther">The other type of the result, after the conversion.</typeparam>
    /// <typeparam name="TParam">The type of the parameter for the conversion.</typeparam>
    /// <param name="converter">The function to convert the value.</param>
    /// <param name="param">The parameter for the conversion.</param>
    /// <returns>The new operation result.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly OperationResult<TOther, ResultErrors> Convert<TOther, TParam>(
        TParam param, Func<TValue, TParam, TOther> converter)
        => Failure ? error : converter(value, param);

    /// <summary>
    /// <para>
    ///     Convert the operation result to another type in case of failure.
    ///     This method can only be used when the operation result is a failure.
    /// </para>
    /// </summary>
    /// <typeparam name="TOther">The other type of the result.</typeparam>
    /// <returns>The new operation result.</returns>
    /// <exception cref="InvalidOperationException">
    ///     The operation result is not a failure.
    /// </exception>"
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly OperationResult<TOther> Convert<TOther>()
    {
        if (Failure)
            return error;
        throw new InvalidOperationException("The operation result is not a failure.");
    }

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
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<ResultErrors, TResult> failure)
        => Failure ? failure(error) : success(value);

    /// <summary>
    /// <para>
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <typeparam name="TParam">The type of the parameter passed to the match function.</typeparam>
    /// <param name="param">The parameter passed to the match function.</param>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult, TParam>(
        TParam param,
        Func<TValue, TParam, TResult> success,
        Func<ResultErrors, TParam, TResult> failure)
        => Failure ? failure(error, param) : success(value, param);

    /// <summary>
    /// <para>
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <typeparam name="TParam">The type of the parameter passed to the success match function.</typeparam>
    /// <param name="param">The parameter passed to the success match function.</param>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult, TParam>(
        TParam param,
        Func<TValue, TParam, TResult> success,
        Func<ResultErrors, TResult> failure)
        => Failure ? failure(error) : success(value, param);

    /// <summary>
    /// <para>
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <typeparam name="TParamSuccess">The type of the parameter passed to the match function when value is present.</typeparam>
    /// <typeparam name="TParamFailure">The type of the parameter passed to the match function when error is present.</typeparam>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <param name="paramSuccess">The parameter passed to the match function.</param>
    /// <param name="paramFailure">The parameter passed to the error function.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult, TParamSuccess, TParamFailure>(
        TParamSuccess paramSuccess, TParamFailure paramFailure,
        Func<TValue, TParamSuccess, TResult> success,
        Func<ResultErrors, TParamFailure, TResult> failure)
        => Failure ? failure(error, paramFailure) : success(value, paramSuccess);

    /// <summary>
    /// Convert the operation result to a string.
    /// When the operation result is successful, the string will be "Success: {value}".
    /// When the operation result is a failure, the string will be "Failure: {error}".
    /// </summary>
    /// <returns>The string representation of the operation result.</returns>
    public override readonly string ToString()
        => Failure ? $"Failure: {error}" : $"Success: {value}";
}