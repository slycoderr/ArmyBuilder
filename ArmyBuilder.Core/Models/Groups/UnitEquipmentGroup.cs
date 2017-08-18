using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArmyBuilder.Core.Models.Groups
{
    public enum GroupFlag
    {
        ByReplaceId,
        ByDependId,
        ByDependantIdAndReplaceId,
        ByMutualIdAndReplaceId
    }

    public class UnitEquipmentGroup : ObservableCollection<object>
    {
        public List<int> GroupIdList { get; set; }
        public GroupFlag GroupFlag { get; }

        public UnitEquipmentGroup(List<object> items, GroupFlag flag) 
        {
            //GroupFlag = flag;

            //if (GroupFlag == GroupFlag.ByDependId)
            //{
            //    GroupIdList = items.ToList().First().EquipmentInfo.Profile.EquipmentTypeNumbers.Select(int.Parse).ToList();
            //}

            //else if (GroupFlag == GroupFlag.ByReplaceId)
            //{
            //    GroupIdList = items.ToList().First().EquipmentInfo.Profile.ReplacesIdList.ToList();
            //}

            //else if (GroupFlag == GroupFlag.ByDependantIdAndReplaceId)
            //{
            //    GroupIdList = items.ToList().First().EquipmentInfo.Profile.EquipmentTypeNumbers.Select(int.Parse).ToList();
            //    GroupIdList.AddRange(items.ToList().First().EquipmentInfo.Profile.ReplacesIdList);
            //}

            //else if (GroupFlag == GroupFlag.ByMutualIdAndReplaceId)
            //{
            //    GroupIdList = items.ToList().First().EquipmentInfo.Profile.ReplacesIdList.ToList();
            //}
        }
    }
}