using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArmyBuilder.XMLEditor.Views
{
    public partial class EditUnitEquipmentView
    {
        public EditUnitEquipmentView()
        {
            InitializeComponent();
            DependencyPropertyDescriptor.FromProperty(TreeView.ItemsSourceProperty, typeof(TreeView)).AddValueChanged(DefaultEquipmentTreeView, (s, e) => { BuildEquipmentTree(s as TreeView); });
            DependencyPropertyDescriptor.FromProperty(TreeView.ItemsSourceProperty, typeof(TreeView)).AddValueChanged(UpgradesTreeView, (s, e) => { BuildEquipmentTree(s as TreeView); });
        }

        private void BuildEquipmentTree(TreeView tree)
        {
            
        }
    }
}
