using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.UWP.Views
{
    public sealed partial class NewArmyListView
    {
        private MainViewModel MainViewModel => DataContext != null && DataContext.GetType() == typeof (MainViewModel) ? (MainViewModel) DataContext : null;

        public NewArmyListView()
        {
            InitializeComponent();
            DoneButton.IsEnabled = ValidateInput();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.AddList(new ArmyList(NameTextBox.Text, int.Parse(PointsTextBox.Text),(Army) ArmyComboBox.SelectedItem, (Detachment)PrimaryDetachmentComboBox.SelectedItem));
            ArmyComboBox.SelectedIndex = -1;
            PointsTextBox.Text = string.Empty;
            NameTextBox.Text = string.Empty;
        }

        private void ArmyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoneButton.IsEnabled = ValidateInput();
        }

        private void PointsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoneButton.IsEnabled = ValidateInput();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoneButton.IsEnabled = ValidateInput();
        }

        private bool ValidateInput()
        {
            int points;

            if (!NameTextBox.Text.Any())
            {
                return false;
            }

            //if (MainViewModel.ArmyLists.Any(l => l.Name == NameTextBox.Text))
            //{
            //    return false;
            //}

            if (ArmyComboBox.SelectedIndex == -1)
            {
                return false;
            }

            if (PrimaryDetachmentComboBox.SelectedIndex == -1)
            {
                return false;
            }

            if (!PointsTextBox.Text.Any())
            {
                return false;
            }

            if (!int.TryParse(PointsTextBox.Text, out points) || points < 0)
            {
                return false;
            }

            return true;
        }
    }
}