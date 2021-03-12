using System;
using System.Collections.Generic;

namespace PhysisWeather.App.Base.Services.ServiceDomains
{
    [Serializable]
    internal class BigDataCloudLocalityInfo
    {
        public List<BigDataCloudAdministrative> Administrative { get; set; }
        public List<BigDataCloudInformative> Informative { get; set; }
    }
}
