using PhysisWeather.Core.Base.Extensions;
using PhysisWeather.Core.Base.Helpers;
using PhysisWeather.Core.Data;
using PhysisWeather.Core.Domains;
using Serilog;
using System;
using System.Threading.Tasks;

namespace PhysisWeather.Core.Services
{
    // API Documentation: http://ziptasticapi.com/
    public class ZiptasticCityService : ICityService
    {
        private const string URL_FORMAT = "http://ziptasticapi.com/{0}"; // {0}Zip

        private ILogger _logger { get; set; }

        public ZiptasticCityService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<CityData> GetCityDataAsync(string zip)
        {
            try
            {
                _logger.Information("Beginning to retrieve city data.");

                if (!string.IsNullOrEmpty(zip))
                {
                    string url = string.Format(URL_FORMAT, zip);
                    string json = await WebRequests.GetCurlResponseAsync(url, _logger);
                    ZiptasticRoot root = await Json.ToObjectAsync<ZiptasticRoot>(json);

                    if (root != null)
                    {
                        return new CityData
                        {
                            City = root.City?.ToDisplayName(),
                            State = root.State,
                            ZipCode = zip,
                            Country = root.Country
                        };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to retrieve city data: {e.Message}");
                return null;
            }

            _logger.Error("Failed to retrieve city data.");
            return null;
        }
    }
}
