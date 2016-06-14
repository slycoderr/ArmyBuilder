using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class EquipmentData : BindableBase
    {
        [XmlIgnore]
        public Equipment Equipment { get; set; }

        [XmlIgnore]
        public EquipmentData ParentEquipment { get; private set; }

        [XmlAttribute]
        public int EquipmentId { get; set; }

        [XmlArray]
        public List<EquipmentData> ReplacementOptions { get; set; } = new List<EquipmentData>();

        [XmlArray]
        public List<EquipmentData> GivenEquipment { get; set; } = new List<EquipmentData>();

        [XmlIgnore]
        public bool CanAdd { get { return canAdd; } set { SetValue(ref canAdd, value); } }

        [XmlAttribute]
        public bool IsTaken
        {
            get { return isTaken; }
            set
            {
                if (SetValue(ref isTaken, value))
                {
                    TraverseEquipment(this);
                }
            }
        }

        [XmlAttribute]
        public string GroupName { get { return groupName; } set { SetValue(ref groupName, value); } }

        [XmlIgnore]
        public int PointsCostTotal { get { return pointsCostTotal; } set { SetValue(ref pointsCostTotal, value); } }

        private bool canAdd;
        private string groupName;
        private bool isTaken;

        private int pointsCostTotal;

        public EquipmentData()
        {
        }

        public EquipmentData(Equipment e, string id, EquipmentData parent)
        {
            Equipment = e;
            GroupName = (Equipment.GroupName ?? Equipment.Name) + id;
            ParentEquipment = parent;
            Equipment.TempLimit = Equipment.Limit;

            EquipmentId = e.Id;

            ReplacementOptions = Equipment.ReplacementOptions.Select(eq => new EquipmentData(eq, id, this)).ToList();
            GivenEquipment = Equipment.GivenEquipment.Select(eq => new EquipmentData(eq, id, this)).ToList();
            IsTaken = Equipment.IsDefault || ((ParentEquipment?.IsTaken ?? false) && Equipment.IsGiven);

            TraverseEquipment(this);
        }

        public EquipmentData(Equipment e, EquipmentData parent)
        {
            Equipment = e;
            ParentEquipment = parent;
            Equipment.TempLimit = Equipment.Limit;

            EquipmentId = e.Id;
            ReplacementOptions = Equipment.ReplacementOptions.Select(eq => new EquipmentData(eq, this)).ToList();
            GivenEquipment = Equipment.GivenEquipment.Select(eq => new EquipmentData(eq, this)).ToList();

            IsTaken = Equipment.IsDefault || ((ParentEquipment?.IsTaken ?? false) && Equipment.IsGiven);
            TraverseEquipment(this);
        }

        public void SetData(Equipment e, EquipmentData parent)
        {
            Equipment = e;
            ParentEquipment = parent;
            Equipment.TempLimit = Equipment.Limit;

            ReplacementOptions.ForEach(i => i.SetData(Equipment.ReplacementOptions.Single(ii => ii.Id == i.EquipmentId), this));
            GivenEquipment.ForEach(i => i.SetData(Equipment.GivenEquipment.Single(ii => ii.Id == i.EquipmentId), this));
            TraverseEquipment(this);
        }

        public void TraverseEquipment(EquipmentData equipment)
        {
            if (equipment.IsTaken || equipment.ReplacementOptions.Any(e => e.IsTaken))
            {
                equipment.CanAdd = true;
                equipment.ReplacementOptions.ForEach(e => e.CanAdd = true);
                equipment.GivenEquipment.ForEach(e => e.CanAdd = true);
                equipment.GivenEquipment.Where(e => e.Equipment.IsGiven).ForEach(e => e.IsTaken = true);
            }

            else
            {
                equipment.CanAdd = equipment.ParentEquipment == null || (ParentEquipment != null && (equipment.ParentEquipment.IsTaken || equipment.ParentEquipment.ReplacementOptions.Any(e => e.IsTaken))); //top most equipment can never be not taken

                equipment.ReplacementOptions.ForEach(e =>
                {
                    e.CanAdd = false;
                    e.IsTaken = false;
                    TraverseEquipment(e);
                });

                equipment.GivenEquipment.ForEach(e =>
                {
                    e.CanAdd = false;
                    e.IsTaken = false;
                    TraverseEquipment(e);
                });
            }
        }
    }
}