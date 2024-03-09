using RoyalCode.OperationResults;

namespace RoyalCode.OperationResults.TestApi.Application.WeatherForecasts;

public class WeatherForecastService
{
    private static readonly List<string> Summaries = new()
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    public OperationResult<IEnumerable<WeatherForecast>> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Count)]
        })
        .ToList();
    }
}
