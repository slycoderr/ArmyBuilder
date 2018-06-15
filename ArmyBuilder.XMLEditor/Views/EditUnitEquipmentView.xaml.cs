using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArmyBuilder.XMLEditor.Views
{
    public partial class EditUnitEquipmentView
    {
        public EditUnitEquipmentView()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            treeViewItem?.Focus();
        }

        private static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while ((source != null) && !(source is TreeViewItem) && source is Visual)
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}