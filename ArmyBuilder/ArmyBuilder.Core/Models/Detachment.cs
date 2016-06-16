using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    public class Detachment
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray]
        public List<Detachment> SuDetachments { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
