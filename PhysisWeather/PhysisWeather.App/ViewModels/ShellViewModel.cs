using PhysisWeather.Core;
using PhysisWeather.Core.Base;
using PhysisWeather.Core.Domains;
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

            Task.Run(() => SetLocationCoordinatesAsync()).Wait();
        }

        private async Task SetLocationCoordinatesAsync()
        {
            if (await Geolocator.RequestAccessAsync() == GeolocationAccessStatus.Allowed)
            {
                Geolocator geolocator = new Geolocator
                {
                    DesiredAccuracyInMeters = DESIRED_ACCURACY_IN_METERS
                };

                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                if (geoposition != null && geoposition.Coordinate != null)
                {
                    if (geoposition.Coordinate != null)
                    {
                        Manager.Coordinates = new Coordinates
                        {
                            Longitude = geoposition.Coordinate.Point?.Position.Longitude.ToString(),
                            Latitude = geoposition.Coordinate.Point?.Position.Latitude.ToString()
                        };
                    }
                }
            }

            //TODO: If coordinates are null, try loading saved ones.
        }
    }
}
