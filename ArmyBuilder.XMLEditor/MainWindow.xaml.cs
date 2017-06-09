using System.Windows;
using ArmyBuilder.XMLEditor.Windows;

namespace ArmyBuilder.XMLEditor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddMassUnits_OnClick(object sender, RoutedEventArgs e)
        {
            new MassUnitEditorWindow().ShowDialog();
        }

        private void AddMassEquipment_OnClick(object sender, RoutedEventArgs e)
        {
            new MassEquipmentEditorWindow().ShowDialog();
        }
    }
}