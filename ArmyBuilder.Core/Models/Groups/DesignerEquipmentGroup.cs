using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArmyBuilder.Core.Models.Groups
{
    public class DesignerEquipmentGroup : ObservableCollection<Equipment>
    {
        public string DesignerGroupId { get; set; }

        public DesignerEquipmentGroup(List<Equipment> items) : base(items.OrderBy(i => i.Name))
        {
            //DesignerGroupId = items.ToList().First().DesignerGroup;
        }
    }
}