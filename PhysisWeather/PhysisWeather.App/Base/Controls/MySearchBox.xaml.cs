using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhysisWeather.App.Base.Controls
{
    public sealed partial class MySearchBox : UserControl
    {
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(MySearchBox.PlaceholderText), typeof(string), typeof(MySearchBox), null);

        public MySearchBox()
        {
            this.InitializeComponent();
        }
    }
}
