using RoyalCode.OperationResults.Convertion;

namespace RoyalCode.OperationResults.Tests;

public class DescribeGenericErrorsWithAboutBlankTests
{
    [Fact]
    public void ShouldReturnProblemDetailsWithAboutBlank_ForResultMessageError()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(CreateOptions());

        // Assert
        Assert.Equal(ProblemDetailsDescriptor.Types.AboutBlank, problemDetails.Type);
        Assert.Equal(ProblemDetailsDescriptor.Titles.AboutBlankTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
    }

    [Fact]
    public void ShouldReturnProblemDetailsWithAboutBlank_ForResultMessageInvalidParameter()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message", "Property1");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(CreateOptions());

        // Assert
        Assert.Equal(ProblemDetailsDescriptor.Types.AboutBlank, problemDetails.Type);
        Assert.Equal(ProblemDetailsDescriptor.Titles.AboutBlankTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
    }

    [Fact]
    public void ShouldReturnProblemDetailsWithAboutBlank_ForResultMessageValidationError()
    {
        // Arrange
        OperationResult result = ResultMessage.ValidationError("Error message", "Property1");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(CreateOptions());

        // Assert
        Assert.Equal(ProblemDetailsDescriptor.Types.AboutBlank, problemDetails.Type);
        Assert.Equal(ProblemDetailsDescriptor.Titles.AboutBlankTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
    }

    [Fact]
    public void ShouldReturnProblemDetailsWithAboutBlank_ForResultMessageApplicationError()
    {
        // Arrange
        OperationResult result = ResultMessage.ApplicationError(new InvalidOperationException("Error message"));
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(CreateOptions());

        // Assert
        Assert.Equal(ProblemDetailsDescriptor.Types.AboutBlank, problemDetails.Type);
        Assert.Equal(ProblemDetailsDescriptor.Titles.AboutBlankTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.InternalErrorsMessage, problemDetails.Detail);
    }

    private static ProblemDetailsOptions CreateOptions()
    {
        var options = new ProblemDetailsOptions
        {
            HowToDescribeGenericErrors = GenericErrorsDescriptions.AboutBlank,
        };
        options.Complete(default!); // it is safe to pass null here because the method will not use it
        return options;
    }
}
