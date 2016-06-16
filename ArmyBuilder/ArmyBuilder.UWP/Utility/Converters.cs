using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;
using EquipmentType = ArmyBuilder.Core.Models.EquipmentType;

namespace ArmyBuilder.Utility.Converters
{
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            if (parameter != null)
            {
                //var equipmentProfile = (EquipmentProfile)((ComboBox)parameter).DataContext;

               // ((ComboBox)parameter).SelectedIndex = equipmentProfile.Equipment.ProfileList.IndexOf(equipmentProfile.Profile);
            }

            return value;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            //var index = (int)value;

            //return profile.Equipment.ProfileList.IndexOf(profile.Profile);
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitSizeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            var model = (Model)value;

            return model.Maximum == model.Minimum ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    public class UnitCompositionConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            var model = (Model)value;

            return model.Maximum == model.Minimum ? $"{model.Minimum} {model.Name}(s)" : $"{model.Minimum} - {model.Maximum} {model.Name}(s)";
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ForceOrgTextConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
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

            public object ConvertBack(object value, Type type, object parameter, string language)
            {
                throw new NotImplementedException();
            }
    }

    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            return value.ToString() != "0";
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            return (bool) value ? 1 : 0;
        }
    }

    public class ModelsColumnSizeConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            if (value is List<ArmyListData>)
            {
                return ((List<ArmyListData>) value).Count > 0 ? 1 : 2;
            }

            return ((List<ModelData>) value).Count > 0 ? 1 : 2;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ListCountToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            if (value is List<ArmyListData>)
            {
                return ((List<ArmyListData>) value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is List<EquipmentData>)
            {
                return ((List<EquipmentData>)value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is List<Detachment>)
            {
                return ((List<Detachment>)value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return ((List<ModelData>) value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EquipmentUpgradeTemplate { get; set; }

        public DataTemplate EquipmentNormalTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch ((EquipmentType) ((EquipmentData) item).Equipment.Type)
            {
                case EquipmentType.Normal:
                    return EquipmentNormalTemplate;
                case EquipmentType.Upgrade:
                    return EquipmentUpgradeTemplate;
                default:
                    throw new ArgumentException();
            }
        }
    }

    public class ArmyTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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


                case "3":
                    {
                        return "Skitarii";
                    }
                case "4":
                    {
                        return "Imperial Knights";
                    }
                default:
                    throw new ArgumentException("army id not found in switch");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
