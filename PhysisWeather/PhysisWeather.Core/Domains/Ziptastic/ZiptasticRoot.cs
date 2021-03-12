using System;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class ZiptasticRoot
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
