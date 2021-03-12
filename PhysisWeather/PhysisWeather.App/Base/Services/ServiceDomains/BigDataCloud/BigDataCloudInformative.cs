using System;

namespace PhysisWeather.App.Base.Services.ServiceDomains
{
    [Serializable]
    internal class BigDataCloudInformative
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsoCode { get; set; }
        public string WikidataId { get; set; }
        public int GeonameId { get; set; }
    }
}
