using FHF.Template.Domain.Entities.ResultPattern;

namespace FHF.Template.Domain.Interfaces.Services;

public interface IWeatherForecastService
{
    Task<Result> Get();
}