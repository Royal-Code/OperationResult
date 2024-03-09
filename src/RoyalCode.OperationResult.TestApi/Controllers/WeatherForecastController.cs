using Microsoft.AspNetCore.Mvc;
using RoyalCode.OperationResults.TestApi.Application.WeatherForecasts;

namespace RoyalCode.OperationResults.TestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly WeatherForecastService service;

    public WeatherForecastController(WeatherForecastService service)
    {
        this.service = service;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        return service.Get().ToActionResult();
    }
}