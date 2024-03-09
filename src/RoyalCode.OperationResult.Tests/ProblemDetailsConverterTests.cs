using Microsoft.AspNetCore.Mvc;
using RoyalCode.OperationResults.Convertion;
using System.Net;

namespace RoyalCode.OperationResults.Tests;

public class ProblemDetailsConverterTests
{
    public ProblemDetailsConverterTests()
    {
        ExceptionsParsers.UseGenericMessageForExceptions = false;
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_Status400_When_Error_WithoutStatus_And_WithoutCode_And_WithoutProperty()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.GenericErrorType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.GenericErrorTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_Status400_When_Error_WithoutStatus_And_WithoutCode_And_WithProperty()
    {
        // Arrange
        OperationResult result = ResultMessage.Error(code: null, "Error message", "Property");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.InvalidParametersType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.InvalidParametersTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_Status400_When_Error_WithoutStatus_And_WithCode()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error Code", "Error message");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_DefinedStatus_And_DefaultDescriptions_When_Error_WithStatus400()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message", HttpStatusCode.BadRequest);
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.InvalidParametersType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.InvalidParametersTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_DefinedStatus_And_DefaultDescriptions_When_Error_WithStatus404()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message", HttpStatusCode.NotFound);
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.NotFoundType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.NotFoundTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(404, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_DefinedStatus_And_DefaultDescriptions_When_Error_WithStatus422()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message", HttpStatusCode.UnprocessableEntity);
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.ValidationType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.ValidationTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(422, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_DefinedStatus_And_DefaultDescriptions_When_Error_WithStatus500()
    {
        // Arrange
        OperationResult result = ResultMessage.Error(new ArgumentException("Error message"));
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.ApplicationErrorType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.ApplicationErrorTitle, problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(500, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_CodeAsTypeAndTitle_When_DescriptionIsNotDefined()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("error-code", "Error message", HttpStatusCode.Conflict);
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();

        // Act
        var problemDetails = error!.ToProblemDetails(options);

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}error-code", problemDetails.Type);
        Assert.Equal("error-code", problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(409, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_CodeAsType_And_DescriptionTitle()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("error-code", "Error message", HttpStatusCode.Conflict);
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("error-code", "Error Title", "Error type description"));

        // Act
        var problemDetails = error!.ToProblemDetails(options);

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}error-code", problemDetails.Type);
        Assert.Equal("Error Title", problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(409, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_DescriptionTypeAndTitle()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("error-code", "Error message", HttpStatusCode.Conflict);
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("error-code", "http://errors.com/error-type", "Error Title", "Error type description"));

        // Act
        var problemDetails = error!.ToProblemDetails(options);

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal("http://errors.com/error-type", problemDetails.Type);
        Assert.Equal("Error Title", problemDetails.Title);
        Assert.Equal("Error message", problemDetails.Detail);
        Assert.Equal(409, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_ReturnProblemDetails_With_DescriptionStatus()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("error-code", "Error message", HttpStatusCode.BadRequest);
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("error-code", "Error Title", "Error type description", HttpStatusCode.Conflict));

        // Act
        var problemDetails = error!.ToProblemDetails(options);

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(409, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveInvalidParamsExtraField_When_HaveManyInvalidParametersErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Mensagen 1", "FieldA");
        result += ResultMessage.InvalidParameter("Mensagen 2", "FieldB");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.InvalidParametersType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.InvalidParametersTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.InvalidParametersMessage, problemDetails.Detail);

        var extraFields = problemDetails.Extensions[ProblemDetailsExtended.Fields.InvalidParametersExtensionField] as List<InvalidParameterDetails>;
        Assert.NotNull(extraFields);
        Assert.Equal(2, extraFields.Count);

        Assert.Equal("FieldA", extraFields[0].Name);
        Assert.Equal("Mensagen 1", extraFields[0].Reason);

        Assert.Equal("FieldB", extraFields[1].Name);
        Assert.Equal("Mensagen 2", extraFields[1].Reason);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveNotFoundExtraField_When_HaveManyNotFoundErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Mensagen 1", "FieldA");
        result += ResultMessage.NotFound("Mensagen 2", "FieldB");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.NotFoundType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.NotFoundTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.NotFoundMessage, problemDetails.Detail);

        var extraFields = problemDetails.Extensions[ProblemDetailsExtended.Fields.NotFoundExtensionField] as List<NotFoundDetails>;
        Assert.NotNull(extraFields);
        Assert.Equal(2, extraFields.Count);

        Assert.Equal("FieldA", extraFields[0].Property);
        Assert.Equal("Mensagen 1", extraFields[0].Message);

        Assert.Equal("FieldB", extraFields[1].Property);
        Assert.Equal("Mensagen 2", extraFields[1].Message);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveErrorsExtensionField_When_HaveManyApplicationErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.ApplicationError(new Exception("Mensagen 1"));
        result += ResultMessage.ApplicationError(new Exception("Mensagen 2"));
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(ProblemDetailsDescriptor.Types.ApplicationErrorType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.ApplicationErrorTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.InternalErrorsMessage, problemDetails.Detail);

        var extraFields = problemDetails.Extensions[ProblemDetailsExtended.Fields.ErrorsExtensionField] as List<ErrorDetails>;
        Assert.NotNull(extraFields);
        Assert.Equal(2, extraFields.Count);

        Assert.Equal("Mensagen 1", extraFields[0]);
        Assert.Equal("Mensagen 2", extraFields[1]);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveInnerDetailsExtensionField_When_HaveManyCustomErrors()
    {
        //arrange
        OperationResult result = ResultMessage.Error("error-code-1", "Error message 1", HttpStatusCode.Conflict);
        result += ResultMessage.Error("error-code-2", "Error message 2", HttpStatusCode.Conflict);
        result.TryGetError(out var error);

        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("error-code-1", "Error Title", "Error type description 1"));
        options.Descriptor.Add(new ProblemDetailsDescription("error-code-2", "Error Title", "Error type description 2"));

        //act
        var problemDetails = error!.ToProblemDetails(options);

        //assert
        Assert.NotNull(problemDetails);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}{ProblemDetailsDescriptor.Codes.AggregateProblemsDetails}", problemDetails.Type);
        Assert.Equal(ProblemDetailsDescriptor.Titles.AggregateProblemsDetailsTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.AggregateMessage, problemDetails.Detail);

        var extraFields = problemDetails.Extensions[ProblemDetailsExtended.Fields.AggregateExtensionField] as List<ProblemDetails>;
        Assert.NotNull(extraFields);
        Assert.Equal(2, extraFields.Count);

        var problemDetails1 = extraFields[0];
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}error-code-1", problemDetails1.Type);
        Assert.Equal("Error Title", problemDetails1.Title);
        Assert.Equal("Error message 1", problemDetails1.Detail);
        Assert.Equal(409, problemDetails1.Status);

        var problemDetails2 = extraFields[1];
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}error-code-2", problemDetails2.Type);
        Assert.Equal("Error Title", problemDetails2.Title);
        Assert.Equal("Error message 2", problemDetails2.Detail);
        Assert.Equal(409, problemDetails2.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveStatus_400_Over_404_WhenHaveMultiplesErrorsTypes()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Mensagen 1", "FieldA");
        result += ResultMessage.InvalidParameter("Mensagen 2", "FieldB");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Equal(ProblemDetailsDescriptor.Types.InvalidParametersType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.InvalidParametersTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.InvalidParametersMessage, problemDetails.Detail);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveStatus_422_Over_404_And_400_WhenHaveMultiplesErrorsTypes()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Mensagen 1", "FieldA");
        result += ResultMessage.InvalidParameter("Mensagen 2", "FieldB");
        result += ResultMessage.ValidationError("Mensagen 3", "FieldC");
        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(422, problemDetails.Status);
        Assert.Equal(ProblemDetailsDescriptor.Types.ValidationType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.ValidationTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.InvalidParametersMessage, problemDetails.Detail);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveStatus_Custom_OverOthers_WhenHaveMultiplesErrorsTypes()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Mensagen 1", "FieldA");
        result += ResultMessage.InvalidParameter("Mensagen 2", "FieldB");
        result += ResultMessage.ValidationError("Mensagen 3", "FieldC");
        result += ResultMessage.Error("error-code-4", "Error message 4", HttpStatusCode.Conflict);
        result += ResultMessage.ApplicationError(new Exception("Mensagen 5"));
        result.TryGetError(out var error);

        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("error-code-4", "Error Title", "Error type description"));

        // Act
        var problemDetails = error!.ToProblemDetails(options);

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}error-code-4", problemDetails.Type);
        Assert.Equal("Error Title", problemDetails.Title);
        Assert.Equal("Error message 4", problemDetails.Detail);
        Assert.Equal(409, problemDetails.Status);
    }

    [Fact]
    public void ToProblemDetails_Should_HaveStatus_500_OverOthersGenerics_WhenHaveMultiplesErrorsTypes()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Mensagen 1", "FieldA");
        result += ResultMessage.InvalidParameter("Mensagen 2", "FieldB");
        result += ResultMessage.ValidationError("Mensagen 3", "FieldC");
        result += ResultMessage.ApplicationError(new Exception("Mensagen 4"));

        result.TryGetError(out var error);

        // Act
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(500, problemDetails.Status);
        Assert.Equal(ProblemDetailsDescriptor.Types.ApplicationErrorType, problemDetails.Type);
        Assert.Equal(ProblemDetailsExtended.Titles.ApplicationErrorTitle, problemDetails.Title);
        Assert.Equal(ProblemDetailsDescriptor.Messages.InternalErrorsMessage, problemDetails.Detail);
    }

    [Fact]
    public void ToProblemDetails_Should_AddExtraFieldsInInvalidParameterDetails_When_HaveManyInvalidParameters()
    {
        //arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message 1", "Property1")
            .WithInformation("info", "info 1");
        result += ResultMessage.InvalidParameter("Error message 2", "Property2")
            .WithInformation("info", "info 2");
        result.TryGetError(out var error);

        var options = new ProblemDetailsOptions();

        //act
        var problemDetails = error!.ToProblemDetails(options);

        //assert
        Assert.NotNull(problemDetails);

        var extraFields = problemDetails.Extensions[ProblemDetailsExtended.Fields.InvalidParametersExtensionField] as List<InvalidParameterDetails>;
        Assert.NotNull(extraFields);
        Assert.Equal(2, extraFields.Count);

        var details1 = extraFields[0];
        Assert.Equal("Property1", details1.Name);
        Assert.Equal("Error message 1", details1.Reason);
        Assert.NotNull(details1.Extensions);
        Assert.Equal("info 1", details1.Extensions["info"]);

        var details2 = extraFields[1];
        Assert.Equal("Property2", details2.Name);
        Assert.Equal("Error message 2", details2.Reason);
        Assert.NotNull(details2.Extensions);
        Assert.Equal("info 2", details2.Extensions["info"]);
    }

    [Fact]
    public void ToProblemDetails_Should_AddExtraFieldsInNotFoundDetails_When_HavanManyNotFound()
    {
        //arrange
        OperationResult result = ResultMessage.NotFound("Error message 1", "Property1")
            .WithInformation("info", "info 1");
        result += ResultMessage.NotFound("Error message 2", "Property2")
            .WithInformation("info", "info 2");
        result.TryGetError(out var error);

        var options = new ProblemDetailsOptions();

        //act
        var problemDetails = error!.ToProblemDetails(options);

        //assert
        Assert.NotNull(problemDetails);

        var extraFields = problemDetails.Extensions[ProblemDetailsExtended.Fields.NotFoundExtensionField] as List<NotFoundDetails>;

        Assert.NotNull(extraFields);
        Assert.Equal(2, extraFields.Count);

        var details1 = extraFields[0];
        Assert.Equal("Property1", details1.Property);
        Assert.Equal("Error message 1", details1.Message);
        Assert.NotNull(details1.Extensions);
        Assert.Equal("info 1", details1.Extensions["info"]);

        var details2 = extraFields[1];
        Assert.Equal("Property2", details2.Property);
        Assert.Equal("Error message 2", details2.Message);
        Assert.NotNull(details2.Extensions);
        Assert.Equal("info 2", details2.Extensions["info"]);
    }
}
