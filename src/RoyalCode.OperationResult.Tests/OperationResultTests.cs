
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace RoyalCode.OperationResults.Tests;

public class OperationResultTests
{
    [Fact]
    public void ImplicitlyConvertValueToSuccessfulOperationResult()
    {
        var value = new object();
        OperationResult<object, string> operationResult = value;

        Assert.True(operationResult.Success);
        Assert.False(operationResult.Failure);
    }

    [Fact]
    public void ImplicitlyConvertErrorToFailureOperationResult()
    {
        var error = "error";
        OperationResult<object, string> operationResult = error;

        Assert.False(operationResult.Success);
        Assert.True(operationResult.Failure);
    }

    [Fact]
    public void CreateSuccessfulOperationResult()
    {
        var value = new object();
        OperationResult<object, string> operationResult = new(value);

        Assert.True(operationResult.Success);
        Assert.False(operationResult.Failure);
    }

    [Fact]
    public void CreateFailureOperationResult()
    {
        var error = "error";
        OperationResult<object, string> operationResult = new(error);

        Assert.False(operationResult.Success);
        Assert.True(operationResult.Failure);
    }

    [Fact]
    public void GetValueFromSuccessfulOperationResult()
    {
        var value = new object();
        OperationResult<object, string> operationResult = new(value);

        Assert.False(operationResult.IsFailureOrGetValue(out var gettedValue));
        Assert.Equal(value, gettedValue);
    }

    [Fact]
    public void GetErrorFromSuccessfulOperationResult()
    {
        var value = new object();
        OperationResult<object, string> operationResult = new(value);

        Assert.True(operationResult.IsSuccessOrGetError(out var error));
        Assert.Null(error);
    }

    [Fact]
    public void GetValueFromFailureOperationResult()
    {
        var error = "error";
        OperationResult<object, string> operationResult = new(error);

        Assert.True(operationResult.IsFailureOrGetValue(out var value));
        Assert.Null(value);
    }

    [Fact]
    public void GetErrorFromFailureOperationResult()
    {
        var error = "error";
        OperationResult<object, string> operationResult = new(error);

        Assert.False(operationResult.IsSuccessOrGetError(out var gettedError));
        Assert.Equal(error, gettedError);
    }

    [Fact]
    public void AddMoreErrorUsingPlusOperator()
    {
        OperationResult result = ResultMessage.Error("Error message 1");
        result += ResultMessage.Error("Error message 2");

        Assert.False(result.IsSuccessOrGetError(out var gettedError));
        Assert.Equal("Error message 1", gettedError[0].Text);
        Assert.Equal("Error message 2", gettedError[^1].Text);
    }

    [Fact]
    public void WithPointerSettingTheValue()
    {
        OperationResult result = ResultMessage.Error("Error message 1")
            .WithPointer("#/property1");

        Assert.False(result.IsSuccessOrGetError(out var gettedError));
        var message = gettedError[0];
        Assert.Equal("#/property1", message.GetPointer());
    }

    [Fact]
    public void WithPointerFromProperty()
    {
        OperationResult result = ResultMessage.Error("error-code-1", "Error message 1", "Property1")
            .WithPointer();

        Assert.False(result.IsSuccessOrGetError(out var gettedError));
        var message = gettedError[0];
        Assert.Equal("#/property1", message.GetPointer());
    }

    [Fact]
    public void WithPointerWithouPropertyMustThrow()
    {
        ResultMessage message = ResultMessage.Error("Error message 1");

        Assert.Throws<InvalidOperationException>(message.WithPointer);
    }

    [Fact]
    public void MatchSuccessfulOperationResult()
    {
        var value = new object();
        OperationResult<object> result = value;

        Response response = result.Match<Response>(
            success: v => new(v, 200),
            failure: e => new(e, 400));

        Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public void MatchFailureOperationResult()
    {
        var error = ResultMessage.Error("error");
        OperationResult<object> result = error;

        Response response = result.Match<Response>(
            success: v => new(v, 200),
            failure: e => new(e, 400));

        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public void MatchConvertSuccessfulOperationResult_1()
    {
        var value = new object();
        OperationResult<object> result = value;

        var serializer = new ResponseSerializer();

        Response response = result.Match<Response, ResponseSerializer>(
            serializer,
            success: static (v, s) => new(s.Serialize(v), 200, "application/json"),
            failure: static (e, s) => new(s.Serialize(e), 400, "application/json"));

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("application/json", response.ContentType);
    }

    [Fact]
    public void MatchConvertSuccessfulOperationResult_2()
    {
        var value = new object();
        OperationResult<object> result = value;

        var serializer = new ResponseSerializer();

        Response response = result.Match<Response, ResponseSerializer>(
            serializer,
            success: static (v, s) => new(s.Serialize(v), 200, "application/json"),
            failure: static e => new(e, 400));

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("application/json", response.ContentType);
    }
}

file class Response
{
    public Response(object? bodyObject, int statusCode)
    {
        BodyObject = bodyObject;
        StatusCode = statusCode;
    }

    public Response(byte[] body, int statusCode, string contentType)
    {
        BodyObject = body ?? throw new ArgumentNullException(nameof(body));
        StatusCode = statusCode;
        ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
    }

    public object? BodyObject { get; }

    public int StatusCode { get; }

    public string? ContentType { get; }
}

file class ResponseSerializer
{
    public byte[] Serialize(object? value)
    {
        if (value is null)
            return Array.Empty<byte>();

        if (value is string stringValue)
            return Encoding.UTF8.GetBytes(stringValue);

        return JsonSerializer.SerializeToUtf8Bytes(value);
    }
}

file static class UsageSamples
{
    private readonly static Func<UsageArguments, UsageValidationResult> Validator = args =>
    {
        return new UsageValidationResult();
    };

    public static OperationResult<object, string> ValidationSample(UsageArguments arg)
    {
        // retornando um erro de operação diretamente
        if (arg is null)
            return "Error: argument is null";

        // retornando um erro de operação a partir de uma validação
        var validation = Validator(arg);
        if (validation.HasError)
            return validation.Error;

        // retornando um erro de operação a partir de outro outro resultado gerado por algum método
        var result = arg.DoSomething();
        if (result.IsFailureOrGetValue(out var value))
            return result;

        Console.WriteLine($"Value: {value}");

        // retornando um erro de operação a partir de outro outro resultado (de tipo diferente) gerado por algum método
        var otherResult = arg.DoOtherthing();
        if (otherResult.TryGetError(out var error))
            return error;

        // retornando um resultado com outro tipo de erro, tenta converter (para string) se é falha.
        var resultWithMessage = arg.DoSomethingWithMessage();
        if (resultWithMessage.TryConvertError(rm => rm.Text, out error))
            return error;


        return arg;
    }

}

file sealed class UsageArguments
{

    public OperationResult<object, string> DoSomething()
    {
        return new object();
    }

    public OperationResult<bool, string> DoOtherthing()
    {
        return true;
    }

    public OperationResult<object, IResultMessage> DoSomethingWithMessage()
    {
        return new object();
    }
}

file sealed class UsageValidationResult
{
    [MemberNotNullWhen(true, nameof(Error))]
    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public string? Error { get; set; }
}
