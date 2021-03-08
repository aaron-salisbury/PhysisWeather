using System;
using System.ComponentModel.DataAnnotations;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class Coordinates
    {
        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }
    }
}
