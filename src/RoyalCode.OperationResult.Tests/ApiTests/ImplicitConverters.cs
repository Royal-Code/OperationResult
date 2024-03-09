using Microsoft.AspNetCore.Http;
using RoyalCode.OperationResults.HttpResults;

namespace RoyalCode.OperationResults.Tests.ApiTests;

#pragma warning disable S1854 // Unused assignments should be removed
#pragma warning disable IDE0059 // Unnecessary assignment of a value

public class ImplicitConvertersTests
{
    [Fact]
    public void ExecuteImplicitConverters()
    {
        var noContentMatch = ImplicitConverters.ReturnsNoContentMatch();
        var okMatch = ImplicitConverters.ReturnsOkMatch();
        var okMatchT = ImplicitConverters.ReturnsOkMatch("x");
        var createdMatch = ImplicitConverters.ReturnsCreatedMatch();
        var createdMatchT = ImplicitConverters.ReturnsCreatedMatch("x");
        
        Assert.NotNull(noContentMatch);
        Assert.NotNull(okMatch);
        Assert.NotNull(okMatchT);
        Assert.NotNull(createdMatch);
        Assert.NotNull(createdMatchT);
    }
}

internal static class ImplicitConverters
{
    public static NoContentMatch ReturnsNoContentMatch()
    {
        NoContentMatch noContentMatch;

        // Implicit conversion from OperationResult
        noContentMatch = new OperationResult();

        // Implicit conversion from ValidableResult
        noContentMatch = new ValidableResult();

        // Implicit conversion from NoContent
        noContentMatch = TypedResults.NoContent();

        // Implicit conversion from MatchErrorResult
        noContentMatch = new MatchErrorResult(ResultMessage.Error("x"));

        // Implicit conversion from ResultMessage
        noContentMatch = ResultMessage.ApplicationError("x");

        return noContentMatch;
    }

    public static OkMatch<T> ReturnsOkMatch<T>(T value)
    {
        OkMatch<T> okMatch;

        // Implicit conversion from OperationResult
        okMatch = new OperationResult<T>(value);

        // Implicit conversion from Ok<T>
        okMatch = TypedResults.Ok(value);

        // Implicit conversion from T
        okMatch = value;

        // Implicit conversion from MatchErrorResult
        okMatch = new MatchErrorResult(ResultMessage.Error("x"));

        // Implicit conversion from ResultMessage
        okMatch = ResultMessage.ApplicationError("x");

        return okMatch;
    }

    public static OkMatch ReturnsOkMatch()
    {
        OkMatch okMatch;

        // Implicit conversion from OperationResult
        okMatch = new OperationResult();

        // Implicit conversion from ValidableResult
        okMatch = new ValidableResult();

        // Implicit conversion from Ok
        okMatch = TypedResults.Ok();

        // Implicit conversion from MatchErrorResult
        okMatch = new MatchErrorResult(ResultMessage.Error("x"));

        // Implicit conversion from ResultMessage
        okMatch = ResultMessage.ApplicationError("x");

        return okMatch;
    }

    public static CreatedMatch<T> ReturnsCreatedMatch<T>(T value)
    {
        CreatedMatch<T> createdMatch;

        // Implicit conversion from Created<T>
        createdMatch = TypedResults.Created("path", value);

        // Implicit conversion from MatchErrorResult
        createdMatch = new MatchErrorResult(ResultMessage.Error("x"));

        // Implicit conversion from ResultMessage
        createdMatch = ResultMessage.ApplicationError("x");

        // Implicit conversion from ResultErrors
        var errors = new ResultErrors();
        errors += ResultMessage.Error("x");
        createdMatch = errors;

        return createdMatch;
    }

    public static CreatedMatch ReturnsCreatedMatch()
    {
        CreatedMatch createdMatch;

        // Implicit conversion from Created<T>
        createdMatch = TypedResults.Created("path");

        // Implicit conversion from MatchErrorResult
        createdMatch = new MatchErrorResult(ResultMessage.Error("x"));

        // Implicit conversion from ResultMessage
        createdMatch = ResultMessage.ApplicationError("x");

        // Implicit conversion from ResultErrors
        var errors = new ResultErrors();
        errors += ResultMessage.Error("x");
        createdMatch = errors;

        return createdMatch;
    }
}
