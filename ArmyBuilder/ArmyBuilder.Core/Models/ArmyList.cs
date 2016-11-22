using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slycoder.MVVM;
using SQLite;

namespace ArmyBuilder.Core.Models
{

    public class ArmyList : BindableBase, IArmyList
    {
        private string name;
        private uint pointsLimit;
        private Army army;
        private GameSystem system;

        [Ignore]
        public Army Army { get { return army; } set { SetValue(ref army, value); } }

        public Guid Id { get; set; }

        public int ArmyId { get; set; }
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        public GameSystem System
        {
            get { return system; }
            set { SetValue(ref system, value); }
        }

        public uint PointsLimit { get { return pointsLimit; } set { SetValue(ref pointsLimit, value); } }

        public ArmyList(Army army)
        {
            Army = army;
        }

        public ArmyList() { }

        public ArmyList(string newName, uint points, Army newArmy, Detachment detach)
        {
            Name = newName;
            PointsLimit = points;
            Army = newArmy;
            ArmyId = newArmy.Id;
            Id = Guid.NewGuid();

        }

        public DateTime DateCreated { get; set; }
        public ObservableCollection<DetachmentData> Detachments { get; set; }
    }
}
