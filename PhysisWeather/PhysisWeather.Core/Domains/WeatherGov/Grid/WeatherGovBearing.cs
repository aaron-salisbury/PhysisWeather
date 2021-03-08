using System;

namespace PhysisWeather.Core.Domains.WeatherGov.Grid
{
    [Serializable]
    public class WeatherGovBearing
    {
        public int Value { get; set; }
        public string UnitCode { get; set; }
    }
}
