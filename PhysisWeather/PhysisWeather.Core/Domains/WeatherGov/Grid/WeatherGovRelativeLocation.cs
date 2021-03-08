using System;

namespace PhysisWeather.Core.Domains.WeatherGov.Grid
{
    [Serializable]
    public class WeatherGovRelativeLocation
    {
        public string Type { get; set; }
        public WeatherGovGeometry Geometry { get; set; }
        public WeatherGovProperties Properties { get; set; }
    }
}
