using System.Windows;
using ArmyBuilder.Windows.Views;

namespace ArmyBuilder.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseUnits_OnClick(object sender, RoutedEventArgs e)
        {
            new UnitExplorerWindow().Show();
        }
    }
}