using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArmyBuilder.Core.Models.Groups
{
    public class ArmyUnitGroup : ObservableCollection<Unit>
    {
        public int ForceOrgId { get; set; }

        public ArmyUnitGroup(List<Unit> items) : base(items.OrderBy(u=>u.Name))
        {
            ForceOrgId = items.First().ForceOrgSlot;
        }
    }
}