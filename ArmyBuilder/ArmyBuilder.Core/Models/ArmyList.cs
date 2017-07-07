using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ArmyList : BindableBase
    {
        private string name;
        private int pointsLimit;
        private Army army;

        [XmlIgnore]
        public Army Army { get => army; set => SetValue(ref army, value); }

        [XmlAttribute]
        public Guid Id { get; set; }
        [XmlAttribute]
        public int ArmyId { get; set; }
        [XmlAttribute]
        public string Name { get => name; set => SetValue(ref name, value); }

        [XmlArray]
        public ObservableCollection<DetachmentData> Detachments { get; } = new ObservableCollection<DetachmentData>();

        [XmlAttribute]
        public int PointsLimit { get => pointsLimit; set => SetValue(ref pointsLimit, value); }

        public ArmyList(Army army)
        {
            Army = army;
        }

        public ArmyList() { }

        public ArmyList(string newName, int points, Army newArmy, Detachment detach)
        {
            Name = newName;
            PointsLimit = points;
            Army = newArmy;
            ArmyId = newArmy.Id;
            Id = Guid.NewGuid();

        }
    }
}
