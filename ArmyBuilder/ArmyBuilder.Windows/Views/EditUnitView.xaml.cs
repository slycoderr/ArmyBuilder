using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;
using ArmyBuilder.Core.Utility;
using ArmyBuilder.Utility;

namespace ArmyBuilder.Views
{
    public sealed partial class EditUnitView : UserControl
    {
        private ArmyListData UnitData => DataContext != null && DataContext.GetType() == typeof(ArmyListData)
    ? (ArmyListData)DataContext
    : null;

        public EditUnitView()
        {
            InitializeComponent();
        }

        private void EditUnitView_OnDataContextChanged(object o, DependencyPropertyChangedEventArgs args)
        {
            if (DataContext != null && DataContext.GetType() == typeof(ArmyListData))
            {
                var selectedItem = EquipmentListView.SelectedItem;
                List<UnitEquipmentGroup> allGroups = new List<UnitEquipmentGroup>();
                var allDependantGroups = ((ArmyListData)DataContext).UnitEquipmentData.Where(e => e.EquipmentInfo.Profile.EquipmentType == EquipmentType.DependantOn).GroupBy(i => i.EquipmentInfo.Profile.EquipmentTypeNumbers).Select(i => new UnitEquipmentGroup(i.ToList(), GroupFlag.ByDependId)).ToList(); //first group dependent gear together
                var allMutualGroups = ((ArmyListData)DataContext).UnitEquipmentData.Where(e => e.EquipmentInfo.Profile.EquipmentType == EquipmentType.MutuallyExclusive).GroupBy(i => i.EquipmentInfo.Profile.EquipmentTypeNumbers).Select(i => new UnitEquipmentGroup(i.ToList(), GroupFlag.ByDependId)).ToList(); //first group dependent gear together
                var allReplaceIdGroups = ((ArmyListData)DataContext).UnitEquipmentData.Where(e => e.EquipmentInfo.Profile.EquipmentType != EquipmentType.DependantOn).GroupBy(i => i.EquipmentInfo.Profile.ReplacesIdList.ToList(), new ArmyBuilder.Core.Utility.IntListComparer()).Select(i => new UnitEquipmentGroup(i.ToList(), GroupFlag.ByReplaceId)); //then group the rest by replaces id
                //var dependantGroupsByReplaceId = allDependantGroups.SelectMany(i => i).GroupBy(i => i.EquipmentInfo.ReplacesIdList, new IntListComparer()).Select(i => new UnitEquipmentGroup(i, GroupFlag.ByDependantIdAndReplaceId)); //group dependant gear by their replace id

                allGroups.AddRange(allReplaceIdGroups);

                foreach (var group in allDependantGroups) //create seperate groups for each dependant id
                {
                    allGroups.AddRange(group.GroupBy(i => i.EquipmentInfo.Profile.ReplacesIdList.ToList(), new IntListComparer()).Select(i => new UnitEquipmentGroup(i.ToList(), GroupFlag.ByDependantIdAndReplaceId)));
                }

                foreach (var group in allMutualGroups) //create seperate groups for each dependant id
                {
                    allGroups.AddRange(group.GroupBy(i => i.EquipmentInfo.Profile.ReplacesIdList.ToList(), new IntListComparer()).Select(i => new UnitEquipmentGroup(i.ToList(), GroupFlag.ByMutualIdAndReplaceId)));
                }

                //allGroups.AddRange(dependantGroupsByReplaceId);

                var cvs = (CollectionViewSource)Application.Current.Resources["UnitEquipmentCollection"];

                cvs.Source = allGroups.SelectMany(l=>l.ToList()).ToList();
                UnitSizeSlider.Value = UnitData.CurrentUnitSize;
                UnitSizeSlider.Minimum = UnitData.UnitData.MinUnitsize;
                UnitSizeSlider.Maximum = UnitData.UnitData.MaxUnitSize;
                UnitSizeSlider.SmallChange = UnitData.UnitData.Step;
                UnitSizeSlider.LargeChange = UnitData.UnitData.Step;
                UnitSizeSlider.Visibility = UnitData.UnitData.Step == 0
                    ? Visibility.Collapsed
                    : Visibility.Visible;
                EditUnitSizeLabel.Visibility = UnitData.UnitData.Step == 0
                    ? Visibility.Collapsed
                    : Visibility.Visible;
                EquipmentListView.SelectedItem = selectedItem;

            }

            else
            {
                Visibility = Visibility.Collapsed;
                
            }
        }

        private void UnitSizeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            if (DataContext != null)
            {
                UnitData.CurrentUnitSize = (int) UnitSizeSlider.Value;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (EquipmentText != null)
            {
                EquipmentText.Text = ((UnitEquipmentData) EquipmentListView.SelectedItem)?.EquipmentInfo.Equipment.Details ?? string.Empty;

                EquipmentTextScrollViewer.Visibility = string.IsNullOrEmpty(EquipmentText.Text) ? Visibility.Collapsed : Visibility.Visible;
                EquipmentListView.SetValue(Grid.RowSpanProperty, string.IsNullOrEmpty(EquipmentText.Text) ? 2 : 1);
            }
        }
    }

    public class ShowUnitSizeEditorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ((ArmyListData)value).UnitData.MaxUnitSize == ((ArmyListData)value).UnitData.MinUnitsize
    ? Visibility.Collapsed
    : Visibility.Visible : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}