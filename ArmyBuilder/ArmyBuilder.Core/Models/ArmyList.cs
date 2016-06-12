using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyBuilder.Core.Database;
using Slycoder.MVVM;
using SQLite;

namespace ArmyBuilder.Core.Models
{
    [UserData]
    public class ArmyList : BindableBase
    {
        private string name;
        private int pointsLimit;
        private Army army;

        [Ignore]
        public Army Army { get { return army; } set { SetValue(ref army, value); } }

        public Guid Id { get; set; }

        public int ArmyId { get; set; }
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        public int PointsLimit { get { return pointsLimit; } set { SetValue(ref pointsLimit, value); } }

        public ArmyList(Army army)
        {
            Army = army;
        }

        public ArmyList() { }

        public ArmyList(string newName, int points, Army newArmy)
        {
            Name = newName;
            PointsLimit = points;
            Army = newArmy;
            ArmyId = newArmy.Id;
            Id = Guid.NewGuid();
        }
    }
}
