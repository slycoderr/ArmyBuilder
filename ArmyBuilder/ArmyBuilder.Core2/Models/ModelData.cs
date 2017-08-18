using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models.Utility;
using MoreLinq;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ModelData : BindableBase
    {
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlIgnore]
        public Unit Unit { get; private set; }

        [XmlAttribute]
        public int ModelId { get; set; }

        [XmlArray]
        public List<EquipmentData> Equipment { get; set; } = new List<EquipmentData>();

        [XmlArray]
        public List<EquipmentData> Upgrades { get; set; } = new List<EquipmentData>();

        [XmlIgnore]
        public int PointsCostTotal { get => pointsCostTotal; set => SetValue(ref pointsCostTotal, value); }
        [XmlIgnore]
        public int UpgradeCostTotal { get => upgradeCostTotal; set => SetValue(ref upgradeCostTotal, value); }

        private int upgradeCostTotal;
        private int pointsCostTotal;

        public ModelData()
        {
        }

        public ModelData(Unit m)
        {
            Id = Guid.NewGuid();
            Unit = m;
            Equipment = new List<EquipmentData>(Unit.DefaultEquipment.Select(e => new EquipmentData(e, Id.ToString(), null)));
            Upgrades = new List<EquipmentData>(Unit.Upgrades.Select(e => new EquipmentData(e, null)));
            ModelId = m.Id;

            Equipment.ForEach(SubscribeToEquipment);
            Upgrades.ForEach(SubscribeToEquipment);
        }

        public void SetData(Unit m)
        {
            Unit = m;

            Equipment.ForEach(e => e.SetData(Unit.DefaultEquipment.Single(i => i.Id == e.EquipmentId), null));
            Upgrades.ForEach(e => e.SetData(Unit.Upgrades.Single(i => i.Id == e.EquipmentId), null));
            CalculatePointsCost();

            Equipment.ForEach(SubscribeToEquipment);
            Upgrades.ForEach(SubscribeToEquipment);
        }

        private void SubscribeToEquipment(EquipmentData equipment)
        {
            equipment.PropertyChanged += EquipmentPropertyChanged;

            equipment.ReplacementOptions.ForEach(e =>
            {
                e.PropertyChanged += EquipmentPropertyChanged;
                SubscribeToEquipment(e);
            });
            equipment.GivenEquipment.ForEach(e =>
            {
                e.PropertyChanged += EquipmentPropertyChanged;
                SubscribeToEquipment(e);
            });
        }

        private int TotalEquipmentCost(EquipmentData equipment)
        {
            return (equipment.IsTaken ? equipment.Equipment.Cost : 0) + equipment.ReplacementOptions.Sum(TotalEquipmentCost) + equipment.GivenEquipment.Sum(TotalEquipmentCost);
        }

        private void EquipmentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsTaken")
            {
                CalculatePointsCost();
            }
        }

        internal void CalculatePointsCost()
        {
            UpgradeCostTotal = Equipment.Select(TotalEquipmentCost).Sum() + Upgrades.Select(TotalEquipmentCost).Sum();
            PointsCostTotal = UpgradeCostTotal + Unit.CostPerModel;
        }
    }
}