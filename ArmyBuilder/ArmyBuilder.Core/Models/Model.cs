using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Model", Namespace = "")]
    public class Model
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Minimum { get; set; }

        [XmlAttribute]
        public int Maximum { get; set; }

        [XmlAttribute]
        public int IncrementCost { get; set; }

        [XmlAttribute]
        public int BaseCost { get; set; }

        [XmlArray]
        public List<Equipment> DefaultEquipment { get; set; }

        [XmlArray]
        public List<Equipment> Upgrades { get; set; }
    }
}