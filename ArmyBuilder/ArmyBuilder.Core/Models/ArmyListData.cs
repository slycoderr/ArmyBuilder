using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core.Database;
using MoreLinq;
using Slycoder.MVVM;
using SQLite;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = ""), UserData]
    public class ArmyListData : BindableBase
    {   [XmlIgnore]
        public Guid Id { get; set; }
        [XmlIgnore]
        public Guid ArmyListId { get; set; }
        [XmlIgnore]
        public int UnitId { get; set; }
        [XmlIgnore]
        public byte[] Data { get { return data; } set { SetValue(ref data, value); } }

        [Ignore, XmlIgnore]
        public int PointsTotal { get { return pointsTotal; } set { SetValue(ref pointsTotal, value); } }

        [Ignore, XmlIgnore]
        public Unit Unit { get; private set; }

        [XmlArray, Ignore]
        public List<ModelData> DedicatedTransports { get; set; }

        [XmlArray, Ignore]
        public List<ModelDataGroup> Models { get; set; }

        [XmlElement, Ignore]
        public ModelData SelectedDedicatedTransport
        {
            get { return selectedDedicatedTransport; }
            set
            {
                var oldValue = selectedDedicatedTransport;

                if (SetValue(ref selectedDedicatedTransport, value))
                {
                    if (oldValue != null)
                    {
                        oldValue.PropertyChanged -= SelectedTransportOnPropertyChanged;
                    }

                    else
                    {
                        value.PropertyChanged += SelectedTransportOnPropertyChanged;
                    }

                    UpdatePointsTotal();
                }
            }
        }

        private void SelectedTransportOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdatePointsTotal();
        }

        private ModelData selectedDedicatedTransport;
        private bool initialized;
        private byte[] data;
        private int pointsTotal;

        public ArmyListData(Unit unit, Guid armyListId)
        {
            Id = Guid.NewGuid();
            ArmyListId = armyListId;
            SetData(unit);
            UnitId = unit.Id;

            PropertyChanged += OnPropertyChanged;
            Models.ForEach(m=>m.PropertyChanged += ModelGroupOnPropertyChanged);
        }

        private void ModelGroupOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PointsCostTotal")
            {
                UpdatePointsTotal();
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            
        }

        public void Save()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    var dsArmy = new XmlSerializer(typeof (ArmyListData));
                    dsArmy.Serialize(writer, this);

                    Data = stream.ToArray();
                }
            }
        }

        public void SetData(Unit unit)
        {
            if (!initialized)
            {
                Unit = unit;

                if (Data == null)
                {
                    DedicatedTransports = new List<ModelData>(Unit.DedicatedTransports.Select(u => new ModelData(u)).ToList());
                    Models = new List<ModelDataGroup>(Unit.Models.Select(u => new ModelDataGroup(u, this)).ToList());
                    Save();
                }

                else
                {
                    using (var stream = new MemoryStream(Data))
                    {
                        using (var reader = XmlReader.Create(stream))
                        {
                            var dsArmy = new XmlSerializer(typeof (ArmyListData));
                            var obj = (ArmyListData) dsArmy.Deserialize(reader);

                            DedicatedTransports = obj.DedicatedTransports;
                            Models = obj.Models;
                            SelectedDedicatedTransport = obj.SelectedDedicatedTransport;

                            SelectedDedicatedTransport?.SetData(unit.DedicatedTransports.Single(m => m.Id == SelectedDedicatedTransport.ModelId));

                            DedicatedTransports.ForEach(t=>t.SetData(Unit.DedicatedTransports.Single(m=>m.Id == t.ModelId)));
                            Models.ForEach(t=>t.SetData(Unit.Models.Single(m=>m.Id == t.ModelId), this));

                        }
                    }
                }
                UpdatePointsTotal();

                initialized = true;
                PropertyChanged += OnPropertyChanged;
                Models.ForEach(m => m.PropertyChanged += ModelGroupOnPropertyChanged);

            }
        }

        private void UpdatePointsTotal()
        {
            int total = (SelectedDedicatedTransport?.PointsCostTotal ?? 0) + (SelectedDedicatedTransport?.Model?.BaseCost ?? 0);

            total += Models.Sum(m => m.PointsCostTotal);

            PointsTotal = total;
            Save();
        }


        public ArmyListData()
        {
        }
    }
}