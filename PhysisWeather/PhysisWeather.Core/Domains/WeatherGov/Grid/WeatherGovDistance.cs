using System;

namespace PhysisWeather.Core.Domains.WeatherGov.Grid
{
    [Serializable]
    public class WeatherGovDistance
    {
        public double Value { get; set; }
        public string UnitCode { get; set; }
    }
}
