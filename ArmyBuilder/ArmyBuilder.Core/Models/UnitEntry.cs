using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("UnitEntry", Namespace = "")]
    public class UnitEntry : BindableBase
    {
        private int id;
        private string name;
        private ForceOrgSlot forceOrgSlot;
        private int maxUnitSize;

        [XmlAttribute]
        public int Id { get { return id; } set { SetValue(ref id, value); } }

        /// <summary>
        /// Used for validating a unit size
        /// </summary>
        [XmlAttribute]
        public int MaxUnitSize { get { return maxUnitSize; } set { SetValue(ref maxUnitSize, value); } }

        [XmlAttribute]
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        [XmlAttribute]
        public ForceOrgSlot ForceOrgSlot { get { return forceOrgSlot; } set { SetValue(ref forceOrgSlot, value); } }

        [XmlAttribute]
        public bool DoeNotCountsTowardForceOrg { get; set; }

        [XmlArray]
        public ObservableCollection<Unit> Units { get; set; } = new ObservableCollection<Unit>();

        [XmlIgnore]
        public Army Army { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}