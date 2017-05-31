using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("DetachmentRequirement", Namespace = "")]
    public class DetachmentRequirement
    {
        [XmlAttribute]
        public int Minimum { get; set; }
        [XmlAttribute]
        public int Maximum { get; set; }

        [XmlAttribute]
        public int Value { get; set; }

    }
}
