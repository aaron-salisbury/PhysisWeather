using PhysisWeather.App.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PhysisWeather.App.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel
        {
            get => ViewModelLocator.Current.SettingsViewModel;
        }

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }
    }
}
