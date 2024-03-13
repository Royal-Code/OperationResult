using Microsoft.AspNetCore.Mvc;

namespace RoyalCode.OperationResults.TestApi.Controllers;

[ApiController]
[Route("[controller]/[Action]")]
public class HttpTestsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTextBadRequest()
    {
        return BadRequest("Erro ao obter valores simples.");
    }
}
