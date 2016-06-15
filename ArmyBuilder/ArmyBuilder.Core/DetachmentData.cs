using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models;

namespace ArmyBuilder.Core
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
