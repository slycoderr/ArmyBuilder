using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentData
    {
        [XmlIgnore]
        public Detachment Detachment { get; private set; }

        [XmlAttribute]
        public bool IsPrimary { get; set; }

        public DetachmentData(Detachment detachment)
        {
            Detachment = detachment;
        }
    }
}
