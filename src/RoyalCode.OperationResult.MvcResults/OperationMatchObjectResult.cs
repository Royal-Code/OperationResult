using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults;

/// <summary>
/// A Mvc <see cref="ObjectResult"/> for <see cref="OperationResult"/>.
/// </summary>
public sealed class OperationMatchObjectResult : OperationMatchObjectResultBase<OperationResult>
{
    /// <summary>
    /// Determines if the default result is <see cref="OperationResult"/> or <see cref="ProblemDetails"/>.
    /// </summary>
    public static bool IsProblemDetailsDefault { get; set; } = false;

    /// <summary>
    /// Creates a new instance of <see cref="OperationMatchObjectResult"/>.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">Optional, the path created by the operation.</param>
    public OperationMatchObjectResult(
        OperationResult result,
        string? createdPath = null) : base(result, createdPath)
    { }

    /// <inheritdoc />
    protected override Task ExecuteMatchAsync(ActionContext context)
        => Result.Match(context, ExecuteSuccessResultAsync, ExecuteErrorResultAsync);

    private Task ExecuteSuccessResultAsync(ActionContext context)
    {
        Value = null;

        if (CreatedPath is not null)
        {
            StatusCode = StatusCodes.Status201Created;
            context.HttpContext.Response.Headers.Add("Location", CreatedPath);
        }
        else
        {
            StatusCode = StatusCodes.Status204NoContent;
        }
        
        return BaseExecuteResultAsync(context);
    }
}

/// <summary>
/// A MVC <see cref="ObjectResult"/> for <see cref="OperationResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public sealed class OperationMatchObjectResult<TValue> : OperationMatchObjectResultBase<OperationResult<TValue>>
{
    /// <summary>
    /// Creates a new instance of <see cref="OperationMatchObjectResult{TValue}"/>.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">Optional, the path created by the operation.</param>
    /// <param name="formatPathWithValue">If true, the <paramref name="createdPath"/> will be formatted with the value of the result.</param>
    public OperationMatchObjectResult(
        OperationResult<TValue> result,
        string? createdPath = null,
        bool formatPathWithValue = false) : base(result, createdPath)
    {
        FormatPathWithValue = formatPathWithValue;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OperationMatchObjectResult{TValue}"/>.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPathProvider">A function to provide the value for the <c>Location</c> header.</param>
    public OperationMatchObjectResult(OperationResult<TValue> result, Func<TValue, string> createdPathProvider) 
        : base(result)
    {
        CreatedPathProvider = createdPathProvider;
    }

    /// <summary>
    /// If true, the <see cref="OperationMatchObjectResultBase{TResult}.CreatedPath"/> will be formatted with the value of the result.
    /// </summary>
    public bool FormatPathWithValue { get; }

    /// <summary>
    /// A function to provide the value for the <c>Location</c> header.
    /// </summary>
    public Func<TValue, string>? CreatedPathProvider { get; set; }

    /// <inheritdoc />
    protected override Task ExecuteMatchAsync(ActionContext context)
        => Result.Match(context, ExecuteSuccessResultAsync, ExecuteErrorResultAsync);

    private Task ExecuteSuccessResultAsync(TValue value, ActionContext context)
    {
        Value = value;

        if (CreatedPath is not null)
        {
            StatusCode = StatusCodes.Status201Created;

            var createdPath = FormatPathWithValue
                    ? string.Format(CreatedPath, value)
                    : CreatedPath;
            
            context.HttpContext.Response.Headers.Add("Location", createdPath);
        }
        else if (CreatedPathProvider is not null)
        {
            StatusCode = StatusCodes.Status201Created;
            context.HttpContext.Response.Headers.Add("Location", CreatedPathProvider(value));
        }
        else
        {
            StatusCode = StatusCodes.Status200OK;
        }

        return BaseExecuteResultAsync(context);
    }
}

/// <summary>
/// A Mvc <see cref="ObjectResult"/> for <see cref="OperationResult"/>.
/// </summary>
public sealed class ValidableMatchObjectResult : OperationMatchObjectResultBase<ValidableResult>
{
    /// <summary>
    /// Determines if the default result is <see cref="OperationResult"/> or <see cref="ProblemDetails"/>.
    /// </summary>
    public static bool IsProblemDetailsDefault { get; set; } = false;

    /// <summary>
    /// Creates a new instance of <see cref="OperationMatchObjectResult"/>.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="createdPath">Optional, the path created by the operation.</param>
    public ValidableMatchObjectResult(
        ValidableResult result,
        string? createdPath = null) : base(result, createdPath)
    { }

    /// <inheritdoc />
    protected override Task ExecuteMatchAsync(ActionContext context)
        => Result.Match(context, ExecuteSuccessResultAsync, ExecuteErrorResultAsync);

    private Task ExecuteSuccessResultAsync(ActionContext context)
    {
        Value = null;

        if (CreatedPath is not null)
        {
            StatusCode = StatusCodes.Status201Created;
            context.HttpContext.Response.Headers.Add("Location", CreatedPath);
        }
        else
        {
            StatusCode = StatusCodes.Status204NoContent;
        }

        return BaseExecuteResultAsync(context);
    }
}