using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;
using MoreLinq;
using Slycoder.MVVM;
using SQLite;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentData : BindableBase
    {
        //private DetachmentData selectedDetachment;

        [XmlIgnore, Ignore]
        public Detachment Detachment { get; }
        [Ignore]
        public ObservableCollection<DetachmentRequirementData> DetachmentRequirementData { get; } = new ObservableCollection<DetachmentRequirementData>();
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
