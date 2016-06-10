using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArmyBuilder.Core.Models.Groups
{
    public class ArmyListGroup : ObservableCollection<ArmyList>
    {
        public int ArmyId { get; set; }

        public ArmyListGroup(List<ArmyList> items) : base(items.OrderBy(i => i.PointsLimit))
        {
            ArmyId = items.ToList().First().ArmyId;
        }
    }
}