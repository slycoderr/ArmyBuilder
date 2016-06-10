using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Unit", Namespace = "")]
    public class Unit
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public int ForceOrgSlot { get; set; }

        [XmlAttribute]
        public bool CountsTowardForceOrg { get; set; }

        [XmlArray]
        public List<Model> Models { get; set; }

        [XmlArray]
        public List<Model> DedicatedTransports { get; set; }

        public override string ToString()
        {
            return Name + " (" + Models.Sum(u => u.BaseCost) + " points)";
        }
    }
}