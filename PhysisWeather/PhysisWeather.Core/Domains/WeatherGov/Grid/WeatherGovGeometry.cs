using System;
using System.Collections.Generic;

namespace PhysisWeather.Core.Domains.WeatherGov.Grid
{
    [Serializable]
    public class WeatherGovGeometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }
}
