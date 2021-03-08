using Newtonsoft.Json;
using System;

namespace PhysisWeather.Core.Domains.WeatherGov.Grid
{
    [Serializable]
    public class WeatherGovProperties
    {
        public string City { get; set; }
        public string State { get; set; }
        public string CWA { get; set; }
        public string ForecastOffice { get; set; }
        public string GridId { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        public string Forecast { get; set; }
        public string ForecastHourly { get; set; }
        public string ForecastGridData { get; set; }
        public string ObservationStations { get; set; }
        public string ForecastZone { get; set; }
        public string County { get; set; }
        public string FireWeatherZone { get; set; }
        public string TimeZone { get; set; }
        public string RadarStation { get; set; }
        public WeatherGovDistance Distance { get; set; }
        public WeatherGovBearing Bearing { get; set; }
        public WeatherGovRelativeLocation RelativeLocation { get; set; }

        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
    }
}
