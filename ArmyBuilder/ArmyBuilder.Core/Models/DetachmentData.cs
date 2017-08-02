using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class DetachmentData : BindableBase
    {
        [XmlAttribute]
        public int DetachmentId { get; set; }
        [XmlIgnore]
        public Detachment Detachment { get; private set; }
        [XmlArray]
        public ObservableCollection<DetachmentRequirementData> DetachmentRequirementData { get; } = new ObservableCollection<DetachmentRequirementData>();

        public DetachmentData()
        {
            
        }

        public DetachmentData(Detachment detachment)
        {
            DetachmentId = detachment.Id;
            Detachment = detachment;
            DetachmentRequirementData = new ObservableCollection<DetachmentRequirementData>(Detachment.Requirements.Select(r=> new DetachmentRequirementData(r, this)));
        }

        public void Initialize(Detachment detach)
        {
            Detachment = detach;
        }

        public override string ToString()
        {
            return Detachment.Name;
        }
    }
    [XmlRoot(Namespace = "")]
    public class DetachmentRequirementData : BindableBase
    {
        private int slotsUsed;

        public DetachmentRequirementData()
        {
        }

        [XmlIgnore]
        public int SlotsUsed { get => slotsUsed; set => SetValue(ref slotsUsed, value); }
        [XmlIgnore]
        public DetachmentRequirement Requirement { get; private set; }
        [XmlAttribute]
        public int RequirementId { get; set; }
        [XmlIgnore]
        public DetachmentData DetachmentData { get; }
        [XmlArray]
        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();

        public void Initialize(DetachmentRequirement req)
        {
            Requirement = req;
        }


        public DetachmentRequirementData(DetachmentRequirement require, DetachmentData detach)
        {
            Requirement = require;
            DetachmentData = detach;
            RequirementId = Requirement.Id;

            Units.CollectionChanged += delegate { SlotsUsed = Units.Count; };
        } 
    }
}
