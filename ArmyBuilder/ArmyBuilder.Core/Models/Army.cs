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
        public ObservableCollection<Unit> UnitEntries { get; set; } = new ObservableCollection<Unit>();

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

    }
}
