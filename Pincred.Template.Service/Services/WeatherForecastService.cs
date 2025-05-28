using Pincred.Template.Domain;
using Pincred.Template.Domain.Attributes;
using Pincred.Template.Domain.Entities.ResultPattern;
using Pincred.Template.Domain.Interfaces.Repositories.Base;
using Pincred.Template.Domain.Interfaces.Services;

namespace Pincred.Template.Service.Services;

[Service]
public sealed class WeatherForecastService(IUnitOfWork unitOfWork) : IWeatherForecastService
{
    public async Task<Result> Get()
    {
        var summaries = await unitOfWork.Summaries.GetAllAsync();
        if (!summaries.Any())
        {
            return Result.NoContent();
        }

        var descriptions = summaries.Select(s => s.Description).ToList();
        var data = Enumerable
                        .Range(1, 5)
                        .Select(index => new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = descriptions[Random.Shared.Next(summaries.Count)]
                        })
                        .ToList();

        return Result.Ok(data);
    }
}