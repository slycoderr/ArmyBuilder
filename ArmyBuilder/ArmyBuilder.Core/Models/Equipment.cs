using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{

    [XmlRoot("Equipment", Namespace = "")]
    public class Equipment : BindableBase
    {
        private int cost;
        private int limit;
        private int maxPerArmy;
        private bool isDefault;
        private bool isGiven;
        private int mutualId;
        private EquipmentType type;
        private int perX;
        private int id;
        private string name;
        private string groupName;

        [XmlAttribute]
        public int Id { get => id; set => SetValue(ref id, value); }

        [XmlAttribute]
        public string Name { get => name; set => SetValue(ref name, value); }

        [XmlAttribute]
        public int Cost { get => cost; set => SetValue(ref cost, value); }

        [XmlAttribute]
        public int Limit { get => limit; set => SetValue(ref limit, value); }

        [XmlAttribute]
        public int MaxPerArmy { get => maxPerArmy; set => SetValue(ref maxPerArmy, value); }

        [XmlAttribute]
        public bool IsDefault { get => isDefault; set => SetValue(ref isDefault, value); }

        [XmlAttribute]
        public bool IsGiven { get => isGiven; set => SetValue(ref isGiven, value); }

        [XmlAttribute]
        public int MutualId { get => mutualId; set => SetValue(ref mutualId, value); }

        [XmlAttribute]
        public EquipmentType Type { get => type; set => SetValue(ref type, value); }

        [XmlAttribute]
        public int PerX { get => perX; set => SetValue(ref perX, value); }

        /// <summary>
        /// todo: remove this. too annoying in the xml
        /// </summary>
        [XmlAttribute]
        public string GroupName { get => groupName; set => SetValue(ref groupName, value); }

        [XmlArray]
        public ObservableCollection<Equipment> ReplacementOptions { get; set; } = new ObservableCollection<Equipment>();

        [XmlIgnore]
        public Unit Unit { get; set; }

        public Equipment Clone()
        {
            var clone = (Equipment)MemberwiseClone();

            clone.ReplacementOptions = new ObservableCollection<Equipment>();

            return clone;
        }

        public void PopulateDefaultPoperties(Equipment defaultEntry)
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = defaultEntry.Name;
            }

            if (Cost == 0)
            {
                Cost = defaultEntry.Cost;
            }

            if (Limit == 0)
            {
                Limit = defaultEntry.Limit;
            }

            if (MaxPerArmy == 0)
            {
                MaxPerArmy = defaultEntry.MaxPerArmy;
            }

            if (!IsDefault)
            {
                IsDefault = defaultEntry.IsDefault;
            }

            if (!IsGiven)
            {
                IsGiven = defaultEntry.IsGiven;
            }

            if (Type == 0)
            {
                Type = defaultEntry.Type;
            }

            if (PerX == 0)
            {
                PerX = defaultEntry.PerX;
            }

            foreach (var equip in ReplacementOptions)
            {
                equip.PopulateDefaultPoperties(Unit.UnitEntry.Army.EquipmentDefinitions.First(e=>e.Id == equip.Id));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}