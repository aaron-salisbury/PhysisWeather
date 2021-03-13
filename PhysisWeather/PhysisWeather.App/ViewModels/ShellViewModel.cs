using PhysisWeather.App.Base.Services;
using PhysisWeather.Core;
using PhysisWeather.Core.Base;
using PhysisWeather.Core.Domains;
using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml;

namespace PhysisWeather.App.ViewModels
{
    public class ShellViewModel : BaseNavigableViewModel
    {
        private const uint DESIRED_ACCURACY_IN_METERS = 100;

        public static Visibility IsDebug
        {
#if DEBUG
            get { return Visibility.Visible; }
#else
            get { return Visibility.Collapsed; }
#endif
        }

        public AppLogger AppLogger { get; set; }
        public Manager Manager { get; set; }

        public ShellViewModel()
        {
            AppLogger = new AppLogger();
            Manager = new Manager(AppLogger);

            Task.Run(() => SetDemographicDataAsync()).Wait();
        }

        private async Task SetDemographicDataAsync()
        {
            Manager.DemographicData = null;

            //TODO: This seems to break if permission isn't already granted.
            //      Hasn't even been asking to grant permission.
            //      Maybe try to set it up in a switch like template studio does.
            if (await Geolocator.RequestAccessAsync() == GeolocationAccessStatus.Allowed)
            {
                Geolocator geolocator = new Geolocator
                {
                    DesiredAccuracyInMeters = DESIRED_ACCURACY_IN_METERS
                };

                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                if (geoposition != null && geoposition.Coordinate != null)
                {
                    if (geoposition.Coordinate != null && geoposition.Coordinate.Point != null)
                    {
                        string zip = await BigDataCloudReverseGeocodingService.GetZipAsync(
                            geoposition.Coordinate.Point.Position.Longitude, 
                            geoposition.Coordinate.Point.Position.Latitude,
                            AppLogger);

                        Manager.BuildDemographicData(zip);
                    }
                }
            }

            //TODO: If Manager.DemographicData is null, try loading saved data.
        }
    }
}
