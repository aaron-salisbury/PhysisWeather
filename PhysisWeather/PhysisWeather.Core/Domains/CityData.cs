using System;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class CityData
    {
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}
