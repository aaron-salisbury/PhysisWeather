using System;
using System.Collections.Generic;

namespace PhysisWeather.Core.Domains.WeatherGov.Forecast
{
    [Serializable]
    public class WeatherGovProperties
    {
        public DateTime Updated { get; set; }
        public string Units { get; set; }
        public string ForecastGenerator { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ValidTimes { get; set; }
        public WeatherGovElevation Elevation { get; set; }
        public List<WeatherGovPeriod> Periods { get; set; }
    }
}
