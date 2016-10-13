using System.Collections.Generic;
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

        [XmlAttribute]
        public int Id { get { return id; } set { SetValue(ref id, value); } }

        [XmlAttribute]
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        [XmlAttribute]
        public int Cost { get { return cost; } set { SetValue(ref cost, value); } }

        [XmlAttribute]
        public int Limit { get { return limit; } set { SetValue(ref limit, value); } }

        [XmlAttribute]
        public int MaxPerArmy { get { return maxPerArmy; } set { SetValue(ref maxPerArmy, value); } }

        [XmlAttribute]
        public bool IsDefault { get { return isDefault; } set { SetValue(ref isDefault, value); } }

        [XmlAttribute]
        public bool IsGiven { get { return isGiven; } set { SetValue(ref isGiven, value); } }

        [XmlAttribute]
        public int MutualId { get { return mutualId; } set { SetValue(ref mutualId, value); } }

        [XmlAttribute]
        public EquipmentType Type { get { return type; } set { SetValue(ref type, value); } }

        [XmlAttribute]
        public int PerX { get { return perX; } set { SetValue(ref perX, value); } }

        ///// <summary>
        ///// todo: remove this. too annoying in the xml
        ///// </summary>
        //[XmlAttribute] 
        //public string GroupName { get; set; }

        [XmlArray]
        public List<Equipment> ReplacementOptions { get; set; }

        [XmlArray]
        public List<Equipment> GivenEquipment { get; set; }

        [XmlIgnore]
        public Unit Unit { get; set; }

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

            foreach (var equip in GivenEquipment)
            {
                equip.PopulateDefaultPoperties(Unit.UnitEntry.Army.EquipmentDefinitions.First(e => e.Id == equip.Id));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}