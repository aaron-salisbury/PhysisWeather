using PhysisWeather.Core.Base;
using PhysisWeather.Core.Domains;
using PhysisWeather.Core.Services;
using Serilog;
using System;
using System.Threading.Tasks;

namespace PhysisWeather.Core
{
    public class Manager : ObservableObject
    {
        private ILogger _logger { get; set; }
        private ICoordinateService _coordinateService { get; set; }
        private ICityService _cityService { get; set; }
        private IWeatherService _weatherService { get; set; }

        private DemographicData _demographicData;
        public DemographicData DemographicData
        {
            get => _demographicData;
            set
            {
                _demographicData = value;
                RaisePropertyChanged();
            }
        }

        private string _searchZip;
        public string SearchZip
        {
            get => _searchZip;
            set
            {
                _searchZip = value;
                RaisePropertyChanged();
            }
        }

        private WeatherForecast _weatherForecast;
        public WeatherForecast WeatherForecast
        {
            get => _weatherForecast;
            set
            {
                _weatherForecast = value;
                RaisePropertyChanged();
            }
        }

        public Manager(AppLogger appLogger)
        {
            _logger = appLogger.Logger;
            _coordinateService = new FrostlineCoordinateService(_logger);
            _cityService = new ZiptasticCityService(_logger);
            _weatherService = new WeatherGovWeatherService(_logger);
        }

        public bool BuildDemographicData()
        {
            if (string.IsNullOrEmpty(SearchZip))
            {
                _logger.Warning("Cannot build demographic data without a zip code.");
                return false;
            }

            Coordinates coordinates = null;
            try
            {
                _logger.Information("Building coordinates.");

                coordinates = Task.Run(() => _coordinateService.GetCoordinatesAsync(SearchZip)).Result;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to build coordinates: {e.Message}");
                return false;
            }

            CityData cityData = null;
            try
            {
                _logger.Information("Building city data.");

                cityData = Task.Run(() => _cityService.GetCityDataAsync(SearchZip)).Result;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to build city data: {e.Message}");
                return false;
            }

            DemographicData = new DemographicData
            {
                Coordinates = coordinates,
                CityData = cityData
            };

            return true;
        }

        public bool Forecast()
        {
            WeatherForecast startingWeather = WeatherForecast;

            try
            {
                _logger.Information("Building weather data.");

                if (DemographicData?.Coordinates != null)
                {
                    WeatherForecast = Task.Run(() => _weatherService.GetWeatherDataAsync(DemographicData.Coordinates)).Result;
                }
            }
            catch (Exception e)
            {
                WeatherForecast = startingWeather;

                _logger.Error($"Failed to build weather data: {e.Message}");
                return false;
            }

            if (WeatherForecast != null)
            {
                return true;
            }
            else
            {
                WeatherForecast = startingWeather;

                _logger.Error("Failed to build weather data.");
                return false;
            }
        }
    }
}
