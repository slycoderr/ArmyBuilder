using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Windows.Utility
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitSizeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = (Model)value;

            return model.Maximum == model.Minimum ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitCompositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = (Model)value;

            return model.Maximum == model.Minimum ? $"{model.Minimum} {model.Name}(s)" : $"{model.Minimum} - {model.Maximum} {model.Name}(s)";
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
            switch ((ForceOrgSlot)(int)value)
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
                    return "Lord Of War";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() != "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }

    public class ModelsColumnSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<ArmyListData>)
            {
                return ((List<ArmyListData>)value).Count > 0 ? 1 : 2;
            }

            return ((List<ModelData>)value).Count > 0 ? 1 : 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListCountToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<ArmyListData>)
            {
                return ((List<ArmyListData>)value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is List<EquipmentData>)
            {
                return ((List<EquipmentData>)value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return ((List<ModelData>)value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class UnitEditorVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ArmyListViewModel)value)?.SelectedUnit != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EquipmentUpgradeTemplate { get; set; }

        public DataTemplate EquipmentNormalTemplate { get; set; }

        

        //protected override DataTemplate SelectTemplateCore(object item)
        //{
        //    switch ((EquipmentType)((EquipmentData)item).Equipment.Type)
        //    {
        //        case EquipmentType.Normal:
        //            return EquipmentNormalTemplate;
        //        case EquipmentType.Upgrade:
        //            return EquipmentUpgradeTemplate;
        //        default:
        //            throw new ArgumentException();
        //    }
        //}
    }

    public class ArmyTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "1":
                    {
                        return "Space Wolves";
                    }
                case "2":
                    {
                        return "Necrons";
                    }

                default:
                    throw new ArgumentException("army id not found in switch");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
