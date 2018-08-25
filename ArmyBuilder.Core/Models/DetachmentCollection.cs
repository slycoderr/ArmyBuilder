﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("DetachmentCollection", Namespace = "")]
    public class DetachmentCollection
    {
        [XmlElement("Detachment")]
        public List<Detachment> Detachments { get; set; }
    }
}