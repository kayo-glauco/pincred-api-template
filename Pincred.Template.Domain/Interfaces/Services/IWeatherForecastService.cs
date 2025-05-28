using Pincred.Template.Domain.Entities.ResultPattern;

namespace Pincred.Template.Domain.Interfaces.Services;

public interface IWeatherForecastService
{
    Task<Result> Get();
}