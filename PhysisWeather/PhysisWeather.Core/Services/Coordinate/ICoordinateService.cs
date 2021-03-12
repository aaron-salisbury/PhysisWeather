using PhysisWeather.Core.Domains;
using System.Threading.Tasks;

namespace PhysisWeather.Core.Services
{
    public interface ICoordinateService
    {
        Task<Coordinates> GetCoordinatesAsync(string zip);
    }
}
