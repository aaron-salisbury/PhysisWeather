using PhysisWeather.Core.Base;
using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PhysisWeather.App.Base.Services
{
    internal class LocationService
    {
        private const uint DESIRED_ACCURACY_IN_METERS = 100;

        internal static async Task<string> GetZipForGeopositionAsync(AppLogger appLogger)
        {
            string zip = null;

            GeolocationAccessStatus locationAccess = await Geolocator.RequestAccessAsync();

            if (locationAccess == GeolocationAccessStatus.Allowed)
            {
                Geolocator geolocator = new Geolocator
                {
                    DesiredAccuracyInMeters = DESIRED_ACCURACY_IN_METERS
                };

                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                if (geoposition != null && geoposition.Coordinate != null && geoposition.Coordinate.Point != null)
                {
                    zip = await BigDataCloudReverseGeocodingService.GetZipAsync(geoposition, appLogger);
                }
            }

            return zip;
        }
    }
}
