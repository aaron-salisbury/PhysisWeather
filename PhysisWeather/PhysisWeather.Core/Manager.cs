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
        private IWeatherService _weatherService { get; set; }

        private Coordinates _coordinates;
        public Coordinates Coordinates
        {
            get => _coordinates;
            set
            {
                _coordinates = value;
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
            _weatherService = new WeatherGovWeatherService(_logger);
        }

        public bool Forecast()
        {
            WeatherForecast startingWeather = WeatherForecast;

            try
            {
                _logger.Information("Building weather data.");

                if (Coordinates != null)
                {
                    WeatherForecast = Task.Run(() => _weatherService.GetWeatherDataAsync(Coordinates)).Result;
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
