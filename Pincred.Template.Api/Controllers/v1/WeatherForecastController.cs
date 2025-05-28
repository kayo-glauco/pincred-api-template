using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pincred.Template.Domain;
using Pincred.Template.Domain.Entities.ResultPattern;
using Pincred.Template.Domain.Interfaces.Services;

namespace Pincred.Template.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<Result> Get(
        [FromServices] IWeatherForecastService weatherForecastService        
        )
    {
        return await weatherForecastService.Get();
    }
}