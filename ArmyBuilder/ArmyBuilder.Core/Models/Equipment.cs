using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    public enum EquipmentType
    {
        Normal,
        Upgrade = 1
    }

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
        public int TempLimit { get; set; }

        [XmlAttribute]
        public int NaxPerArmy { get; set; }

        [XmlAttribute]
        public bool IsDefault { get; set; }

        [XmlAttribute]
        public bool IsGiven { get; set; }

        [XmlAttribute]
        public int MutualId { get; set; }

        [XmlAttribute]
        public int Type { get; set; }

        [XmlAttribute]
        public int PerX { get; set; }

        [XmlAttribute]
        public string GroupName { get; set; }

        [XmlArray]
        public List<Equipment> ReplacementOptions { get; set; }

        [XmlArray]
        public List<Equipment> GivenEquipment { get; set; }
    }
}