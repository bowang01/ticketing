using Microsoft.AspNetCore.Mvc;
using Ticketing.Infrastructure.Persistence;

namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly TicketingDbContext _ticketingDbContext;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, TicketingDbContext ticketingDbContext)
        {
            _logger = logger;
            _ticketingDbContext = ticketingDbContext;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("Ping")]
        [ProducesResponseType(typeof(DbHealthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Ping(CancellationToken cancellationToken)
        {
            bool ok = await _ticketingDbContext.Database.CanConnectAsync(cancellationToken);
            if (!ok) 
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new ProblemDetails
                {
                    Title = "Database unavailable",
                    Status = StatusCodes.Status503ServiceUnavailable,
                    Detail = "Cannot connect to SQL Server."
                });
             }
            return Ok(new DbHealthResponse(Connected: true, CheckedAtUtc: DateTime.UtcNow));
        }

        public record DbHealthResponse(bool Connected, DateTime CheckedAtUtc);
    }
}
