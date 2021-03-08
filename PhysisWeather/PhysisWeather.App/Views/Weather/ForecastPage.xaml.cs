using PhysisWeather.App.ViewModels;
using Windows.UI.Xaml.Controls;

namespace PhysisWeather.App.Views
{
    public sealed partial class ForecastPage : Page
    {
        private ForecastViewModel ViewModel
        {
            get => ViewModelLocator.Current.ForecastViewModel;
        }

        public ForecastPage()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
