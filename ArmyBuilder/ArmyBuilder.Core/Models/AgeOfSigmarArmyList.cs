using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyBuilder.Core.Models
{
    public class AgeOfSigmarArmyList : IArmyList
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public uint PointsLimit { get; set; }
        public ObservableCollection<DetachmentData> Detachments { get; set; }
        public Allegiance Allegiance { get; set; }
        public GameSystem System { get; set; }
        public Army Army { get; set; }
    }
}
