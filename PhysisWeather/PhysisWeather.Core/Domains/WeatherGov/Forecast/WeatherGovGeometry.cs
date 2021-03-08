using System;
using System.Collections.Generic;

namespace PhysisWeather.Core.Domains.WeatherGov.Forecast
{
    [Serializable]
    public class WeatherGovGeometry
    {
        public string Type { get; set; }
        public List<List<List<double>>> Coordinates { get; set; }
    }
}
