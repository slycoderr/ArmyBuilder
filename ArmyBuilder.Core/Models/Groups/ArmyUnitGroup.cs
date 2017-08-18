using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArmyBuilder.Core.Models.Groups
{
    public class ArmyUnitGroup : ObservableCollection<UnitEntry>
    {
        public ForceOrgSlot ForceOrgId { get; set; }

        public ArmyUnitGroup(List<UnitEntry> items) : base(items.OrderBy(u=>u.Name))
        {
            ForceOrgId = items.First().ForceOrgSlot;
        }
    }
}