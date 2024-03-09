
using Microsoft.AspNetCore.Builder;
using RoyalCode.OperationResults.HttpResults;

namespace RoyalCode.OperationResults.TestApi.Apis;

public static class ErrorsSamples
{
    public static void MapErrors(this WebApplication app)
    {
        var group = app.MapGroup("/api/erros")
            .WithOpenApi()
            .WithDescription("Exemplos de erros.");



        group.MapGet("SingleError", SingleError)
            .WithName("SingleError")
            .WithDescription("Obtém um erro de BadRequest.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("SingleInvalidParameters", SingleInvalidParameters)
            .WithName("SingleInvalidParameters")
            .WithDescription("Obtém um erro de BadRequest.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("SingleNotFound", SingleNotFound)
            .WithName("SingleNotFound")
            .WithDescription("Obtém um erro de NotFound.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("SingleValidationError", SingleValidationError)
            .WithName("SingleValidationError")
            .WithDescription("Obtém um erro de ValidationError.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();



        group.MapGet("MultipleErrors", MultipleGenericErrors)
            .WithName("MultipleErrors")
            .WithDescription("Obtém um erro de BadRequest com vários erros.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("MultipleInvalidParameters", MultipleInvalidParameters)
            .WithName("MultipleInvalidParameters")
            .WithDescription("Obtém um erro de BadRequest com vários erros de parâmetros inválidos.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("MultipleNotFound", MultipleNotFound)
            .WithName("MultipleNotFound")
            .WithDescription("Obtém um erro de NotFound com vários erros.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("MultipleValidationError", MultipleValidationError)
            .WithName("MultipleValidationError")
            .WithDescription("Obtém um erro de ValidationError com vários erros.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();



        group.MapGet("SingleCustomError", SingleCustomError)
            .WithName("SingleCustomError")
            .WithDescription("Obtém um erro customizado.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("MultipleCustomError", MultipleCustomError)
            .WithName("MultipleCustomError")
            .WithDescription("Obtém um erro customizado com vários erros.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();



        group.MapGet("CustomErrorAndGenericErrors", CustomErrorAndGenericErrors)
            .WithName("CustomErrorAndGenericErrors")
            .WithDescription("Obtém um erro customizado com vários erros genéricos.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("CustomErrorAndInvalidParameters", CustomErrorAndInvalidParameters)
            .WithName("CustomErrorAndInvalidParameters")
            .WithDescription("Obtém um erro customizado com vários erros de parâmetros inválidos.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("CustomErrorAndNotFound", CustomErrorAndNotFound)
            .WithName("CustomErrorAndNotFound")
            .WithDescription("Obtém um erro customizado com vários erros de NotFound.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("CustomErrorAndValidationError", CustomErrorAndValidationError)
            .WithName("CustomErrorAndValidationError")
            .WithDescription("Obtém um erro customizado com vários erros de ValidationError.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("CustomErrorAndAllGenericErrors", CustomErrorAndAllGenericErrors)
            .WithName("CustomErrorAndAllGenericErrors")
            .WithDescription("Obtém um erro customizado com todos os tipos de erros genéricos.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();

        group.MapGet("MultipleCustomErrorAndAllGenericErrors", MultipleCustomErrorAndAllGenericErrors)
            .WithName("MultipleCustomErrorAndAllGenericErrors")
            .WithDescription("Obtém um erro customizado com vários erros e todos os tipos de erros genéricos.")
            .WithOpenApi()
            .WithErrorResponseTypeHeader();
    }

    private static OkMatch SingleError()
    {
        return ResultMessage.Error("invalid input");
    }

    private static OkMatch SingleInvalidParameters()
    {
        return ResultMessage.InvalidParameter("invalid input", "prop");
    }

    private static OkMatch SingleNotFound()
    {
        return ResultMessage.NotFound("not found", null);
    }

    private static OkMatch SingleValidationError()
    {
        return ResultMessage.ValidationError("invalid input", "prop");
    }

    private static OkMatch MultipleGenericErrors()
    {
        ResultErrors error = ResultMessage.Error("invalid input1");
        error += ResultMessage.Error("invalid input2");
        error += ResultMessage.Error("invalid input3");

        return error;
    }

    private static OkMatch MultipleInvalidParameters()
    {
        ResultErrors error = ResultMessage.InvalidParameter("invalid input1", "prop1");
        error += ResultMessage.InvalidParameter("invalid input2", "prop2");
        error += ResultMessage.InvalidParameter("invalid input3", "prop3");

        return error;
    }

    private static OkMatch MultipleNotFound()
    {
        ResultErrors error = ResultMessage.NotFound("not found1", "prop1");
        error += ResultMessage.NotFound("not found2", "prop2");
        error += ResultMessage.NotFound("not found3", "prop3");

        return error;
    }

    private static OkMatch MultipleValidationError()
    {
        ResultErrors error = ResultMessage.ValidationError("invalid input1", "prop1");
        error += ResultMessage.ValidationError("invalid input2", "prop2");
        error += ResultMessage.ValidationError("invalid input3", "prop3");

        return error;
    }

    private static OkMatch SingleCustomError()
    {
        return ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");
    }

    private static OkMatch MultipleCustomError()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.ValidationError("size-out-of-bounds", "The items of the collection are out of bounds.", null, null)
            .WithInformation("parcelId", "123456789")
            .WithInformation("CollectionId", "123456789")
            .WithInformation("size", "100")
            .WithInformation("minSize", "10")
            .WithInformation("maxSize", "50");

        error += ResultMessage.NotFound("dependencies-not-found", "The dependencies of the operation were not found.", null)
            .WithInformation("operationId", "123456789")
            .WithInformation("dependencies", "123456789, 123456789, 123456789");

        return error;
    }

    private static OkMatch CustomErrorAndGenericErrors()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.Error("invalid input1");
        error += ResultMessage.Error("invalid input2");
        error += ResultMessage.Error("invalid input3");

        return error;
    }

    private static OkMatch CustomErrorAndInvalidParameters()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.InvalidParameter("invalid input1", "prop1");
        error += ResultMessage.InvalidParameter("invalid input2", "prop2");
        error += ResultMessage.InvalidParameter("invalid input3", "prop3");

        return error;
    }

    private static OkMatch CustomErrorAndNotFound()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.NotFound("not found1", "prop1");
        error += ResultMessage.NotFound("not found2", "prop2");
        error += ResultMessage.NotFound("not found3", "prop3");

        return error;
    }

    private static OkMatch CustomErrorAndValidationError()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.ValidationError("invalid input1", "prop1");
        error += ResultMessage.ValidationError("invalid input2", "prop2");
        error += ResultMessage.ValidationError("invalid input3", "prop3");

        return error;
    }

    private static OkMatch CustomErrorAndAllGenericErrors()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.Error("invalid input1");
        error += ResultMessage.Error("invalid input2");
        error += ResultMessage.Error("invalid input3");

        error += ResultMessage.InvalidParameter("invalid input1", "prop1");
        error += ResultMessage.InvalidParameter("invalid input2", "prop2");
        error += ResultMessage.InvalidParameter("invalid input3", "prop3");

        error += ResultMessage.NotFound("not found1", "prop1");
        error += ResultMessage.NotFound("not found2", "prop2");
        error += ResultMessage.NotFound("not found3", "prop3");

        error += ResultMessage.ValidationError("invalid input1", "prop1");
        error += ResultMessage.ValidationError("invalid input2", "prop2");
        error += ResultMessage.ValidationError("invalid input3", "prop3");

        return error;
    }

    private static OkMatch MultipleCustomErrorAndAllGenericErrors()
    {
        ResultErrors error = ResultMessage.Conflict("insufficient-credits", "The partner does not have enough credits to perform the operation.")
            .WithInformation("operationId", "123456789")
            .WithInformation("partnerId", "123456789")
            .WithInformation("credits", "100")
            .WithInformation("requiredCredits", "200");

        error += ResultMessage.ValidationError("size-out-of-bounds", "The items of the collection are out of bounds.", null, null)
            .WithInformation("parcelId", "123456789")
            .WithInformation("CollectionId", "123456789")
            .WithInformation("size", "100")
            .WithInformation("minSize", "10")
            .WithInformation("maxSize", "50");

        error += ResultMessage.NotFound("dependencies-not-found", "The dependencies of the operation were not found.", null)
            .WithInformation("operationId", "123456789")
            .WithInformation("dependencies", "123456789, 123456789, 123456789");

        error += ResultMessage.Error("invalid input1");
        error += ResultMessage.Error("invalid input2");
        error += ResultMessage.Error("invalid input3");

        error += ResultMessage.InvalidParameter("invalid input1", "prop1");
        error += ResultMessage.InvalidParameter("invalid input2", "prop2");
        error += ResultMessage.InvalidParameter("invalid input3", "prop3");

        error += ResultMessage.NotFound("not found1", "prop1");
        error += ResultMessage.NotFound("not found2", "prop2");
        error += ResultMessage.NotFound("not found3", "prop3");

        error += ResultMessage.ValidationError("invalid input1", "prop1");
        error += ResultMessage.ValidationError("invalid input2", "prop2");
        error += ResultMessage.ValidationError("invalid input3", "prop3");

        return error;
    }
}