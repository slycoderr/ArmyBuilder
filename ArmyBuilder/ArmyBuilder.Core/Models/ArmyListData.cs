using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using MoreLinq;
using Slycoder.MVVM;
using SQLite;

namespace ArmyBuilder.Core.Models
{

    public class ArmyListData : BindableBase

    {
        public Unit Unit { get; }
        public Army Army { get; }

        public uint Count { get; set; }

        public ArmyListData(Unit unit, Army army)
        {
            Army = army;
            Unit = unit;
        }
    }
}