using RoyalCode.OperationResults.TestApi.Application.SeedWork;

namespace RoyalCode.OperationResults.TestApi.Application.WeatherForecasts
{
    public class WeatherForecast : IHasId<Guid>
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }
}