using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentData : BindableBase
    {
        //private DetachmentData selectedDetachment;

        [XmlIgnore]
        public Detachment Detachment { get; }

        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();

        [XmlAttribute]
        public bool IsPrimary { get; set; }

        public DetachmentData()
        {
        }

        public DetachmentData(Detachment detachment)
        {
            Detachment = detachment;
        }
    }
}