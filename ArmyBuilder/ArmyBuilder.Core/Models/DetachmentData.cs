﻿using System.Collections.ObjectModel;
using System.Xml.Serialization;
using MoreLinq;

namespace ArmyBuilder.Core.Models
{
    public class DetachmentData
    {
        [XmlIgnore]
        public Detachment Detachment { get; private set; }

        public Army Army { get; private set; }

        public ObservableCollection<DetachmentData> SelectedDetachments { get; private set; } = new ObservableCollection<DetachmentData>();

        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>(); 

        [XmlAttribute]
        public bool IsPrimary { get; set; }

        public DetachmentData(Detachment detachment, Army army)
        {
            Detachment = detachment;
            Army = army;
        }
    }
}
