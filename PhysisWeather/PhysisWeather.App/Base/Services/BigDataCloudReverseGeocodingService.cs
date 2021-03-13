using PhysisWeather.App.Base.Services.ServiceDomains;
using PhysisWeather.Core.Base;
using PhysisWeather.Core.Base.Helpers;
using PhysisWeather.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysisWeather.App.Base.Services
{
    // API Documentation: https://www.bigdatacloud.com/geocoding-apis/free-reverse-geocode-to-city-api
    internal class BigDataCloudReverseGeocodingService
    {
        private const string URL_FORMAT = "https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={0}&longitude={1}&localityLanguage=en"; // {0}Latitude, {1}Longitude

        /// <summary>
        /// Use Policy: May only use current coordinates obtained from a client's web browser using getCurrentPosition() method or
        /// from client devices using their own current coordinates only.
        /// IMPORTANT! Using elsewhere obtained coordinates with BigDataCloud's free API services can lead to blacklisting.
        /// </summary>
        public static async Task<string> GetZipAsync(double longitude, double latitude, AppLogger appLogger)
        {
            try
            {
                string url = string.Format(URL_FORMAT, latitude, longitude);
                string json = await WebRequests.GetCurlResponseAsync(url, appLogger.Logger);
                BigDataCloudRoot root = await Json.ToObjectAsync<BigDataCloudRoot>(json);

                return root?.Postcode;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
