using PhysisWeather.Core.Base;
using PhysisWeather.Core.Domains;
using Windows.UI.Xaml.Controls;

namespace PhysisWeather.App.Models
{
    public class WeatherData : ObservableObject
    {
        private WeatherPeriod _weatherPeriod;
        public WeatherPeriod WeatherPeriod
        {
            get => _weatherPeriod;
            set
            {
                _weatherPeriod = value;
                RaisePropertyChanged();
            }
        }

        private ControlTemplate _icon = new ControlTemplate();
        public ControlTemplate Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                RaisePropertyChanged();
            }
        }
    }
}
