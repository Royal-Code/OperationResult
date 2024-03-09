using RoyalCode.OperationResults.Convertion;
using System.Dynamic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace RoyalCode.OperationResults.Tests.HttpTests;

/// <summary>
/// Tests of conversion of <see cref="ProblemDetailsExtended"/> to <see cref="OperationResult"/>.
/// </summary>
public class ProblemDetailsExtendedTests
{
    private static readonly JsonTypeInfo<ProblemDetailsExtended>
        JsonType = ProblemDetailsSerializer.DefaultProblemDetailsExtended;

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_SimpleError()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);
        var message = resultErrors[0];
        Assert.Equal("Error message", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message 1");
        result += ResultMessage.Error("Error message 2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);
        
        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_SingleInvalidParameters()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message", "Property");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.Equal("Error message", message.Text);
        Assert.Equal("Property", message.Property);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesInvalidParameters()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message 1", "Property1");
        result += ResultMessage.InvalidParameter("Error message 2", "Property2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesErrorsAndInvalidParameters()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message 1");
        result += ResultMessage.InvalidParameter("Error message 2", "Property2");
        result += ResultMessage.InvalidParameter("Error message 3", "Property3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_SingleNotFound()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Error message", "Property");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.Equal("Error message", message.Text);
        Assert.Equal("Property", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesNotFound()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Error message 1", "Property1");
        result += ResultMessage.NotFound("Error message 2", "Property2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal("Property1", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal("Property2", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesErrorsAndNotFound()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message 1");
        result += ResultMessage.NotFound("Error message 2", "Property2");
        result += ResultMessage.NotFound("Error message 3", "Property3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal("Property2", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal("Property3", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
        
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_SingleValidationError()
    {
        // Arrange
        OperationResult result = ResultMessage.ValidationError("Error message", "Property");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.Equal("Error message", message.Text);
        Assert.Equal("Property", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesValidationErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.ValidationError("Error message 1", "Property1");
        result += ResultMessage.ValidationError("Error message 2", "Property2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal("Property1", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal("Property2", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesValidationErrorsAndInvalidParameters()
    {
        // Arrange
        OperationResult result = ResultMessage.ValidationError("Error message 1", "Property1");
        result += ResultMessage.ValidationError("Error message 2", "Property2");
        result += ResultMessage.InvalidParameter("Error message 3", "Property3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal("Property1", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal("Property2", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal("Property3", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesValidationErrorsAndNotFound()
    {
        // Arrange
        OperationResult result = ResultMessage.ValidationError("Error message 1", "Property1");
        result += ResultMessage.ValidationError("Error message 2", "Property2");
        result += ResultMessage.NotFound("Error message 3", "Property3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal("Property1", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal("Property2", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal("Property3", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesErrorsAndValidationErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.ValidationError("Error message 1", "Property1");
        result += ResultMessage.ValidationError("Error message 2", "Property2");
        result += ResultMessage.Error("Error message 3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal("Property1", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal("Property2", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_SingleApplicationError()
    {
        // Arrange
        OperationResult result = ResultMessage.ApplicationError("Error message");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.Equal("Error message", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesApplicationErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.ApplicationError("Error message 1");
        result += ResultMessage.ApplicationError("Error message 2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesApplicationErrorsAndGenericError()
    {
        // Arrange
        OperationResult result = ResultMessage.ApplicationError("Error message 1");
        result += ResultMessage.ApplicationError("Error message 2");
        result += ResultMessage.Error("Error message 3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[^1];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesApplicationErrorsAndInvalidParameter()
    {
        // Arrange
        OperationResult result = ResultMessage.ApplicationError("Error message 1");
        result += ResultMessage.ApplicationError("Error message 2");
        result += ResultMessage.InvalidParameter("Error message 3", "Property3");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal("Property3", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);
        
        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal(HttpStatusCode.InternalServerError, message.Status);
        Assert.NotNull(message.Code);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_SingleConflict()
    {
        // Arrange
        OperationResult result = ResultMessage.Conflict("conflict-1", "Error message");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.Equal("Error message", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-1", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesConflicts()
    {
        // Arrange
        OperationResult result = ResultMessage.Conflict("conflict-1", "Error message 1");
        result += ResultMessage.Conflict("conflict-2", "Error message 2");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-2", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-1", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-2", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesConflictsAndError()
    {
        // Arrange
        OperationResult result = ResultMessage.Conflict("conflict-1", "Error message 1");
        result += ResultMessage.Conflict("conflict-2", "Error message 2");
        result += ResultMessage.Error("Error message 3");
        result += ResultMessage.Error("Error message 4");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-2", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(4, resultErrors.Count);

        var message = resultErrors[0];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 4", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[2];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-1", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);

        message = resultErrors[^1];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-2", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrors_For_MultiplesConflictsAndAllOthersErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.Conflict("conflict-1", "Error message 1");
        result += ResultMessage.Conflict("conflict-2", "Error message 2");
        result += ResultMessage.Error("Error message 3");
        result += ResultMessage.Error("Error message 4");
        result += ResultMessage.ValidationError("Error message 5", "Property5");
        result += ResultMessage.ValidationError("Error message 6", "Property6");
        result += ResultMessage.InvalidParameter("Error message 7", "Property7");
        result += ResultMessage.InvalidParameter("Error message 8", "Property8");
        result += ResultMessage.NotFound("Error message 9", "Property9");
        result += ResultMessage.NotFound("Error message 10", "Property10");

        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-2", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(10, resultErrors.Count);

        IResultMessage message;

        message = resultErrors[0];
        Assert.Equal("Error message 5", message.Text);
        Assert.Equal("Property5", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[1];
        Assert.Equal("Error message 6", message.Text);
        Assert.Equal("Property6", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[2];
        Assert.Equal("Error message 7", message.Text);
        Assert.Equal("Property7", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[3];
        Assert.Equal("Error message 8", message.Text);
        Assert.Equal("Property8", message.Property);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[4];
        Assert.Equal("Error message 9", message.Text);
        Assert.Equal("Property9", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[5];
        Assert.Equal("Error message 10", message.Text);
        Assert.Equal("Property10", message.Property);
        Assert.Equal(HttpStatusCode.NotFound, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[6];
        Assert.Equal("Error message 3", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[7];
        Assert.Equal("Error message 4", message.Text);
        Assert.Equal(HttpStatusCode.BadRequest, message.Status);
        Assert.NotNull(message.Code);

        message = resultErrors[8];
        Assert.Equal("Error message 1", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-1", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);
        
        message = resultErrors[9];
        Assert.Equal("Error message 2", message.Text);
        Assert.Equal($"{options.BaseAddress}{options.TypeComplement}conflict-2", message.Code);
        Assert.Equal(HttpStatusCode.Conflict, message.Status);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_SingleError()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message")
            .WithInformation("Info1", "Value 1");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);
        
        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesErrors()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message 1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.Error("Error message 2")
            .WithInformation("Info2", "Value 2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);
        
        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesErrorsAndConflict()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message 1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.Error("Error message 2")
            .WithInformation("Info2", "Value 2");
        result += ResultMessage.Conflict("conflict-1", "Error message 3")
            .WithInformation("Info3", "Value 3");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);

        message = resultErrors[2];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 3", message.AdditionalInformation!["Info3"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesErrorsAndConflicts()
    {
        // Arrange
        OperationResult result = ResultMessage.Error("Error message 1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.Error("Error message 2")
            .WithInformation("Info2", "Value 2");
        result += ResultMessage.Conflict("conflict-1", "Error message 3")
            .WithInformation("Info3", "Value 3");
        result += ResultMessage.Conflict("conflict-2", "Error message 4")
            .WithInformation("Info4", "Value 4");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-2", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(4, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);

        message = resultErrors[2];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 3", message.AdditionalInformation!["Info3"]);
        Assert.False(message.AdditionalInformation!.ContainsKey("Info1"));
        Assert.False(message.AdditionalInformation!.ContainsKey("Info2"));
        Assert.False(message.AdditionalInformation!.ContainsKey("Info4"));
        
        message = resultErrors[3];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 4", message.AdditionalInformation!["Info4"]);
        Assert.False(message.AdditionalInformation!.ContainsKey("Info1"));
        Assert.False(message.AdditionalInformation!.ContainsKey("Info2"));
        Assert.False(message.AdditionalInformation!.ContainsKey("Info3"));
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_SingleInvalidParameter()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesInvalidParameters()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.InvalidParameter("Error message 2", "Property2")
            .WithInformation("Info2", "Value 2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesInvalidParametersAndConflict()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.InvalidParameter("Error message 2", "Property2")
            .WithInformation("Info2", "Value 2");
        result += ResultMessage.Conflict("conflict-1", "Error message 3")
            .WithInformation("Info3", "Value 3");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);

        message = resultErrors[2];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 3", message.AdditionalInformation!["Info3"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesInvalidParametersAndConflicts()
    {
        // Arrange
        OperationResult result = ResultMessage.InvalidParameter("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.InvalidParameter("Error message 2", "Property2")
            .WithInformation("Info2", "Value 2");
        result += ResultMessage.Conflict("conflict-1", "Error message 3")
            .WithInformation("Info3", "Value 3");
        result += ResultMessage.Conflict("conflict-2", "Error message 4")
            .WithInformation("Info4", "Value 4");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-2", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(4, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);

        message = resultErrors[2];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 3", message.AdditionalInformation!["Info3"]);

        message = resultErrors[3];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 4", message.AdditionalInformation!["Info4"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_SingleNotFound()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesNotFound()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.NotFound("Error message 2", "Property2")
            .WithInformation("Info2", "Value 2");
        result.TryGetError(out var error);
        var problemDetails = error!.ToProblemDetails(new ProblemDetailsOptions());
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(2, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesNotFoundAndConflict()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.NotFound("Error message 2", "Property2")
            .WithInformation("Info2", "Value 2");
        result += ResultMessage.Conflict("conflict-1", "Error message 3")
            .WithInformation("Info3", "Value 3");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(3, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);

        message = resultErrors[2];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 3", message.AdditionalInformation!["Info3"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesNotFoundAndConflicts()
    {
        // Arrange
        OperationResult result = ResultMessage.NotFound("Error message 1", "Property1")
            .WithInformation("Info1", "Value 1");
        result += ResultMessage.NotFound("Error message 2", "Property2")
            .WithInformation("Info2", "Value 2");
        result += ResultMessage.Conflict("conflict-1", "Error message 3")
            .WithInformation("Info3", "Value 3");
        result += ResultMessage.Conflict("conflict-2", "Error message 4")
            .WithInformation("Info4", "Value 4");
        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-2", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Equal(4, resultErrors.Count);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);

        message = resultErrors[1];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 2", message.AdditionalInformation!["Info2"]);

        message = resultErrors[2];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 3", message.AdditionalInformation!["Info3"]);

        message = resultErrors[3];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 4", message.AdditionalInformation!["Info4"]);
    }

    [Fact]
    public void ToResultErrors_ShouldReturnResultErrorsAndExtensions_For_MultiplesKindsOfObjects()
    {
        // Arrange
        OperationResult result = ResultMessage.Conflict("conflict-1", "Error message")
            .WithInformation("Info1", "Value 1")
            .WithInformation("Info2a", true)
            .WithInformation("Info2b", false)
            .WithInformation("Info3", 1)
            .WithInformation("Info4", 1.1)
            .WithInformation("Info5", new[] { "1", "2" })
            .WithInformation("Info6", new { Name = "Name", Value = "Value" });

        result.TryGetError(out var error);
        var options = new ProblemDetailsOptions();
        options.Descriptor.Add(new ProblemDetailsDescription("conflict-1", "Conflict Error", "A conflict has occurred", HttpStatusCode.Conflict));
        var problemDetails = error!.ToProblemDetails(options);
        var json = JsonSerializer.Serialize(problemDetails, ProblemDetailsSerializer.JsonSerializerOptions);
        var extended = JsonSerializer.Deserialize(json, JsonType)!;

        dynamic expectedObject = new ExpandoObject();
        expectedObject.name = "Name";
        expectedObject.value = "Value";

        // Act
        var resultErrors = extended.ToResultErrors();

        // Assert
        Assert.NotNull(resultErrors);
        Assert.Single(resultErrors);

        var message = resultErrors[0];
        Assert.NotNull(message.AdditionalInformation);
        Assert.Equal("Value 1", message.AdditionalInformation!["Info1"]);
        Assert.Equal(true, message.AdditionalInformation!["Info2a"]);
        Assert.Equal(false, message.AdditionalInformation!["Info2b"]);
        Assert.Equal(1M, message.AdditionalInformation!["Info3"]);
        Assert.Equal(1.1M, message.AdditionalInformation!["Info4"]);
        Assert.Equal(new[] { "1", "2" }, message.AdditionalInformation!["Info5"]);
        Assert.Equivalent(expectedObject, message.AdditionalInformation!["Info6"]);
    }
}
