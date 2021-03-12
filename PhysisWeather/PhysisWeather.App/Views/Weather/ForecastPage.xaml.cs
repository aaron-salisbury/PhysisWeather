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

            ViewModel.WorkflowSuccessAction = this.AppBar.AnimateSucess;
            ViewModel.WorkflowFailureAction = this.AppBar.AnimateFailure;
        }

        private void ZipCodeSearchBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //TODO: Us zip to find coordinats. Then update location fields and conduct forecast.
        }

        private void ZipCodeSearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Numbers only and up to 5 digits.
            string queryText = ZipCodeSearchBox.Text;

            queryText = System.Text.RegularExpressions.Regex.Replace(queryText, "[^.0-9]", "");

            if (queryText.Length > 5)
            {
                queryText = queryText.Substring(0, 5);
            }

            ZipCodeSearchBox.Text = queryText;
        }
    }
}
