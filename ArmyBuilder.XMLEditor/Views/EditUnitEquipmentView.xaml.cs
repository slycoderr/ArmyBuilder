using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private TreeViewItem selectedDefault;

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
                TreeViewItem i = new TreeViewItem { Header = e.Name, DataContext = e};
                item.Items.Add(i);
                TraverseEquipment(i, e);
            }

            foreach (var e in equip.GivenEquipment)
            {
                TreeViewItem i = new TreeViewItem { Header = e.Name, DataContext = e};
                item.Items.Add(i);
                TraverseEquipment(i, e);
            }
        }

        public TreeViewItem GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TreeViewItem;
        }

        private void RemoveDefaultEquipmentItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedTreeItem = DefaultEquipmentTreeView.SelectedItem as TreeViewItem;
            var selectedEquipment = selectedTreeItem.DataContext as Equipment;
            var parentTreeItem = GetSelectedTreeViewItemParent(selectedTreeItem);

            if (parentTreeItem != null)
            {
                var parentEquipment = parentTreeItem.DataContext as Equipment; //got the parent

                //determine whether its a replacement option or given by comparing the reference
                foreach (var equip in parentEquipment.ReplacementOptions)
                {
                    if (ReferenceEquals(equip, selectedEquipment))
                    {
                        parentEquipment.ReplacementOptions.Remove(selectedEquipment);
                        parentTreeItem.Items.Remove(selectedTreeItem);
                        break;
                    }
                }

                foreach (var equip in parentEquipment.GivenEquipment)
                {
                    if (ReferenceEquals(equip, selectedEquipment))
                    {
                        parentEquipment.GivenEquipment.Remove(selectedEquipment);
                        parentTreeItem.Items.Remove(selectedTreeItem);
                        break;
                    }
                }
            }

            else
            {//remove that whole node
                ((MainViewModel) FindResource("MainViewModel")).SelectedUnit.DefaultEquipment.Remove(DefaultEquipmentTreeView.SelectedItem as Equipment);
                DefaultEquipmentTreeView.Items.Remove(selectedTreeItem);
                selectedEquipment.ReplacementOptions = new ObservableCollection<Equipment>();
                selectedEquipment.GivenEquipment = new ObservableCollection<Equipment>();
            }
        }

        private void RemoveUpgradeItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedTreeItem = UpgradesTreeView.SelectedItem as TreeViewItem;
            var selectedEquipment = selectedTreeItem.DataContext as Equipment;
            var parentTreeItem = GetSelectedTreeViewItemParent(selectedTreeItem);

            if (parentTreeItem != null)
            {
                var parentEquipment = parentTreeItem.DataContext as Equipment; //got the parent

                //determine whether its a replacement option or given by comparing the reference
                foreach (var equip in parentEquipment.ReplacementOptions)
                {
                    if (ReferenceEquals(equip, selectedEquipment))
                    {
                        parentEquipment.ReplacementOptions.Remove(selectedEquipment);
                        parentTreeItem.Items.Remove(selectedTreeItem);
                        break;
                    }
                }

                foreach (var equip in parentEquipment.GivenEquipment)
                {
                    if (ReferenceEquals(equip, selectedEquipment))
                    {
                        parentEquipment.GivenEquipment.Remove(selectedEquipment);
                        parentTreeItem.Items.Remove(selectedTreeItem);
                        break;
                    }
                }
            }

            else
            {//remove that whole node
                ((MainViewModel)FindResource("MainViewModel")).SelectedUnit.Upgrades.Remove(UpgradesTreeView.SelectedItem as Equipment);
                UpgradesTreeView.Items.Remove(selectedTreeItem);
                selectedEquipment.ReplacementOptions = new ObservableCollection<Equipment>();
                selectedEquipment.GivenEquipment = new ObservableCollection<Equipment>();
            }
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                //e.Handled = true;
                selectedDefault = treeViewItem;
            }


            
        }
        

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
