using PhysisWeather.Core.Domains;
using System.Threading.Tasks;

namespace PhysisWeather.Core.Services
{
    public interface ICityService
    {
        Task<CityData> GetCityDataAsync(string zip);
    }
}
