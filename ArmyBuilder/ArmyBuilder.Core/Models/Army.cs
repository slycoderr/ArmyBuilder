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
        public List<UnitEntry> UnitEntries { get; set; }

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

        /// <summary>
        /// Populates all navigation properties and overrides default values; 
        /// </summary>
        public void Configure()
        {
            foreach (var unitEntry in UnitEntries)
            {
                unitEntry.Army = this;

                //populate unit entries
                foreach (var unit in unitEntry.Units)
                {
                    unit.UnitEntry = unitEntry;

                    foreach (var equipment in unit.DefaultEquipment)
                    {
                        equipment.Unit = unit;
                        equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e=> e.Id == equipment.Id));
                    }

                    foreach (var equipment in unit.Upgrades)
                    {
                        equipment.Unit = unit;
                        equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e => e.Id == equipment.Id));
                    }
                }

                //populate the dedicated transports
                foreach (var transport in unitEntry.DedicatedTransports)
                {
                    transport.Army = this;
                    transport.PopulateDefaultPoperties(UnitEntries.First(u=>u.Id == transport.Id));

                    //populate unit entries
                    foreach (var unit in transport.Units)
                    {
                        unit.UnitEntry = transport;

                        foreach (var equipment in unit.DefaultEquipment)
                        {
                            equipment.Unit = unit;
                            equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e => e.Id == equipment.Id));
                        }

                        foreach (var equipment in unit.Upgrades)
                        {
                            equipment.Unit = unit;
                            equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e => e.Id == equipment.Id));
                        }
                    }
                }
            }
        }
    }
}
