using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Army", Namespace = "")]
    public class Army
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlArray]
        public List<Unit> Units { get; set; }

        public Army() { }

        public override string ToString()
        {
            return Name;
        }
    }
}
