using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Army", Namespace = "")]
    public class Army
    {
        [XmlAttribute]
        public int Id { get; set; }
        /// <summary>
        /// Version of the XML Army data schema.
        /// </summary>
        [XmlAttribute]
        public int SchemaVersion { get; set; }
        /// <summary>
        /// Version of this army's data file.
        /// </summary>
        [XmlAttribute]
        public int Version { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlArray]
        public List<Unit> Units { get; set; }

        [XmlArray]
        public List<Detachment> Detachments { get; set; }

        [XmlArray]
        public List<Equipment> EquipmentDefinitions { get; set; }

        public Army() { }

        public override string ToString()
        {
            return Name;
        }

        public static IReadOnlyList<string> Armies = new List<string> {"Space Wolves", "Necrons", "Skitarii", "Imperial Knights", "Dark Eldar"};
    }
}
