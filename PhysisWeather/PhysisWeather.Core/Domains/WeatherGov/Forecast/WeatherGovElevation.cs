using System;

namespace PhysisWeather.Core.Domains.WeatherGov.Forecast
{
    [Serializable]
    public class WeatherGovElevation
    {
        public double Value { get; set; }
        public string UnitCode { get; set; }
    }
}
