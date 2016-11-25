using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class ArmyListData : BindableBase

    {
        public Unit Unit { get; }
        public Army Army { get; }

        public uint Count { get; set; }

        public ArmyListData(Unit unit, Army army)
        {
            Army = army;
            Unit = unit;
        }
    }
}