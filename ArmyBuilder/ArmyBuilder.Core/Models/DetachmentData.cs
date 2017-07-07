using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentData : BindableBase
    {
        //private DetachmentData selectedDetachment;

        [XmlIgnore]
        public Detachment Detachment { get; }
        [XmlIgnore]
        public ObservableCollection<DetachmentRequirementData> DetachmentRequirementData { get; } = new ObservableCollection<DetachmentRequirementData>();
        [XmlArray]
        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();

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

    public class DetachmentRequirementData
    {
        public DetachmentRequirement Requirement { get; }
        public DetachmentData DetachmentData { get; }
        

        public DetachmentRequirementData(DetachmentRequirement require, DetachmentData detach)
        {
            Requirement = require;
            DetachmentData = detach;
        } 
    }
}
