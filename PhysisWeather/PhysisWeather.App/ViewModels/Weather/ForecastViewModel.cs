using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using PhysisWeather.App.Base.Services;
using PhysisWeather.App.Models;
using PhysisWeather.Core.Domains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhysisWeather.App.ViewModels
{
    public class ForecastViewModel : BaseViewModel
    {
        private Dictionary<WeatherPeriod.IconTypes, ControlTemplate> _forecastIconCache;

        public Action WorkflowSuccessAction { get; set; }
        public Action WorkflowFailureAction { get; set; }

        public RelayCommand RefreshCommand { get; }
        public RelayCommand SearchZipCommand { get; }

        private string _zipCode;
        public string ZipCode
        {
            get => _zipCode;
            set
            {
                Set(ref _zipCode, value);
                Manager.SearchZip = value;
            }
        }

        private ObservableCollection<WeatherData> _forecastDays;
        public ObservableCollection<WeatherData> ForecastDays
        {
            get => _forecastDays;
            set
            {
                Set(ref _forecastDays, value);
            }
        }

        private ObservableCollection<WeatherData> _forecastHours;
        public ObservableCollection<WeatherData> ForecastHours
        {
            get => _forecastHours;
            set
            {
                Set(ref _forecastHours, value);
            }
        }

        private WeatherData _currentWeatherData;
        public WeatherData CurrentWeatherData
        {
            get => _currentWeatherData;
            set
            {
                Set(ref _currentWeatherData, value);
            }
        }

        private WeatherData _nextWeatherData;
        public WeatherData NextWeatherData
        {
            get => _nextWeatherData;
            set
            {
                Set(ref _nextWeatherData, value);
            }
        }

        private WeatherData _nextWeatherDataSecond;
        public WeatherData NextWeatherDataSecond
        {
            get => _nextWeatherDataSecond;
            set
            {
                Set(ref _nextWeatherDataSecond, value);
            }
        }

        public ForecastViewModel()
        {
            Manager.PropertyChanged += Manager_OnPropertyChanged;

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                LoadForecastIconCache();
            });

            SearchZipCommand = new RelayCommand(async () => await InitiateProcessAsync(UpdateAndForecast, SearchZipCommand, WorkflowSuccessAction, WorkflowFailureAction), () => !IsBusy);
            RefreshCommand = new RelayCommand(async () => await InitiateProcessAsync(Manager.Forecast, RefreshCommand, WorkflowSuccessAction, WorkflowFailureAction), () => !IsBusy);
        }

        internal async Task InitializeLocationAndForecastAsync()
        {
            if (Manager.DemographicData == null)
            {
                //TODO: This should turn on the is busy progress wheel.
                ZipCode = await LocationService.GetZipForGeopositionAsync(AppLogger);

                if (!string.IsNullOrEmpty(ZipCode))
                {
                    SearchZipCommand.Execute(null);
                }
            }
        }

        private bool UpdateAndForecast()
        {
            if (!string.IsNullOrEmpty(ZipCode))
            {
                Manager.BuildDemographicData();

                Manager.Forecast();
            }

            return true;
        }

        private void Manager_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(Core.Manager.WeatherForecast)))
            {
                if (Manager?.WeatherForecast != null)
                {
                    List<WeatherData> days = BuildDataFromPeriods(Manager.WeatherForecast.Days).ToList();

                    CurrentWeatherData = days.Where(d => d.WeatherPeriod.Number == 1)?.FirstOrDefault();
                    days.Remove(CurrentWeatherData);

                    NextWeatherData = days.Where(d => d.WeatherPeriod.Number == 2)?.FirstOrDefault();
                    days.Remove(NextWeatherData);

                    NextWeatherDataSecond = days.Where(d => d.WeatherPeriod.Number == 3)?.FirstOrDefault();
                    days.Remove(NextWeatherDataSecond);

                    ForecastDays = new ObservableCollection<WeatherData>(days);
                    ForecastHours = new ObservableCollection<WeatherData>(BuildDataFromPeriods(Manager.WeatherForecast.Hours));
                }
                else
                {
                    ForecastDays = new ObservableCollection<WeatherData>();
                    ForecastHours = new ObservableCollection<WeatherData>();
                }
            }
        }

        private IEnumerable<WeatherData> BuildDataFromPeriods(List<WeatherPeriod> weatherPeriods)
        {
            List<WeatherData> data = new List<WeatherData>();

            foreach (WeatherPeriod period in weatherPeriods)
            {
                data.Add(new WeatherData
                {
                    WeatherPeriod = period,
                    Icon = _forecastIconCache[period.IconType]
                });
            }

            return data;
        }

        private void LoadForecastIconCache()
        {
            _forecastIconCache = new Dictionary<WeatherPeriod.IconTypes, ControlTemplate>();

            foreach (WeatherPeriod.IconTypes iconType in Enum.GetValues(typeof(WeatherPeriod.IconTypes)).Cast<WeatherPeriod.IconTypes>())
            {
                // For this technique, there has to be weather icon control templates in the app xaml resources for every WeatherPeriod.IconTypes
                // and they have to be named the same.
                _forecastIconCache[iconType] = (ControlTemplate)Application.Current.Resources[iconType.ToString()];
            }
        }
    }
}
