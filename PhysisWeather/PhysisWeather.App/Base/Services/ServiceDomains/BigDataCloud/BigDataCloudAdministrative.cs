using System;

namespace PhysisWeather.App.Base.Services.ServiceDomains
{
    [Serializable]
    internal class BigDataCloudAdministrative
    {
        public int Order { get; set; }
        public int AdminLevel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsoName { get; set; }
        public string IsoCode { get; set; }
        public string WikidataId { get; set; }
        public int GeonameId { get; set; }
    }
}
