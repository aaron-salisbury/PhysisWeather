using PhysisWeather.App.ViewModels;
using Windows.UI.Xaml.Controls;

namespace PhysisWeather.App.Views
{
    public sealed partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.Current.LogViewModel;
        }
    }
}
