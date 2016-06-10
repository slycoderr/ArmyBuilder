using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;
using Microsoft.Xaml.Interactivity;

namespace ArmyBuilder.Utility
{
    public static class ArmyFlyoutMenuExtension
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
            DependencyProperty.Register("MyItems", typeof(List<MenuFlyout>), typeof(ArmyFlyoutMenuExtension),
            new PropertyMetadata(new List<MenuFlyoutItem>(), (sender, e) =>
            {
                var menu = sender as MenuFlyout;
                
                menu?.Items?.Clear();

                if (e.NewValue != null && menu != null)
                {
                    var armies = (ObservableCollection<Army>) e.NewValue;

                    foreach (var army in armies)
                    {
                        var newitem = new MenuFlyoutItem() {Text = string.Format("{0}", army.Name), Tag = army};

                        newitem.Click += (o, args) =>
                        {
                            //DesignerViewModel mainViewModel = DesignerViewModel.Current;

                            //mainViewModel.SelectedArmy = ((MenuFlyoutItem)o).Tag as Army;
                        };

                        menu.Items?.Add(newitem);
                    }
                }
            }));
    }

    public class CloseFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var flyout = sender as Flyout;
            if (flyout == null)
                throw new ArgumentException("CloseFlyoutAction can be used only with Flyout");

            flyout.Hide();

            return null;
        }
    }

    public static class FlyoutHelpers
    {
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.RegisterAttached("IsOpen", typeof(bool),
            typeof(FlyoutHelpers), new PropertyMetadata(false, OnIsOpenPropertyChanged));

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.RegisterAttached("Parent", typeof(Button),
            typeof(FlyoutHelpers), new PropertyMetadata(null, OnParentPropertyChanged));

        public static void SetIsOpen(DependencyObject d, bool value)
        {
            d.SetValue(IsOpenProperty, value);
        }

        public static bool GetIsOpen(DependencyObject d)
        {
            return (bool)d.GetValue(IsOpenProperty);
        }

        public static void SetParent(DependencyObject d, Button value)
        {
            d.SetValue(ParentProperty, value);
        }

        public static Button GetParent(DependencyObject d)
        {
            return (Button)d.GetValue(ParentProperty);
        }

        private static void OnParentPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var flyout = d as Flyout;
            if (flyout != null)
            {
                flyout.Opening += (s, args) =>
                {
                    flyout.SetValue(IsOpenProperty, true);
                };

                flyout.Closed += (s, args) =>
                {
                    flyout.SetValue(IsOpenProperty, false);
                };
            }
        }

        private static void OnIsOpenPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var flyout = d as Flyout;
            var parent = (Button)d.GetValue(ParentProperty);

            if (flyout != null && parent != null)
            {
                var newValue = (bool)e.NewValue;

                if (newValue)
                    flyout.ShowAt(parent);
                else
                    flyout.Hide();
            }
        }
    }


}
