using PhysisWeather.App.Base.Services;
using PhysisWeather.Core;
using PhysisWeather.Core.Base;
using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
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
            //      Maybe it needs the dispatcher to ask?
            if (await Geolocator.RequestAccessAsync() == GeolocationAccessStatus.Allowed)
            {
                Geolocator geolocator = new Geolocator
                {
                    DesiredAccuracyInMeters = DESIRED_ACCURACY_IN_METERS
                };

                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                if (geoposition != null && geoposition.Coordinate != null && geoposition.Coordinate.Point != null)
                {
                    Manager.SearchZip = await BigDataCloudReverseGeocodingService.GetZipAsync(geoposition, AppLogger);
                    Manager.BuildDemographicData();
                }
            }

            //TODO: If Manager.DemographicData is null, try loading saved data.
        }
    }
}
