using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class AgeOfSigmarArmyList : BindableBase, IArmyList
    {
        private string name;
        private uint pointsLimit;
        private uint currentPointsTotal;
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        public DateTime DateCreated { get; set; }
        public uint PointsLimit { get { return pointsLimit; } set { SetValue(ref pointsLimit, value); } }

        public ObservableCollection<DetachmentData> Detachments { get; set; } = new ObservableCollection<DetachmentData>();
        public Allegiance Allegiance { get; set; }
        public GameSystem System { get; set; }
        public Army Army { get; set; }
        public uint CurrentPointsTotal { get { return currentPointsTotal; } set { SetValue(ref currentPointsTotal, value); } }

        public ObservableCollection<string> Data { get; set; } = new ObservableCollection<string>();

        public AgeOfSigmarArmyList()
        {
            Data.Add("");
        }
    }
}
