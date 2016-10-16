using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyBuilder.Core.Models.Utility
{
    class Util
    {
        public static List<EquipmentData> GetAllEquipmentSubEquipment(EquipmentData e)
        {
            var allEquip = new List<EquipmentData>();

            allEquip.AddRange(e.GivenEquipment);
            allEquip.AddRange(e.ReplacementOptions);

            foreach (var ee in allEquip.ToList())
            {
                allEquip.AddRange(GetAllEquipmentSubEquipment(ee));
            }

            return allEquip;
        }

        public static List<EquipmentData> GetAllGivenSubEquipment(EquipmentData e)
        {
            var allEquip = new List<EquipmentData>();

            allEquip.AddRange(e.GivenEquipment);

            foreach (var ee in allEquip.ToList())
            {
                allEquip.AddRange(GetAllGivenSubEquipment(ee));
            }

            return allEquip;
        }

        public static List<Equipment> GetAllEquipmentSubEquipment(Equipment e)
        {
            var allEquip = new List<Equipment>();

            allEquip.AddRange(e.ReplacementOptions);

            foreach (var ee in allEquip.ToList())
            {
                allEquip.AddRange(GetAllEquipmentSubEquipment(ee));
            }

            return allEquip;
        }
    }
}
