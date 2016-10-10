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
        private DetachmentData selectedDetachment;

        [XmlIgnore]
        public Detachment Detachment { get; private set; }

        public Army Army { get; private set; }

        public DetachmentData SelectedDetachment { get { return selectedDetachment; } set { SetValue(ref selectedDetachment, value); } }

        public ObservableCollection<DetachmentData> SelectedDetachments { get; private set; } = new ObservableCollection<DetachmentData>();

        public ObservableCollection<DetachmentRequirementData> DetachmentRequirementData { get; } = new ObservableCollection<DetachmentRequirementData>();

        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();

        public RelayCommand<Detachment> AddSubDetachmentCommand => new RelayCommand<Detachment>(AddSubDetachment);

        private void AddSubDetachment(Detachment detachment)
        {
            SelectedDetachments.Add(new DetachmentData(detachment, Army));
        }

        [XmlAttribute]
        public bool IsPrimary { get; set; }

        public DetachmentData(Detachment detachment, Army army)
        {
            Detachment = detachment;
            DetachmentRequirementData = new ObservableCollection<DetachmentRequirementData>(Detachment.Requirements.Select(r=> new DetachmentRequirementData(r, this)));
            Army = army;
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
