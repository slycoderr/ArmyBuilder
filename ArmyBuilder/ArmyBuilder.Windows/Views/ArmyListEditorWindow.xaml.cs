using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using ArmyBuilder.Core.Utility;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Windows.Views
{
    public partial class ArmyListEditorWindow : Window
    {
        private MainViewModel MainViewModel
    =>
        DataContext != null && DataContext.GetType() == typeof(MainViewModel)
            ? (MainViewModel)DataContext
            : null;

        public ArmyListEditorWindow()
        {
            InitializeComponent();
            NewArmyListEditor.ListCreated += NewArmyListEditorOnListCreated;
        }

        private void NewArmyListEditorOnListCreated(object sender, EventArgs eventArgs)
        {
            NewArmyListEditor.Visibility = Visibility.Collapsed;
        }

        private void ArmyListsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NewArmyListEditor != null && EditArmyListEditor != null)
            {
                NewArmyListEditor.Visibility = Visibility.Collapsed;
                EditArmyListEditor.Visibility = ArmyListsListView.SelectedIndex != -1
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void ArmyListUpdated(object sender, ArmyListsUpdatedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var cvs = (CollectionViewSource)Application.Current.Resources["ArmyListCollection"];
                cvs.Source = e?.Groups?.OrderBy(g => g.ArmyId).SelectMany(l => l.ToList());
            });
        }

        private void ArmyListEditorWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((MainViewModel)e.NewValue).ArmyListUpdated -= ArmyListUpdated;
            }

            if (e.NewValue != null)
            {
                ((MainViewModel)e.NewValue).ArmyListUpdated += ArmyListUpdated;
            }
        }

        private async void ArmyListEditorWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToggleLoading();

            await MainViewModel.Load(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data.db"),
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "user.db"));
            ToggleLoading(false);
        }


        private void ToggleLoading(bool show = true)
        {
            ProgressPanel.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            IsEnabled = !show;
        }

        private void CreateArmyListButton_Click(object sender, RoutedEventArgs e)
        {
            ArmyListsListView.SelectedItem = null;
            NewArmyListEditor.Visibility = Visibility.Visible;
        }

        private void EditListBtton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
