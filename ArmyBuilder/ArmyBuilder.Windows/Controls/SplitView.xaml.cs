using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using EZMotion.Annotations;
using MenuItem = EZMotion.Data.MenuItem;

namespace EZMotion.Controls
{
    public partial class SplitView : UserControl
    {
        public delegate void SettingsButtonClickedEventHandler(object sender, RoutedEventArgs e);

        public event SettingsButtonClickedEventHandler SettingsButtonClicked;


        public bool ShowSettingsButton
        {
            get; //{ return (bool)GetValue(ShowSettingsButtonProperty); }
            set; //{ SetValue(ShowSettingsButtonProperty, value); SettingsButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();
        public bool IsCollapsed { get; private set; } = true;
        public double IconSize { get; set; } = 40;

        public SplitView()
        {
            InitializeComponent();
            List.ItemsSource = MenuItems;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            IsCollapsed = !IsCollapsed;
            MenuColumn.Width = IsCollapsed ? new GridLength(70) : new GridLength(250);
            List.ItemTemplate = IsCollapsed ? (DataTemplate) FindResource("MenuItemCollapsedTemplate") : (DataTemplate) FindResource("MenuItemExpandedTemplate");
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentPane.Content = ((MenuItem) List?.SelectedItem)?.View;
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsButtonClicked?.Invoke(this, e);
        }

        private void SplitView_OnLoaded(object sender, RoutedEventArgs e)
        {
            List.SelectedItem = MenuItems?.FirstOrDefault();
            ContentPane.Content = MenuItems?.FirstOrDefault()?.View;
        }

        /// <summary> 
        ///     SelectedIndex DependencyProperty
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
                DependencyProperty.Register(
                        "SelectedIndex",
                        typeof(int),
                        typeof(SplitView),
                        new FrameworkPropertyMetadata(
                                -1,
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                                new PropertyChangedCallback(OnSelectedIndexChanged),
                                new CoerceValueCallback(CoerceSelectedIndex)),
                        new ValidateValueCallback(ValidateSelectedIndex));

        /// <summary> 
        ///     The index of the first item in the current selection or -1 if the selection is empty. 
        /// </summary>
        [Bindable(true), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)] // not localizable
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SplitView s = (SplitView)d;

            //// If we're in the middle of a selection change, ignore all changes
            //if (!s.SelectionChange.IsActive)
            //{
            //    int newIndex = (int)e.NewValue;
            //    object item = (newIndex == -1) ? null : s.Items[newIndex];
            //    s.SelectionChange.SelectJustThisItem(item, true /* assumeInItemsCollection */);
            //}
        }

        private static object CoerceSelectedIndex(DependencyObject d, object value)
        {
            Selector s = (Selector)d;
            if ((value is int) && (int)value >= s.Items.Count)
            {
                return DependencyProperty.UnsetValue;
            }

            return value;
        }


        private static bool ValidateSelectedIndex(object o)
        {
            return ((int)o) >= -1;
        }
    }
}