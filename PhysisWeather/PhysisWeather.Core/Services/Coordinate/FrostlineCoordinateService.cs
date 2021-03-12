using PhysisWeather.Core.Base.Helpers;
using PhysisWeather.Core.Data;
using PhysisWeather.Core.Domains;
using Serilog;
using System;
using System.Threading.Tasks;

namespace PhysisWeather.Core.Services
{
    // API Documentation: https://github.com/waldoj/frostline
    public class FrostlineCoordinateService : ICoordinateService
    {
        private const string URL_FORMAT = "https://phzmapi.org/{0}.json"; // {0}Zip

        private ILogger _logger { get; set; }

        public FrostlineCoordinateService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Coordinates> GetCoordinatesAsync(string zip)
        {
            try
            {
                _logger.Information("Beginning to retrieve coordinates.");

                if (!string.IsNullOrEmpty(zip))
                {
                    string url = string.Format(URL_FORMAT, zip);
                    string json = await WebRequests.GetCurlResponseAsync(url, _logger);
                    FrostLineRoot root = await Json.ToObjectAsync<FrostLineRoot>(json);

                    if (root != null && root.Coordinates != null)
                    {
                        return new Coordinates
                        {
                            Longitude = root.Coordinates.Lon.ToString(),
                            Latitude = root.Coordinates.Lat.ToString()
                        };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to retrieve coordinates: {e.Message}");
                return null;
            }

            _logger.Error("Failed to retrieve coordinates.");
            return null;
        }
    }
}
