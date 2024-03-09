
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RoyalCode.OperationResults.HttpResults;
using RoyalCode.OperationResults.Interceptors;

namespace RoyalCode.OperationResults.Tests;

public class InterceptorsTest
{
    public InterceptorsTest()
    {
        ErrorResultTypeOptions.SetResultType(ErrorResultTypes.OperationResultAsDefault);
    }

    [Fact]
    public void ErrorsInterceptors()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMatchErrorInterceptor, ErrorInterceptor1>();
        services.AddSingleton<IMatchErrorInterceptor, ErrorInterceptor2>();
        var provider = services.BuildServiceProvider();

        HttpContext httpContext = new DefaultHttpContext()
        {
            RequestServices = provider
        };

        var interceptors = httpContext.RequestServices.GetRequiredService<IEnumerable<IMatchErrorInterceptor>>();
        Assert.Equal(2, interceptors.OfType<IErrorInterceptor>().Count());

        var result = new ResultErrors
        {
            ResultMessage.Error("Error 1"),
            ResultMessage.Error("Error 2")
        };

        var match = new MatchErrorResult(result);
        match.ExecuteAsync(httpContext);

        foreach (var i in interceptors.OfType<IErrorInterceptor>())
        {
            Assert.Single(i.Errors);
        }
    }

    [Fact]
    public void ProblemDetailsInterceptors()
    {
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddLogging();
        services.AddSingleton<IMatchProblemDetailsInterceptor, ProbleDetailsInterceptor1>();
        services.AddSingleton<IMatchProblemDetailsInterceptor, ProbleDetailsInterceptor2>();
        var provider = services.BuildServiceProvider();

        HttpContext httpContext = new DefaultHttpContext()
        {
            RequestServices = provider
        };
        httpContext.Request.Headers.Add(HeaderExtensions.ErrorTypeHeaderName, nameof(ProblemDetails));

        var interceptors = httpContext.RequestServices.GetRequiredService<IEnumerable<IMatchProblemDetailsInterceptor>>();
        Assert.Equal(2, interceptors.OfType<IProblemsInterceptor>().Count());

        var result = new ResultErrors
        {
            ResultMessage.Error("Error 1"),
            ResultMessage.Error("Error 2")
        };

        var match = new MatchErrorResult(result);
        match.ExecuteAsync(httpContext);

        foreach (var i in interceptors.OfType<IProblemsInterceptor>())
        {
            Assert.Single(i.Problems);
        }
    }
}

file interface IErrorInterceptor
{
    ICollection<ResultErrors> Errors { get; }
}

file interface IProblemsInterceptor
{
    ICollection<ProblemDetails> Problems { get; }
}

file class ErrorInterceptor1 : IMatchErrorInterceptor, IErrorInterceptor
{
    public ICollection<ResultErrors> Errors { get; } = new List<ResultErrors>();

    public void WritingResultErrors(ResultErrors errors)
    {
        Errors.Add(errors);
    }
}

file class ErrorInterceptor2 : IMatchErrorInterceptor, IErrorInterceptor
{
    public ICollection<ResultErrors> Errors { get; } = new List<ResultErrors>();

    public void WritingResultErrors(ResultErrors errors)
    {
        Errors.Add(errors);
    }
}

file class ProbleDetailsInterceptor1 : IMatchProblemDetailsInterceptor, IProblemsInterceptor
{
    public ICollection<ProblemDetails> Problems { get; } = new List<ProblemDetails>();

    public void WritingProblemDetails(ProblemDetails problemDetails, ResultErrors errors)
    {
        Problems.Add(problemDetails);
    }
}

file class ProbleDetailsInterceptor2 : IMatchProblemDetailsInterceptor, IProblemsInterceptor
{
    public ICollection<ProblemDetails> Problems { get; } = new List<ProblemDetails>();

    public void WritingProblemDetails(ProblemDetails problemDetails, ResultErrors errors)
    {
        Problems.Add(problemDetails);
    }
}