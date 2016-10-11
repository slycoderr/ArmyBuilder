using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{

    [XmlRoot("Equipment", Namespace = "")]
    public class Equipment
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Cost { get; set; }

        [XmlAttribute]
        public int Limit { get; set; }

        [XmlAttribute]
        public int NaxPerArmy { get; set; }

        [XmlAttribute]
        public bool IsDefault { get; set; }

        [XmlAttribute]
        public bool IsGiven { get; set; }

        [XmlAttribute]
        public int MutualId { get; set; }

        [XmlAttribute]
        public EquipmentType Type { get; set; }

        [XmlAttribute]
        public int PerX { get; set; }

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

            if (NaxPerArmy == 0)
            {
                NaxPerArmy = defaultEntry.NaxPerArmy;
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
    }
}