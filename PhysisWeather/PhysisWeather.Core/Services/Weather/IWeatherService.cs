using PhysisWeather.Core.Domains;
using System.Threading.Tasks;

namespace PhysisWeather.Core.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherDataAsync(Coordinates coordinates);
    }
}
