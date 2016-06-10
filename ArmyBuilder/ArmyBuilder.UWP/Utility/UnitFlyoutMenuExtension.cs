using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Utility
{
    public static class UnitFlyoutMenuExtension
    {
        public static List<MenuFlyoutItem> GetMyItems(DependencyObject obj)
        {
            return (List<MenuFlyoutItem>)obj.GetValue(MyItemsProperty);
        }

        public static void SetMyItems(DependencyObject obj, List<MenuFlyoutItem> value)
        {
            obj.SetValue(MyItemsProperty, value);
        }

        public static readonly DependencyProperty MyItemsProperty =
            DependencyProperty.Register("MyItems", typeof(List<MenuFlyoutItem>), typeof(UnitFlyoutMenuExtension),
            new PropertyMetadata(new List<MenuFlyoutItem>(), (sender, e) =>
            {
                var menu = sender as MenuFlyoutSubItem;
                
                menu?.Items?.Clear();

                if (e.NewValue != null && menu != null)
                {
                    foreach (var item in e.NewValue as List<Unit>)
                    {
                        var newitem = new MenuFlyoutItem() {Text = string.Format("{0} ({1} points)", item.Name, item.Models.Sum(m=>m.BaseCost)), Tag = item};

                        newitem.Click += (o, args) =>
                        {
                            MainViewModel mainViewModel = (MainViewModel)Application.Current.Resources["MainViewModel"];

                            mainViewModel.CurrentArmyListViewModel.AddUnit(((MenuFlyoutItem)o).Tag as Unit);
                        };

                        menu.Items?.Add(newitem);
                    }
                }
            }));
    }
}
