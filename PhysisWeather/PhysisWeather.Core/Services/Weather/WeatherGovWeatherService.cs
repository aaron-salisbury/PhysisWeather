using PhysisWeather.Core.Base.Extensions;
using PhysisWeather.Core.Base.Helpers;
using PhysisWeather.Core.Data;
using PhysisWeather.Core.Domains;
using PhysisWeather.Core.Domains.WeatherGov.Forecast;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysisWeather.Core.Services
{
    // API Documentation: https://www.weather.gov/documentation/services-web-api
    public class WeatherGovWeatherService : IWeatherService
    {
        private const int ATTEMPT_LIMIT = 3;
        private const string URL_FORMAT_GRIDS = "https://api.weather.gov/points/{0},{1}"; // {0}Latitude, {1}Longitude

        private ILogger _logger { get; set; }
        private int _attemptCounter { get; set; }

        public string UserAgent { get; set; }

        public WeatherGovWeatherService(ILogger logger, string userAgent = null)
        {
            _logger = logger;
            _attemptCounter = 1;

            UserAgent = !string.IsNullOrEmpty(userAgent) ? userAgent : Guid.NewGuid().ToString();
        }

        public async Task<WeatherForecast> GetWeatherDataAsync(Coordinates coordinates)
        {
            try
            {
                _logger.Information("Beginning to retrieve weather forecast.");

                if (coordinates != null)
                {
                    Dictionary<string, string> requestHeaders = new Dictionary<string, string>
                    {
                        { "User-Agent", UserAgent }
                    };

                    string urlGrids = string.Format(URL_FORMAT_GRIDS, coordinates.Latitude, coordinates.Longitude);
                    string jsonGrids = await WebRequests.GetCurlResponseAsync(urlGrids, _logger, requestHeaders);
                    Domains.WeatherGov.Grid.WeatherGovRoot rootGrid = await Json.ToObjectAsync<Domains.WeatherGov.Grid.WeatherGovRoot>(jsonGrids);
                    //TODO: Save coord/grid data so this request can be skipped in the future.

                    if (rootGrid?.Properties != null)
                    {
                        string urlForecast = rootGrid.Properties.Forecast;
                        string jsonForecast = await WebRequests.GetCurlResponseAsync(urlForecast, _logger, requestHeaders);
                        WeatherGovRoot rootForecast = await Json.ToObjectAsync<WeatherGovRoot>(jsonForecast);

                        string urlForecastHourlyURL = rootGrid.Properties.ForecastHourly;
                        string jsonForecastHourly = await WebRequests.GetCurlResponseAsync(urlForecastHourlyURL, _logger, requestHeaders);
                        WeatherGovRoot rootForecastHourly = await Json.ToObjectAsync<WeatherGovRoot>(jsonForecastHourly);

                        if (rootForecast != null && rootForecastHourly != null)
                        {
                            _attemptCounter = 1;
                            return new WeatherForecast
                            {
                                Days = ConvertPeriods(rootForecast?.Properties?.Periods),
                                Hours = ConvertPeriods(rootForecastHourly?.Properties?.Periods, true)
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to retrieve weather forecast: {e.Message}");
                if (_attemptCounter >= ATTEMPT_LIMIT)
                {
                    _attemptCounter = 1;
                    return null;
                }
                // Sometimes the first request fails even when everything is fine.
                _attemptCounter++;
                return await GetWeatherDataAsync(coordinates);
            }

            _logger.Error("Failed to retrieve weather forecast.");
            _attemptCounter = 0;
            return null;
        }

        private List<WeatherPeriod> ConvertPeriods(IEnumerable<WeatherGovPeriod> govPeriods, bool isHours = false)
        {
            if (govPeriods == null)
            {
                return null;
            }

            List<WeatherPeriod> days = new List<WeatherPeriod>();

            DateTime limitData;
            if (isHours)
            {
                // Get one day's worth of hourly weather data.
                DateTime tomorrow = DateTime.Now.Date.AddDays(1);
                limitData = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 59, 59);
            }
            else
            {
                // Get one week's worth of period weather data.
                limitData = DateTime.Now.Date.AddDays(7);
            }

            foreach (WeatherGovPeriod govPeriod in govPeriods.Where(gp => gp.StartTime <= limitData).OrderBy(p => p.Number))
            {
                days.Add(new WeatherPeriod
                {
                    Number = govPeriod.Number,
                    StartTime = govPeriod.StartTime,
                    EndTime = govPeriod.EndTime,
                    Name = govPeriod.Name.ToDisplayName(),
                    ShortDescription = govPeriod.ShortForecast,
                    LongDescription = govPeriod.DetailedForecast,
                    IsDaytime = govPeriod.IsDaytime,
                    WindSpeed = govPeriod.WindSpeed,
                    WindDirection = govPeriod.WindDirection,
                    Temperature = govPeriod.Temperature,
                    IconType = GetIconType(govPeriod.Icon, govPeriod.IsDaytime),
                    TemperatureType = string.Equals(govPeriod.TemperatureUnit.ToUpper(), "F")
                        ? WeatherPeriod.TemperatureTypes.Fahrenheit
                        : WeatherPeriod.TemperatureTypes.Celsius
                });
            }

            return days;
        }

        private WeatherPeriod.IconTypes GetIconType(string govIcon, bool isDaytime)
        {
            // weather.gov provides an "Icon" property like "https://api.weather.gov/icons/land/day/skc?size=medium"
            string broadWeatherGovIconType = govIcon?.GetAfterLastOrEmpty("/").GetUntilOrEmpty("?");

            bool isChance = broadWeatherGovIconType.Contains(",");

            if (isChance)
            {
                broadWeatherGovIconType = broadWeatherGovIconType.GetUntilOrEmpty(",");
            }

            // weather.gov icons: https://www.weather.gov/forecast-icons

            WeatherPeriod.IconTypes iconType;
            switch (broadWeatherGovIconType)
            {
                case "skc":
                    iconType = isDaytime ? WeatherPeriod.IconTypes.AllClear : WeatherPeriod.IconTypes.AllClearNight;
                    break;
                case "few":
                case "sct":
                    iconType = isDaytime ? WeatherPeriod.IconTypes.SomeClouds : WeatherPeriod.IconTypes.SomeCloudsNight;
                    break;
                case "bkn":
                case "ovc":
                    iconType = WeatherPeriod.IconTypes.MostlyClouds;
                    break;
                case "sn":
                case "snow":
                    iconType = isChance ? WeatherPeriod.IconTypes.SnowChance : WeatherPeriod.IconTypes.Snow;
                    break;
                case "ra_sn":
                case "rain":
                    iconType = WeatherPeriod.IconTypes.RainSnow;
                    break;
                case "fzra":
                case "ra_fzra":
                case "fzra_sn":
                    iconType = WeatherPeriod.IconTypes.FreezingRain;
                    break;
                case "ip":
                case "snip":
                    iconType = WeatherPeriod.IconTypes.IcePellets;
                    break;
                case "minus_ra":
                case "ra":
                case "shra":
                case "hi_shwrs":
                case "rain_showers":
                    iconType = isChance ? WeatherPeriod.IconTypes.RainChance : WeatherPeriod.IconTypes.Rain;
                    break;
                case "tsra":
                case "scttsra":
                case "hi_tsra":
                case "tsra_hi":
                    iconType = isChance ? WeatherPeriod.IconTypes.ThunderstormChance : WeatherPeriod.IconTypes.Thunderstorm;
                    break;
                case "fc":
                case "tor":
                    iconType = WeatherPeriod.IconTypes.FunnelCloud_Tornado;
                    break;
                case "hur_warn":
                case "hur_watch":
                case "ts_warn":
                case "ts_watch":
                case "ts_nowarn":
                    iconType = WeatherPeriod.IconTypes.Hurricane_TropicalStorm;
                    break;
                case "wind_skc":
                case "wind_few":
                case "wind_sct":
                case "wind_bkn":
                case "wind_ovc":
                    iconType = WeatherPeriod.IconTypes.Windy;
                    break;
                case "du":
                case "fu":
                case "hz":
                case "fg":
                case "fog":
                    iconType = WeatherPeriod.IconTypes.Dust_Smoke_Haze_Fog;
                    break;
                case "hot":
                    iconType = WeatherPeriod.IconTypes.Hot;
                    break;
                case "cold":
                    iconType = WeatherPeriod.IconTypes.Cold;
                    break;
                case "blizzard":
                    iconType = WeatherPeriod.IconTypes.Blizzard;
                    break;
                default:
                    iconType = WeatherPeriod.IconTypes.NotSet;
                    _logger.Warning($"Was not able to extrapolate icon from URL {govIcon}");
                    break;
            }

            return iconType;
        }
    }
}
