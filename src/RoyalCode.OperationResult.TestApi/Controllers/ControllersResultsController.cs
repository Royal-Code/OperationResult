using Microsoft.AspNetCore.Mvc;
using RoyalCode.OperationResults.TestApi.Application.ResultsModels;

namespace RoyalCode.OperationResults.TestApi.Controllers;

[ApiController]
[Route("[controller]/[Action]")]
public class ControllersResultsController : ControllerBase
{

    [HttpGet]
    public IActionResult GetSimpleValues()
    {
        // cria novo resultado de sucesso a partir do resultado.
        OperationResult<SimpleValues> result = new SimpleValues();

        return result.ToActionResult();
    }

    [HttpGet]
    public IActionResult GetSimpleValuesWithCreatedPath()
    {
        // cria novo resultado de sucesso a partir do resultado.
        OperationResult<SimpleValues> result = new SimpleValues();

        return result.ToActionResult("/simple-values");
    }

    [HttpGet]
    public IActionResult GetSimpleValuesWithError()
    {
        // cria novo resultado de erro a partir do resultado.
        OperationResult<SimpleValues> result = ResultMessage.Error("Erro ao obter valores simples.");

        return result.ToActionResult();
    }

    [HttpGet]
    public ActionResult GetSimpleValuesWithErrorWithCreatedPath()
    {
        // cria novo resultado de erro a partir do resultado.
        OperationResult<SimpleValues> result = ResultMessage.Error("Erro ao obter valores simples.");

        return result.ToActionResult("/simple-values");
    }

    [HttpGet]
    public IActionResult GetSimpleValuesWithCreatedPathAndFormat()
    {
        // cria novo resultado de sucesso a partir do resultado.
        OperationResult<SimpleValues> simpleResult = new SimpleValues();

        var result = simpleResult.Convert(v => v.Number);

        return result.ToActionResult("/simple-values/{0}", true);
    }

    [HttpGet]
    public IActionResult GetSimpleValuesWithCreatedPathProvider()
    {
        // cria novo resultado de sucesso a partir do resultado.
        OperationResult<SimpleValues> simpleResult = new SimpleValues();

        return simpleResult.ToActionResult(value => $"/simple-values/{value.Number}");
    }

    [HttpGet]
    public IActionResult GetSimpleValuesWithErrorWithCreatedPathAndFormat()
    {
        // cria novo resultado de erro a partir do resultado.
        OperationResult<SimpleValues> simpleResult = ResultMessage.Error("Erro ao obter valores simples.");

        var result = simpleResult.Convert(v => v.Number);

        return result.ToActionResult("/simple-values/{0}", true);
    }

    [HttpGet]
    public IActionResult GetSimpleValuesIfValidInput([FromQuery] string? error)
    {
        ValidableResult result = new();

        if (!string.IsNullOrEmpty(error))
        {
            result += ResultMessage.InvalidParameter("Parâmetro inválido.", nameof(error));
        }

        var simpleResult = result.Convert(() => new SimpleValues());

        return simpleResult.ToActionResult();
    }

    [HttpGet]
    public IActionResult ValidableResult([FromQuery] string? input)
    {
        ValidableResult result = new();

        if (string.IsNullOrWhiteSpace(input))
        {
            result += ResultMessage.InvalidParameter("Input inválido.", nameof(input));
        }

        return result.ToActionResult();
    }

    [HttpGet]
    public IActionResult GetSimpleValuesWithException()
    {
        var Exception = new Exception("Erro ao obter valores simples.");
        OperationResult<SimpleValues> result = ResultMessage.Error(Exception);
        return result.ToActionResult();
    }
}
