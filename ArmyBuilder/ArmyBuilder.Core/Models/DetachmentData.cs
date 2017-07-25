using System.Collections.ObjectModel;
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
        public Detachment Detachment { get; }
        [XmlArray]
        public ObservableCollection<DetachmentRequirementData> DetachmentRequirementData { get; } = new ObservableCollection<DetachmentRequirementData>();

        public DetachmentData()
        {
            
        }

        public DetachmentData(Detachment detachment)
        {
            Detachment = detachment;
            DetachmentRequirementData = new ObservableCollection<DetachmentRequirementData>(Detachment.Requirements.Select(r=> new DetachmentRequirementData(r, this)));

            
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
        public int SlotsUsed { get { return slotsUsed; } set { SetValue(ref slotsUsed, value); } }
        [XmlIgnore]
        public DetachmentRequirement Requirement { get; }
        [XmlIgnore]
        public DetachmentData DetachmentData { get; }
        [XmlArray]
        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();


        public DetachmentRequirementData(DetachmentRequirement require, DetachmentData detach)
        {
            Requirement = require;
            DetachmentData = detach;
        } 
    }
}
