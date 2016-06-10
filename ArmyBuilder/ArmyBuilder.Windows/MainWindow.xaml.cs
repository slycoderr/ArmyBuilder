using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Utility;
using ArmyBuilder.Core.ViewModels;
using ArmyBuilder.Windows.Views;

namespace ArmyBuilder.Windows
{
    public partial class MainWindow : Window
    {
        private ArmyListViewModel ArmyListViewModel
            =>
                DataContext != null && DataContext.GetType() == typeof (ArmyListViewModel)
                    ? (ArmyListViewModel) DataContext
                    : null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UnitListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnitEditor.Visibility = UnitListView.SelectedItem != null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleLoading(bool show = true)
        {
            ProgressPanel.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            IsEnabled = !show;
        }

        private void TroopItemsMenu_Click(object sender, RoutedEventArgs e)
        {
            var unit = ((MenuItem) e.OriginalSource).DataContext as Unit;

            ArmyListViewModel.AddUnit(unit);
        }

        private void MainWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ArmyListViewModel != null)
            {
                //ArmyListViewModel.UpdateArmyListDataSource(); //force since it misses the update
            }
        }

        private void ManageArmyListButton_Click(object sender, RoutedEventArgs e)
        {
            new ArmyListEditorWindow().Show();
            Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (ArmyListViewModel != null)
            {
                //ArmyListViewModel.UnitListUpdated -= UnitListUpdated;
            }
        }
    }
}