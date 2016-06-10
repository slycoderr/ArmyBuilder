using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Views
{
    public sealed partial class NewArmyListView : UserControl
    {
        private MainViewModel MainViewModel => DataContext != null && DataContext.GetType() == typeof (MainViewModel) ? (MainViewModel) DataContext : null;

        public NewArmyListView()
        {
            InitializeComponent();
            DoneButton.IsEnabled = ValidateInput();
        }

        public event EventHandler ListCreated;

        private void DoneButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            MainViewModel.AddList(new ArmyList(NameTextBox.Text, (Army) ArmyComboBox.SelectedItem, int.Parse(PointsTextBox.Text)));
            ListCreated?.Invoke(this, new EventArgs());
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

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
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