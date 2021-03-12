using System;

namespace PhysisWeather.App.Base.Services.ServiceDomains
{
    [Serializable]
    internal class BigDataCloudRoot
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LookupSource { get; set; }
        public string PlusCode { get; set; }
        public string LocalityLanguageRequested { get; set; }
        public string Continent { get; set; }
        public string ContinentCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PrincipalSubdivision { get; set; }
        public string PrincipalSubdivisionCode { get; set; }
        public string City { get; set; }
        public string Locality { get; set; }
        public string Postcode { get; set; }
        public BigDataCloudLocalityInfo LocalityInfo { get; set; }
    }
}
