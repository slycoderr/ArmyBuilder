using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models.Utility;
using MoreLinq;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ArmyListData : BindableBase
    { 

        public int UnitId { get; set; }

        public int ArmyId { get; set; }
        [XmlIgnore]
        public int PointsTotal { get => pointsTotal; set => SetValue(ref pointsTotal, value); }
        [XmlIgnore]
        public int ModelCount { get => modelCount; set => SetValue(ref modelCount, value); }

        [XmlIgnore]
        public UnitEntry UnitEntry { get; private set; }
        [XmlIgnore]
        public DetachmentData Detachment { get; private set; }

        [XmlArray]
        public ObservableCollection<ModelData> DedicatedTransports { get; set; }

        [XmlArray]
        public ObservableCollection<ModelDataGroup> ModelGroups { get; set; }

        [XmlElement]
        public ModelData SelectedDedicatedTransport
        {
            get => selectedDedicatedTransport;
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

        private int pointsTotal;
        private int modelCount;

        public ArmyListData(UnitEntry unitEntry, DetachmentData detach, Army army)
        {
            ArmyId = army.Id;
            Detachment = detach;
            SetData(unitEntry);
            UnitId = unitEntry.Id;

            PropertyChanged += OnPropertyChanged;
            ModelGroups.ForEach(m=>m.PropertyChanged += ModelGroupOnPropertyChanged);
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

        public void SetData(UnitEntry unitEntry)
        {
            if (!initialized)
            {
                UnitEntry = unitEntry;
                ModelGroups = new ObservableCollection<ModelDataGroup>(UnitEntry.Units.Select(u => new ModelDataGroup(u, this)).ToList());

                UpdatePointsTotal();

                initialized = true;
                PropertyChanged += OnPropertyChanged;
                ModelGroups.ForEach(m => m.PropertyChanged += ModelGroupOnPropertyChanged);

            }
        }

        private void UpdatePointsTotal()
        {
            int total = (SelectedDedicatedTransport?.UpgradeCostTotal ?? 0) + (SelectedDedicatedTransport?.Unit?.BaseCost ?? 0);

            total += ModelGroups.Sum(m => m.PointsCostTotal);

            PointsTotal = total;
            ModelCount = ModelGroups.Sum(g => g.CurrentUnitSize);
        }


        public ArmyListData()
        {
        }
    }
}