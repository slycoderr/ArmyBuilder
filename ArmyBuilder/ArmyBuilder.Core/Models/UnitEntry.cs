using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("UnitEntry", Namespace = "")]
    public class UnitEntry
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public ForceOrgSlot ForceOrgSlot { get; set; }

        [XmlAttribute]
        public bool CountsTowardForceOrg { get; set; }

        [XmlArray]
        public List<Unit> Units { get; set; }

        [XmlArray]
        public List<UnitEntry> DedicatedTransports { get; set; }

        public override string ToString()
        {
            return Name + " (" + Units.Sum(u => u.BaseCost) + " points)";
        }
    }
}