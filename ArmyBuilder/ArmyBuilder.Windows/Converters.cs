using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;

namespace ArmyBuilder.Windows
{
    public class DetachmentDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            return element.FindResource("ForceOrgDetachmentTemplate") as DataTemplate;
            //if (element != null && item != null && item is DetachmentData)
            //{
            //    DetachmentData detachment = item as DetachmentData;

            //    switch (detachment.Detachment.Type)
            //    {
            //        case DetachmentType.UnitDetachment:
            //            return element.FindResource("UnitDetachmentTemplate") as DataTemplate;
            //        case DetachmentType.BattleForged:
            //            return element.FindResource("BattleForgedDetachmentTemplate") as DataTemplate;
            //        case DetachmentType.ForceOrgDetachment:
            //            return element.FindResource("ForceOrgDetachmentTemplate") as DataTemplate;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //}




            return null;
        }
    }

    public class ModelDataGroupToNumColumnsConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 1;
            }

            int val = (int) value;

            if (val >= 20)
            {
                return 5;
            }

            if (val >= 9)
            {
                return 3;
            }

            if (val >= 4)
            {
                return 2;
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DetachmentRequirementsToTextColorConverter : System.Windows.Data.IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || !(values[0] is DetachmentRequirementData))
            {
                return new SolidColorBrush(Colors.Black);
            }

            var data = (DetachmentRequirementData)values[0];

            if (data.SlotsUsed < data.Requirement.Minimum)
            {
                return new SolidColorBrush(Colors.Red);
            }

            else if (data.SlotsUsed > data.Requirement.Maximum)
            {
                return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ForceOrgGroupToText : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((CollectionViewGroup) value).Name;
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
            var model = (Unit)value;

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
            var model = (Unit)value;

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
            if (parameter == null)
            {



                switch ((ForceOrgSlot) (int) value)
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
                    case ForceOrgSlot.Fortification:
                        return "Fortification";
                    case ForceOrgSlot.Flyer:
                        return "Flyer";
                    case ForceOrgSlot.DedicatedTransport:
                        return "Dedicated Transport";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }

            else
            {
                switch ((ForceOrgSlot)(int)value)
                {
                    case ForceOrgSlot.HQ:
                        return new BitmapImage(new Uri("Images/hq.JPG"));
                    case ForceOrgSlot.Troop:
                        return new BitmapImage(new Uri("Images/troop.JPG"));
                    case ForceOrgSlot.Elite:
                        return new BitmapImage(new Uri("Images/elite.JPG"));
                    case ForceOrgSlot.FastAttack:
                        return new BitmapImage(new Uri("Images/fast.JPG"));
                    case ForceOrgSlot.HeavySupport:
                        return new BitmapImage(new Uri("Images/heavy.JPG"));
                    case ForceOrgSlot.LordOfWar:
                        return new BitmapImage(new Uri("Images/lord.JPG"));
                    case ForceOrgSlot.Fortification:
                        return new BitmapImage(new Uri("Images/fort.JPG"));
                    case ForceOrgSlot.Flyer:
                        return new BitmapImage(new Uri("Images/flyer.JPG"));
                    case ForceOrgSlot.DedicatedTransport:
                        return new BitmapImage(new Uri("Images/transport.JPG"));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
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

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentCostDifferenceVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int))
            {
                return string.Empty;
            }

            if (((int)value) < 0)
            {
                return $"({value})";
            }

            return $"(+{value})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentCostDifferenceColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || !(value is int))
            {
                return new SolidColorBrush(Colors.Black);
            }

            if(((int)value) < 0)
            {
                return new SolidColorBrush(Colors.Green);
            }

            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ModelsColumnSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 1;
            }

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
            if (value == null)
            {
                return Visibility.Visible;
            }

            IList f = (IList)value;

            return f.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DetachmentListCountToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }

            IList f = (IList)value;

            return f.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            //if (element != null && item != null && item is DetachmentData)
            //{
            //    DetachmentData detachment = item as DetachmentData;

            //    switch (detachment.Detachment.Type)
            //    {
            //        case DetachmentType.UnitDetachment:
            //            return element.FindResource("UnitDetachmentTemplate") as DataTemplate;
            //        case DetachmentType.BattleForged:
            //            return element.FindResource("BattleForgedDetachmentTemplate") as DataTemplate;
            //        case DetachmentType.ForceOrgDetachment:
            //            return element.FindResource("ForceOrgDetachmentTemplate") as DataTemplate;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //}

            switch (((EquipmentData)item).Equipment.Type)
            {
                case EquipmentType.Normal:
                    return element.FindResource("EquipmentNormalTemplate") as DataTemplate;
                case EquipmentType.Upgrade:
                    return element.FindResource("EquipmentUpgradeTemplate") as DataTemplate;
                default:
                    throw new ArgumentException();
            }

            return null;
        }
    }
}
