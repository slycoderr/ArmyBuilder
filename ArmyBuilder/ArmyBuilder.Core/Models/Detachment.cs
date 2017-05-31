using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Detachment", Namespace = "")]
    public class Detachment
    {
        [XmlIgnore]
        public Army Army { get; set; }
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray]
        public ObservableCollection<DetachmentRequirement> Requirements { get; set; } = new ObservableCollection<DetachmentRequirement>();

        public override string ToString()
        {
            return Name;
        }
    }
}
