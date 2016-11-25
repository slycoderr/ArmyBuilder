using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Unit", Namespace = "")]
    public class Unit : BindableBase
    {
        [XmlAttribute]
        public int Id
        {
            get { return id; }
            set { SetValue(ref id, value); }
        }

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }

        [XmlAttribute]
        public uint Minimum
        {
            get { return minimum; }
            set { SetValue(ref minimum, value); }
        }

        [XmlAttribute]
        public uint Maximum
        {
            get { return maximum; }
            set { SetValue(ref maximum, value); }
        }

        [XmlAttribute]
        public uint IncrementCost
        {
            get { return incrementCost; }
            set { SetValue(ref incrementCost, value); }
        }

        [XmlAttribute]
        public uint IncrementSize
        {
            get { return incrementSize; }
            set { SetValue(ref incrementSize, value); }
        }

        [XmlIgnore]
        public uint CostPerModel => IncrementCost / (IncrementSize > 0 ? IncrementSize : 1);

        [XmlAttribute]
        public uint BaseCost
        {
            get { return baseCost; }
            set { SetValue(ref baseCost, value); }
        }

        /// <summary>
        ///     Needed for loading since transports don't know what unit entry they're from.
        /// </summary>
        [XmlAttribute]
        public int UnitEntryId { get; set; }

        [XmlArray]
        public ObservableCollection<Equipment> DefaultEquipment { get; set; } = new ObservableCollection<Equipment>();

        [XmlArray]
        public ObservableCollection<Equipment> Upgrades { get; set; } = new ObservableCollection<Equipment>();

        [XmlIgnore]
        public UnitEntry UnitEntry { get; set; }

        private uint baseCost;
        private int id;
        private uint incrementCost;
        private uint incrementSize;
        private uint maximum;
        private uint minimum;
        private string name;

        public override string ToString()
        {
            return Name;
        }
    }
}