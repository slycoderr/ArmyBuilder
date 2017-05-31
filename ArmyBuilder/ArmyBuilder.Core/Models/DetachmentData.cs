using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentData : BindableBase
    {
        //private DetachmentData selectedDetachment;

        [XmlIgnore]
        public Detachment Detachment { get; }

        public ObservableCollection<DetachmentRequirementData> DetachmentRequirementData { get; } = new ObservableCollection<DetachmentRequirementData>();

        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();

        public RelayCommand<Detachment> AddSubDetachmentCommand => new RelayCommand<Detachment>(AddSubDetachment);

        public ObservableCollection<UnitEntry> AvailableUnits { get; } = new ObservableCollection<UnitEntry>();


        private void AddSubDetachment(Detachment detachment)
        {
            //SelectedDetachments.Add(new DetachmentData(detachment, Army));
        }

        [XmlAttribute]
        public bool IsPrimary { get; set; }

        public DetachmentData()
        {
            
        }

        public DetachmentData(Detachment detachment)
        {
            Detachment = detachment;
            DetachmentRequirementData = new ObservableCollection<DetachmentRequirementData>(Detachment.Requirements.Select(r=> new DetachmentRequirementData(r, this)));
                foreach (var req in Detachment.Requirements)
                {
                    Detachment.Army.UnitEntries.Where(u => (int)u.ForceOrgSlot == req.Value).ForEach(AvailableUnits.Add);
                }
            
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
