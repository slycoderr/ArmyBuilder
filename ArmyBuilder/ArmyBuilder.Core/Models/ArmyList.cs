using System;
using System.Collections.ObjectModel;
using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class ArmyList : BindableBase, IArmyList
    {
        public Guid Id { get; set; }

        public int ArmyId { get; set; }
        private Army army;
        private uint currentPointsTotal;
        private string name;
        private uint pointsLimit;
        private GameSystem system;

        public ArmyList(Army army)
        {
            Army = army;
        }

        public ArmyList()
        {
        }

        public ArmyList(string newName, uint points, Army newArmy, Detachment detach)
        {
            Name = newName;
            PointsLimit = points;
            Army = newArmy;
            ArmyId = newArmy.Id;
            Id = Guid.NewGuid();
        }

        public Army Army
        {
            get { return army; }
            set { SetValue(ref army, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }

        public GameSystem System
        {
            get { return system; }
            set { SetValue(ref system, value); }
        }

        public uint PointsLimit
        {
            get { return pointsLimit; }
            set { SetValue(ref pointsLimit, value); }
        }

        public uint CurrentPointsTotal
        {
            get { return currentPointsTotal; }
            set { SetValue(ref currentPointsTotal, value); }
        }

        public DateTime DateCreated { get; set; }
        public ObservableCollection<DetachmentData> Detachments { get; set; }
        public ObservableCollection<string> Data { get; set; }
    }
}