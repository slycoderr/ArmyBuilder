using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;

namespace ArmyBuilder.Core.Utility
{
    public class EquipmentListUpdatedEventArgs : EventArgs
    {
        public List<DesignerEquipmentGroup> Groups { get; }

        public EquipmentListUpdatedEventArgs(List<DesignerEquipmentGroup> g)
        {
            Groups = g;
        }
    }
}
