using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RoyalCode.OperationResults;
#if NETSTANDARD2_1
#pragma warning disable CS8604 // Possible null reference argument.
#endif

/// <summary>
/// <para>
///     A validable result represents the result of an operation that can be either successful or a failure.
/// </para>
/// <para>
///     This result type is similar to <see cref="OperationResult{TValue, TError}"/>, but it does not contain a value.
/// </para>
/// </summary>
/// <typeparam name="TError">The type of the error.</typeparam>
public readonly struct ValidableResult<TError> 
{
    /// <summary>
    /// Implicitly convert an error to a failure operation result.
    /// </summary>
    /// <param name="error"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValidableResult<TError>(TError error) => new(error);

    /// <summary>
    /// Extracts actual result error.
    /// </summary>
    /// <param name="result">The result object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator TError?(in ValidableResult<TError> result) => result.error;

    private readonly TError? error;

    /// <summary>
    /// Creates a successful operation result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValidableResult()
    {
        Failure = false;
    }

    /// <summary>
    /// Creates a failure operation result.
    /// </summary>
    /// <param name="error">The error of the operation result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValidableResult(TError error)
    {
        this.error = error;
        Failure = true;
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
    public readonly bool Success { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => !Failure; }

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
    public readonly bool Failure { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

    /// <summary>
    /// <para>
    ///     Check if the operation result is failure, 
    ///     then return true and set the error of the operation result.
    /// </para>
    /// </summary>
    /// <param name="error">The error of the operation result.</param>
    /// <returns>Whether the operation result is failure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetError([NotNullWhen(true)] out TError? error)
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
        Func<TError, TOtherError> converter,
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
    public readonly bool IsSuccessOrGetError([NotNullWhen(false)] out TError? error)
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
        Func<TError, TOtherError> converter,
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
    ///     Match a function depending on the operation result.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type returned by the match function.</typeparam>
    /// <param name="success">The function to execute if the operation result is successful.</param>
    /// <param name="failure">The function to execute if the operation result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<TResult> success, Func<TError, TResult> failure)
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
        Func<TError, TParam, TResult> failure)
        => Failure ? failure(error, param) : success(param);

    /// <summary>
    /// Convert the operation result to a string.
    /// When the operation result is successful, the string will be "Success: {value}".
    /// When the operation result is a failure, the string will be "Failure: {error}".
    /// </summary>
    /// <returns>The string representation of the operation result.</returns>
    public override readonly string ToString()
        => Failure ? $"Failure: {error}" : "Success";
}
