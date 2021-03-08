using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PhysisWeather.Core.Domains.WeatherGov.Forecast
{
    [Serializable]
    public class WeatherGovRoot
    {
        [JsonProperty("@context")]
        public List<object> Context { get; set; }

        public string Type { get; set; }
        public WeatherGovGeometry Geometry { get; set; }
        public WeatherGovProperties Properties { get; set; }
    }
}
