using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;
using ArmyBuilder.Core.ViewModels;
using ArmyBuilder.Utility;

namespace ArmyBuilder.Templates
{
    public sealed partial class EquipmentDataTemplate : ResourceDictionary
    {
        public EquipmentDataTemplate()
        {
            InitializeComponent();
        }

        private void WholeRuleCheckBox_Checked(object sender, RoutedEventArgs routedEventArgs)
        {
            var mainViewModel = (MainViewModel) Application.Current.Resources["MainViewModel"];
            var equip = (UnitEquipmentData) ((CheckBox) sender).DataContext;

            equip.Count = mainViewModel.CurrentArmyListViewModel.SelectedUnit.CurrentUnitSize;
        }

        private void WholeRuleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var equip = (UnitEquipmentData) ((CheckBox) sender).DataContext;

            equip.Count = 0;
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            ((UnitEquipmentData) ((Button) sender).DataContext).Count++;
        }

        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            ((UnitEquipmentData) ((Button) sender).DataContext).Count--;
        }

        private void MinIncrementAddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var equip = (UnitEquipmentData) ((Button) sender).DataContext;

            equip.Count = 1;
        }

        private void MinDecrementButton_OnClick(object sender, RoutedEventArgs e)
        {
            var equip = (UnitEquipmentData) ((Button) sender).DataContext;

            equip.Count = 0;
        }

        private void CheckBox_DataContextChanged(object o, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var cb = (CheckBox) o;

            if (cb.DataContext != null)
            {
                cb.Checked -= WholeRuleCheckBox_Checked;
                cb.Unchecked -= WholeRuleCheckBox_Unchecked;
                cb.IsChecked = (cb.DataContext as UnitEquipmentData).Count > 0;
                cb.Checked += WholeRuleCheckBox_Checked;
                cb.Unchecked += WholeRuleCheckBox_Unchecked;
            }
        }
    }

    public class ShowWholeControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EquipmentType)value)
            {
                case EquipmentType.UnitMustHaveMinSize:
                    return Visibility.Collapsed;
                case EquipmentType.UnitMustNotExceedMaxSize:
                    return Visibility.Collapsed;
                case EquipmentType.Upgrade:
                    return Visibility.Collapsed;
                case EquipmentType.AnyCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.AllMustTake:
                    return Visibility.Visible;
                case EquipmentType.PerNumberCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.DependantOn:
                    return Visibility.Collapsed;
                case EquipmentType.MutuallyExclusive:
                    return Visibility.Collapsed;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowAnyControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EquipmentType) value)
            {
                case EquipmentType.UnitMustHaveMinSize:
                    return Visibility.Collapsed;
                case EquipmentType.UnitMustNotExceedMaxSize:
                    return Visibility.Collapsed;
                case EquipmentType.Upgrade:
                    return Visibility.Collapsed;
                case EquipmentType.AnyCanTake:
                    return Visibility.Visible;
                case EquipmentType.AllMustTake:
                    return Visibility.Collapsed;
                case EquipmentType.PerNumberCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.DependantOn:
                    return Visibility.Collapsed;
                case EquipmentType.MutuallyExclusive:
                    return Visibility.Visible;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowPerXControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EquipmentType) value)
            {
                case EquipmentType.UnitMustHaveMinSize:
                    return Visibility.Collapsed;
                case EquipmentType.UnitMustNotExceedMaxSize:
                    return Visibility.Collapsed;
                case EquipmentType.Upgrade:
                    return Visibility.Collapsed;
                case EquipmentType.AnyCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.AllMustTake:
                    return Visibility.Collapsed;
                case EquipmentType.PerNumberCanTake:
                    return Visibility.Visible;
                case EquipmentType.DependantOn:
                    return Visibility.Collapsed;
                case EquipmentType.MutuallyExclusive:
                    return Visibility.Collapsed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowMinControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EquipmentType) value)
            {
                case EquipmentType.UnitMustHaveMinSize:
                    return Visibility.Visible;
                case EquipmentType.UnitMustNotExceedMaxSize:
                    return Visibility.Visible;
                case EquipmentType.Upgrade:
                    return Visibility.Collapsed;
                case EquipmentType.AnyCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.AllMustTake:
                    return Visibility.Collapsed;
                case EquipmentType.PerNumberCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.DependantOn:
                    return Visibility.Collapsed;
                case EquipmentType.MutuallyExclusive:
                    return Visibility.Collapsed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowUpgradeControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EquipmentType) value)
            {
                case EquipmentType.UnitMustHaveMinSize:
                    return Visibility.Collapsed;
                case EquipmentType.UnitMustNotExceedMaxSize:
                    return Visibility.Collapsed;
                case EquipmentType.Upgrade:
                    return Visibility.Visible;
                case EquipmentType.AnyCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.AllMustTake:
                    return Visibility.Collapsed;
                case EquipmentType.PerNumberCanTake:
                    return Visibility.Collapsed;
                case EquipmentType.DependantOn:
                    return Visibility.Visible;
                case EquipmentType.MutuallyExclusive:
                    return Visibility.Collapsed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentGroupPreTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var group = value as UnitEquipmentGroup;

            if (group.GroupFlag == GroupFlag.ByReplaceId)
            {
                var equip = group.First().EquipmentInfo;

                switch (equip.Profile.EquipmentType)
                {
                    case EquipmentType.MutuallyExclusive:
                        return "May take one of the following upgrades";
                    case EquipmentType.UnitMustNotExceedMaxSize:
                        return "May take any of the following upgrades";
                    case EquipmentType.UnitMustHaveMinSize:
                        return "May take any of the following upgrades";
                    //return "May take the following upgrades if the unit size is at least " + equip.CurrentProfile.EquipmentTypeNumbers + " models";
                    case EquipmentType.Upgrade:
                        return "May take any of the following upgrades";
                    case EquipmentType.AnyCanTake:
                        return "May replace";
                    case EquipmentType.AllMustTake:
                        return "The entire unit may take any combination of the following";
                    case EquipmentType.PerNumberCanTake:
                        return "May replace";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }

            else if (group.GroupFlag == GroupFlag.ByDependId)
            {
                return "If a";
            }

            else if (group.GroupFlag == GroupFlag.ByDependantIdAndReplaceId)
            {
                return "If a";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentGroupPostTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var group = value as UnitEquipmentGroup;

            if (group.GroupFlag == GroupFlag.ByReplaceId)
            {
                var equip = group.First().EquipmentInfo;
                var numOf = group.First().Unit.UnitData.DefaultEquipmentIdList.Count(i => group.GroupIdList.Contains(i.ProfileId));

                switch (equip.Profile.EquipmentType)
                {
                    case EquipmentType.MutuallyExclusive:
                        return "with any " + numOf + " of the following";
                    case EquipmentType.UnitMustHaveMinSize:
                        return "";
                    case EquipmentType.UnitMustNotExceedMaxSize:
                        return "";
                    case EquipmentType.Upgrade:
                        return "";
                    case EquipmentType.AnyCanTake:
                        return "with any " + numOf + " of the following";
                    case EquipmentType.AllMustTake:
                        return "";
                    case EquipmentType.PerNumberCanTake:
                        return "with any " + numOf + " of the following";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }

            else if (group.GroupFlag == GroupFlag.ByDependId)
            {
                return "is taken, it may take any of the following";
            }

            else if (group.GroupFlag == GroupFlag.ByDependantIdAndReplaceId)
            {
                if (group.GroupIdList.Contains(-1))
                {
                    return "is taken, it may take any of the following";
                }

                else
                {
                    var equipIdList = group.GroupIdList.ToList();

                    equipIdList.RemoveAt(0);

                    var mainViewModel = (MainViewModel) Application.Current.Resources["MainViewModel"];
                    var equipNames = !equipIdList.Contains(-1) ? string.Join(", ", equipIdList.Select(i => mainViewModel.FindEquipment(i).Equipment.Name)) : string.Empty;
                    var numOf = group.First().Unit.UnitData.DefaultEquipmentIdList.Count(i => group.GroupIdList.Contains(i.ProfileId));

                    return "is taken, it may replace it's " + equipNames + " with any " + numOf + " of the following";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentGroupNameTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var group = value as UnitEquipmentGroup;
            var mainViewModel = (MainViewModel) Application.Current.Resources["MainViewModel"];

            if (group.GroupFlag == GroupFlag.ByReplaceId)
            {
                string seperator = ", ";


                return !group.GroupIdList.Contains(-1) ? string.Join(", ", group.GroupIdList.Select(i => mainViewModel.FindEquipment(i).Equipment.Name)) : string.Empty;
            }

            else if (group.GroupFlag == GroupFlag.ByDependId)
            {
                return mainViewModel.FindEquipment(group.GroupIdList.First()).Equipment.Name;
            }

            else if (group.GroupFlag == GroupFlag.ByDependantIdAndReplaceId)
            {
                return mainViewModel.FindEquipment(group.GroupIdList.First()).Equipment.Name;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ForceOrgTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var forceOrgSlot = (ForceOrgSlot) int.Parse(value.ToString());

            switch (forceOrgSlot)
            {
                case ForceOrgSlot.HQ:
                    return "HQ";
                case ForceOrgSlot.Troop:
                    return "Troop";
                case ForceOrgSlot.Elite:
                    return "Elite";
                case ForceOrgSlot.FastAttack:
                    return "Fast Attack";
                case ForceOrgSlot.HeavySupport:
                    return "Heavy Support";
                case ForceOrgSlot.LordOfWar:
                    return "Lord of War";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ArmyTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var army = int.Parse(value.ToString());

            return MainViewModel.Current.Armies.Single(a => a.Id == army);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}