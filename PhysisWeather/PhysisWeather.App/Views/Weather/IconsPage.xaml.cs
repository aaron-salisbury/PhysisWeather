using PhysisWeather.App.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static PhysisWeather.Core.Domains.WeatherPeriod;

namespace PhysisWeather.App.Views
{
    public sealed partial class IconsPage : Page
    {
        private IconsViewModel ViewModel
        {
            get => ViewModelLocator.Current.IconsViewModel;
        }

        public IconsPage()
        {
            this.InitializeComponent();
            DataContext = ViewModel;

            BuildDynamicIconGrid();
        }

        private void BuildDynamicIconGrid()
        {
            int numberOfRows = 5;
            int numberOfColumns = 4;

            Grid iconGrid = new Grid();

            for (int i = 0; i < numberOfRows; i++)
            {
                iconGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < numberOfColumns; i++)
            {
                iconGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            int index = 0;
            for (int row = 0; row < numberOfRows; row++)
            {
                for (int column = 0; column < numberOfColumns; column++)
                {
                    if (index < ViewModel.ForecastIcons.Count)
                    {
                        int iconSteps = 0;
                        foreach (KeyValuePair<IconTypes, ControlTemplate> iconByType in ViewModel.ForecastIcons)
                        {
                            if (iconSteps++ == index)
                            {
                                StackPanel iconData = new StackPanel
                                {
                                    Orientation = Orientation.Vertical,
                                    Margin = new Thickness(0)
                                };

                                TextBlock iconLabel = new TextBlock
                                {
                                    Text = iconByType.Key.ToString(),
                                    Margin = new Thickness(0)
                                };

                                ContentControl icon = new ContentControl
                                {
                                    Template = iconByType.Value,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    Margin = new Thickness(0)
                                };

                                iconData.Children.Add(iconLabel);
                                iconData.Children.Add(icon);

                                Grid.SetRow(iconData, row);
                                Grid.SetColumn(iconData, column);
                                iconGrid.Children.Add(iconData);

                                break;
                            }
                        }

                        index++;
                    }
                }
            }

            // "MainGrid" is set in the name attribute of the root grid in XAML.
            // We want to add this dynamic grid as a child of the root grid so it can take up all the remaining space available.
            Grid.SetRow(iconGrid, 0);
            IconScrollViewer.Children.Add(iconGrid);
        }
    }
}
