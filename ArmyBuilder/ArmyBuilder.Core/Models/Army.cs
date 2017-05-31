using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Army", Namespace = "")]
    public class Army : BindableBase
    {
        [XmlAttribute]
        public int Id { get => id; set => SetValue(ref id, value); }

        [XmlAttribute]
        public string Name { get => name; set => SetValue(ref name, value); }
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
        public ObservableCollection<UnitEntry> UnitEntries { get; set; } = new ObservableCollection<UnitEntry>();

        [XmlArray]
        public ObservableCollection<Detachment> Detachments { get; set; } = new ObservableCollection<Detachment>();

        [XmlArray]
        public ObservableCollection<Equipment> EquipmentDefinitions { get; set; } = new ObservableCollection<Equipment>();

        public Army() { }

        public override string ToString()
        {
            return Name;
        }

        private int id;
        private string name;

        /// <summary>
        /// Populates all navigation properties and overrides default values; 
        /// </summary>
        public void Configure()
        {
            foreach(var detachment in Detachments)
            {
                detachment.Army = this;
            }

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
                        Utility.Util.GetAllEquipmentSubEquipment(equipment).ForEach(e => e.Unit = unit);
                        equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e=> e.Id == equipment.Id));
                    }

                    foreach (var equipment in unit.Upgrades)
                    {
                        equipment.Unit = unit;
                        Utility.Util.GetAllEquipmentSubEquipment(equipment).ForEach(e => e.Unit = unit);
                        equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e => e.Id == equipment.Id));
                    }
                }
            }

            //i have to do this after the fact so that when i set the transports UnitEntry reference, that reference to its Army isn't null.
            foreach (var unitEntry in UnitEntries)
            {
                //populate the dedicated transports
                foreach (var transport in unitEntry.DedicatedTransports)
                {
                    transport.UnitEntry = UnitEntries.First(u => u.Id == transport.UnitEntryId);

                    foreach (var equipment in transport.DefaultEquipment)
                    {
                        equipment.Unit = transport;
                        Utility.Util.GetAllEquipmentSubEquipment(equipment).ForEach(e => e.Unit = transport);
                        equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e => e.Id == equipment.Id));
                    }

                    foreach (var equipment in transport.Upgrades)
                    {
                        equipment.Unit = transport;
                        Utility.Util.GetAllEquipmentSubEquipment(equipment).ForEach(e => e.Unit = transport);
                        equipment.PopulateDefaultPoperties(EquipmentDefinitions.First(e => e.Id == equipment.Id));
                    }
                }
            }
        }
    }
}
