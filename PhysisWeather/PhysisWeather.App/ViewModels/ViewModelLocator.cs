using GalaSoft.MvvmLight.Ioc;
using PhysisWeather.App.Base.Services;
using PhysisWeather.App.Views;

namespace PhysisWeather.App.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<SettingsViewModel, SettingsPage>();
            Register<LogViewModel, LogPage>();

            Register<ForecastViewModel, ForecastPage>();
            Register<IconsViewModel, IconsPage>();
        }

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();
        public ShellViewModel ShellViewModel => SimpleIoc.Default.GetInstance<ShellViewModel>();
        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();
        public LogViewModel LogViewModel => SimpleIoc.Default.GetInstance<LogViewModel>();

        public ForecastViewModel ForecastViewModel => SimpleIoc.Default.GetInstance<ForecastViewModel>();
        public IconsViewModel IconsViewModel => SimpleIoc.Default.GetInstance<IconsViewModel>();

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
