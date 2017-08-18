using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models.Utility;

namespace ArmyBuilder.XMLEditor.Models
{
    public class MassUnitEntry :BindableBase
    {
        private string name = "New Entry";
        private int minUnitSize = 1;
        private int maxUnitSize = 1;
        private int costPerModel;
        private ForceOrgSlot forceOrgSlot = ForceOrgSlot.HQ;
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        public int MinUnitSize { get { return minUnitSize; } set { SetValue(ref minUnitSize, value); } }

        public int MaxUnitSize { get { return maxUnitSize; } set { SetValue(ref maxUnitSize, value); } }

        public int CostPerModel { get { return costPerModel; } set { SetValue(ref costPerModel, value); } }

        public ForceOrgSlot ForceOrgSlot { get { return forceOrgSlot; } set { SetValue(ref forceOrgSlot, value); } }
    }

    public class MassUnitEntryCollection : ObservableCollection<MassUnitEntry>
    {
        
    }

    public class MassEquipmentEntry : BindableBase
    {
        private string name = "New Entry";
        private int cost;
        private EquipmentType type = EquipmentType.Normal;
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        public int Cost { get { return cost; } set { SetValue(ref cost, value); } }

        public EquipmentType Type { get { return type; } set { SetValue(ref type, value); } }
    }

    public class MassEquipmentEntryCollection : ObservableCollection<MassEquipmentEntry>
    {

    }
}
