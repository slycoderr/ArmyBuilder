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
