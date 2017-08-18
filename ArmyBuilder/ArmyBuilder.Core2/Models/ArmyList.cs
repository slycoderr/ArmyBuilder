using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models.Utility;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ArmyList : BindableBase
    {
        private string name;
        private uint pointsLimit;
        private ObservableCollection<DetachmentData> detachments = new ObservableCollection<DetachmentData>();
        private bool usePowerPoints;
        private ArmyListType armyListType;

        [XmlAttribute]
        public string Name { get => name; set => SetValue(ref name, value); }

        [XmlArray]
        public ObservableCollection<DetachmentData> Detachments { get => detachments; internal set => SetValue(ref detachments, value); }

        [XmlAttribute]
        public uint PointsLimit { get => pointsLimit; set => SetValue(ref pointsLimit, value); }

        [XmlAttribute]
        public bool UsePowerPoints { get => usePowerPoints; set => SetValue(ref usePowerPoints, value); }

        [XmlAttribute]
        public ArmyListType ArmyListType { get => armyListType; set => SetValue(ref armyListType, value); }

        public ArmyList() { }

        public ArmyList(string newName, uint points)
        {
            Name = newName;
            PointsLimit = points;
            ArmyListType = ArmyListType.MatchedPlay;
            UsePowerPoints = false;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Load(IEnumerable<Detachment> detachments)
        {
            foreach (var detachment in Detachments)
            {
                detachment.Initialize(detachments.First(d => d.Id == detachment.DetachmentId));

                foreach (var req in detachment.DetachmentRequirementData)
                {
                    req.Initialize(detachment.Detachment.Requirements.First(r => r.Id == req.RequirementId));
                }
            }
        }
    }
}
