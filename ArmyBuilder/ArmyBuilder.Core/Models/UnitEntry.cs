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
        public bool DoeNotCountsTowardForceOrg { get; set; }

        [XmlArray]
        public List<Unit> Units { get; set; }

        [XmlArray]
        public List<UnitEntry> DedicatedTransports { get; set; }

        [XmlIgnore]
        public Army Army { get; set; }

        public override string ToString()
        {
            return Name + " (" + Units.Sum(u => u.BaseCost) + " points)";
        }

        public void PopulateDefaultPoperties(UnitEntry defaultEntry)
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = defaultEntry.Name;
            }

            if (ForceOrgSlot == 0)
            {
                ForceOrgSlot = defaultEntry.ForceOrgSlot;
            }

            if (!DoeNotCountsTowardForceOrg) //if its the default
            {
                DoeNotCountsTowardForceOrg = defaultEntry.DoeNotCountsTowardForceOrg;
            }

            if (Units == null || !Units.Any())
            {
                Units = defaultEntry.Units;
            }

            if (DedicatedTransports == null || !DedicatedTransports.Any())
            {
                DedicatedTransports = defaultEntry.DedicatedTransports;
            }
        }
    }
}