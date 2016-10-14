using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Army", Namespace = "")]
    public class Army : BindableBase
    {
        [XmlAttribute]
        public int Id { get { return id; } set { SetValue(ref id, value); } }

        [XmlAttribute]
        public string Name { get { return name; } set { SetValue(ref name, value); } }
        /// <summary>
        /// Version of the XML Army data schema.
        /// </summary>
        [XmlAttribute]
        public string SchemaVersion { get; set; }
        /// <summary>
        /// Version of this army's data file.
        /// </summary>
        [XmlAttribute]
        public int Version { get; set; }


        [XmlArray]
        public ObservableCollection<UnitEntry> UnitEntries { get; set; }

        [XmlArray]
        public ObservableCollection<Detachment> Detachments { get; set; }

        [XmlArray]
        public ObservableCollection<Equipment> EquipmentDefinitions { get; set; }

        public Army() { }

        public override string ToString()
        {
            return Name;
        }

        public static IReadOnlyList<string> Armies = new List<string> {"Space Wolves", "Necrons", "Skitarii", "Imperial Knights", "Dark Eldar"};
        private int id;
        private string name;

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
