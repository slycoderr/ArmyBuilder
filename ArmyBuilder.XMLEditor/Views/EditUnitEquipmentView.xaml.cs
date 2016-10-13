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
using ArmyBuilder.Core.Models;

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
            var collection = tree.Items.Cast<Equipment>().ToList();

            foreach (var item in collection)
            {
                TraverseEquipment(tree.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem, item);
            }
        }

        private void TraverseEquipment(TreeViewItem item, Equipment equip)
        {
            item.IsExpanded = true;

            foreach (var e in equip.ReplacementOptions)
            {
                TreeViewItem i = new TreeViewItem { Header = e.Name };
                item.Items.Add(i);
                TraverseEquipment(i, e);
            }

            foreach (var e in equip.GivenEquipment)
            {
                TreeViewItem i = new TreeViewItem { Header = e.Name };
                item.Items.Add(i);
                TraverseEquipment(i, e);
            }
        }
    }
}
