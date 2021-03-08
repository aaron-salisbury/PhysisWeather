using System;
using System.Collections.Generic;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class WeatherForecast
    {
        public List<WeatherPeriod> Days { get; set; }
        public List<WeatherPeriod> Hours { get; set; }
    }
}
