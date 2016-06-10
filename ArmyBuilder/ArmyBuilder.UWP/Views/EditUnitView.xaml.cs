using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;
using ArmyBuilder.Core.Utility;

namespace ArmyBuilder.UWP.Views
{
    public sealed partial class EditUnitView : UserControl
    {
        private ArmyListData ArmyListData => DataContext != null && DataContext.GetType() == typeof (ArmyListData)
            ? (ArmyListData) DataContext
            : null;

        public EditUnitView()
        {
            InitializeComponent();
            EquipmentListView.SelectionMode = ListViewSelectionMode.Single;
        }

        private void EditUnitView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
        }

        private void UnitSizeSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (ArmyListData != null)
            {
                //ArmyListData.CurrentUnitSize = (int) UnitSizeSlider.Value;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EquipmentText != null)
            {
                //EquipmentText.Text =
                //    ((UnitEquipmentData) EquipmentListView.SelectedItem)?.EquipmentInfo.Equipment.Details ??
                //    string.Empty;

                //EquipmentTextScrollViewer.Visibility = string.IsNullOrEmpty(EquipmentText.Text)
                //    ? Visibility.Collapsed
                //    : Visibility.Visible;
                //EquipmentListView.SetValue(Grid.RowSpanProperty, string.IsNullOrEmpty(EquipmentText.Text) ? 2 : 1);
            }
        }
    }

    public class ShowUnitSizeEditorConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            //return ((ArmyListData) value).UnitData.MaxUnitSize == ((ArmyListData) value).UnitData.MinUnitsize
            //    ? Visibility.Collapsed
            //    : Visibility.Visible;
            return null;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}