using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ArmyList : BindableBase
    {
        private string name;
        private int pointsLimit;
        private ObservableCollection<DetachmentData> detachments = new ObservableCollection<DetachmentData>();

        [XmlAttribute]
        public string Name { get => name; set => SetValue(ref name, value); }

        [XmlArray]
        public ObservableCollection<DetachmentData> Detachments { get => detachments; internal set => SetValue(ref detachments, value); }

        [XmlAttribute]
        public int PointsLimit { get => pointsLimit; set => SetValue(ref pointsLimit, value); }

        public ArmyList() { }

        public ArmyList(string newName, int points)
        {
            Name = newName;
            PointsLimit = points;

        }
    }
}
