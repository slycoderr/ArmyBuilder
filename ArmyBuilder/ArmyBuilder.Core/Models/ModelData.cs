﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ModelData : BindableBase
    {
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlIgnore]
        public Model Model { get; private set; }

        [XmlAttribute]
        public int ModelId { get; set; }

        [XmlArray]
        public List<EquipmentData> Equipment { get; set; } = new List<EquipmentData>();

        [XmlArray]
        public List<EquipmentData> Upgrades { get; set; } = new List<EquipmentData>();

        [XmlIgnore]
        public int PointsCostTotal { get { return pointsCostTotal; } set { SetValue(ref pointsCostTotal, value); } }

        private int pointsCostTotal;

        public ModelData()
        {
        }

        public ModelData(Model m)
        {
            Id = Guid.NewGuid();
            Model = m;
            Equipment = new List<EquipmentData>(Model.DefaultEquipment.Select(e => new EquipmentData(e, Id.ToString(), null)));
            Upgrades = new List<EquipmentData>(Model.Upgrades.Select(e => new EquipmentData(e, null)));
            ModelId = m.Id;

            Equipment.ForEach(SubscribeToEquipment);
            Upgrades.ForEach(SubscribeToEquipment);
        }

        public void SetData(Model m)
        {
            Model = m;

            Equipment.ForEach(e => e.SetData(Model.DefaultEquipment.Single(i => i.Id == e.EquipmentId), null));
            Upgrades.ForEach(e => e.SetData(Model.Upgrades.Single(i => i.Id == e.EquipmentId), null));
            CalculatePointsCost();

            Equipment.ForEach(SubscribeToEquipment);
            Upgrades.ForEach(SubscribeToEquipment);
        }

        private void SubscribeToEquipment(EquipmentData equipment)
        {
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

        private void CalculatePointsCost()
        {
            PointsCostTotal = Equipment.Select(TotalEquipmentCost).Sum() + Upgrades.Select(TotalEquipmentCost).Sum();
        }
    }
}