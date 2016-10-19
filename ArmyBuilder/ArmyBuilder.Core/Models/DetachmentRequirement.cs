using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentRequirement
    {
        [XmlAttribute]
        public DetachmentRequirementType Type { get; set; }

        [XmlAttribute]
        public int Minimum { get; set; }
        [XmlAttribute]
        public int Maximum { get; set; }

        [XmlAttribute]
        public int Value { get; set; }

    }
}
