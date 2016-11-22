using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyBuilder.Core.Models
{
    public interface IArmyList
    {
        string Name { get; set; }
        DateTime DateCreated { get; set; }
        uint PointsLimit { get; set; }
        GameSystem System { get; set; }
        ObservableCollection<DetachmentData> Detachments { get; set; }
        Army Army { get; set; }
    }
}
