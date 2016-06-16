using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArmyBuilder.Core.Models.Groups
{
    public class ForceOrgGroup : ObservableCollection<ArmyListData>
    {
        public ForceOrgSlot ForceOrgId { get; set; }

        public ForceOrgGroup(List<ArmyListData> items) : base(items)
        {
            ForceOrgId = items.First().Unit.ForceOrgSlot;
        }
    }
}