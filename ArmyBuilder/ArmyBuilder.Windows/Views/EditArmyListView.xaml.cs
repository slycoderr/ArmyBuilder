using System.Windows.Controls;
using ArmyBuilder.Core.Models;

namespace ArmyBuilder.Windows.Views
{
    public sealed partial class EditArmyListView : UserControl
    {
        private ArmyList CurrentArmyList => DataContext != null && DataContext.GetType() == typeof (ArmyList) ? (ArmyList) DataContext : null;

        public EditArmyListView()
        {
            InitializeComponent();
        }

        private void PointsTextBox_TextChanging(object o, TextChangedEventArgs textChangedEventArgs)
        {
            int points;

            if (int.TryParse(PointsTextBox.Text, out points) && points > 0)
            {
                CurrentArmyList.PointsLimit = points;
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (NameTextBox.Text.Length > 0)
            {
                CurrentArmyList.Name = NameTextBox.Text;
            }
        }
    }
}