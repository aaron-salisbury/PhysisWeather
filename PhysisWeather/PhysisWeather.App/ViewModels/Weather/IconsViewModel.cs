using GalaSoft.MvvmLight.Threading;
using PhysisWeather.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhysisWeather.App.ViewModels
{
    public class IconsViewModel : BaseViewModel
    {
        private Dictionary<WeatherPeriod.IconTypes, ControlTemplate> _forecastIcons;
        public Dictionary<WeatherPeriod.IconTypes, ControlTemplate> ForecastIcons
        {
            get => _forecastIcons;
            set
            {
                _forecastIcons = value;
                RaisePropertyChanged();
            }
        }

        public IconsViewModel()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                LoadForecastIconCacheAsync();
            });
        }

        private void LoadForecastIconCacheAsync()
        {
            Dictionary<WeatherPeriod.IconTypes, ControlTemplate> forecastIcons = new Dictionary<WeatherPeriod.IconTypes, ControlTemplate>();

            foreach (WeatherPeriod.IconTypes iconType in Enum.GetValues(typeof(WeatherPeriod.IconTypes)).Cast<WeatherPeriod.IconTypes>())
            {
                string iconTypeString = iconType.ToString();

                if (Application.Current.Resources.ContainsKey(iconTypeString))
                {
                    forecastIcons[iconType] = (ControlTemplate)Application.Current.Resources[iconTypeString];
                }
            }

            ForecastIcons = forecastIcons;
        }
    }
}
