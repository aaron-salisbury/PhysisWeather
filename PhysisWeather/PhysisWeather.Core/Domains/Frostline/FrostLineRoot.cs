using System;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class FrostLineRoot
    {
        public FrostLineCoordinates Coordinates { get; set; }
        public string Zone { get; set; }
        public string Temperature_Range { get; set; }
    }
}
