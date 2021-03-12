using System;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class DemographicData
    {
        public CityData CityData { get; set; }
        public Coordinates Coordinates { get; set; }
    }
}
