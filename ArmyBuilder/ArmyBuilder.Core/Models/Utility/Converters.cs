using System;
using System.Collections.Generic;

namespace ArmyBuilder.Core.Utility
{
    public class Converters
    {
        public static string UnitRuleToString(UnitRuleType type, List<int> args)
        {
            switch (type)
            {
                case UnitRuleType.None:
                    return null;
                case UnitRuleType.DependsOn:
                    return ("<PUR="+string.Join(",", args)+">").Replace(",>", ">");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string AffectsRuleToString(AffectRule type, List<int> args)
        {
            switch (type)
            {
                case AffectRule.None:
                    return null;
                case AffectRule.SlotChange:
                    return ("<SC=" + string.Join(",", args) + ">").Replace(",>", ">");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        //public static string EquipmentRuleToString(EquipmentType type, List<int> args)
        //{
        //    switch (type)
        //    {
        //        case EquipmentType.AnyCanTake:
        //            return "<PMR=ANY>";
        //        case EquipmentType.AllMustTake:
        //            return "<PMR=WHL>";
        //        case EquipmentType.PerNumberCanTake:
        //            return ("<PMR=PER" + string.Join(",", args) + ">").Replace(",>", ">");
        //        case EquipmentType.UnitMustHaveMinSize:
        //            return ("<PMR=MIN" + string.Join(",", args) + ">").Replace(",>", ">");
        //        case EquipmentType.UnitMustNotExceedMaxSize:
        //            return ("<PMR=MAX" + string.Join(",", args) + ">").Replace(",>", ">"); 
        //        case EquipmentType.Upgrade:
        //            return "<PMR=UPG>";
        //        case EquipmentType.DependantOn:
        //            return ("<PMR=DEP" + string.Join(",", args) + ">").Replace(",>", ">");
        //        case EquipmentType.MutuallyExclusive:
        //            return ("<PMR=MUE" + string.Join(",", args) + ">").Replace(",>", ">"); ;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        //    }
        //}

        public static string EquipmentRuleToString(EquipmentType type, List<string> args)
        {
            if (args == null)
            {
                args = new List<string>();
            }

            switch (type)
            {
                case EquipmentType.AnyCanTake:
                    return "<PMR=ANY>";
                case EquipmentType.AllMustTake:
                    return "<PMR=WHL>";
                case EquipmentType.PerNumberCanTake:
                    return ("<PMR=PER" + string.Join(",", args) + ">").Replace(",>", ">");
                case EquipmentType.UnitMustHaveMinSize:
                    return ("<PMR=MIN" + string.Join(",", args) + ">").Replace(",>", ">");
                case EquipmentType.UnitMustNotExceedMaxSize:
                    return ("<PMR=MAX" + string.Join(",", args) + ">").Replace(",>", ">");
                case EquipmentType.Upgrade:
                    return "<PMR=UPG>";
                case EquipmentType.DependantOn:
                    return ("<PMR=DEP" + string.Join(",", args) + ">").Replace(",>", ">");
                case EquipmentType.MutuallyExclusive:
                    return ("<PMR=MUE" + string.Join(",", args) + ">").Replace(",>", ">");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string ReplacesRuleToString(ReplacesRule type, List<int> args)
        {
            switch (type)
            {
                case ReplacesRule.None:
                    return "<R=-1>";
                case ReplacesRule.Default:
                    return ("<R=" + string.Join(",", args) + ">").Replace(",>", ">");
                case ReplacesRule.ReplacesAll:
                    return ("<RALL=" + string.Join(",", args) + ">").Replace(",>", ">");
                case ReplacesRule.ReplacesAny:
                    return ("<RANY=" + string.Join(",", args) + ">").Replace(",>", ">");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
