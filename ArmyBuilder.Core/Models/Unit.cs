﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models.Utility;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot("Unit", Namespace = "")]
    public class Unit : BindableBase
    {
        private int id;
        private string name;
        private int minimum;
        private int maximum;
        private int costPerModel;
        private int baseCost;
        private bool isWargearCostIncludedInBaseCost;

        [XmlAttribute]
        public int Id { get => id; set => SetValue(ref id, value); }

        [XmlAttribute]
        public string Name { get => name; set => SetValue(ref name, value); }

        [XmlAttribute]
        public int Minimum { get => minimum; set => SetValue(ref minimum, value); }

        [XmlAttribute]
        public int Maximum { get => maximum; set => SetValue(ref maximum, value); }

        [XmlAttribute]
        public int CostPerModel { get => costPerModel; set => SetValue(ref costPerModel, value); }

        [XmlAttribute]
        public int BaseCost { get => baseCost; set => SetValue(ref baseCost, value); }


        [XmlAttribute]
        public bool IsWargearCostIncludedInBaseCost { get => isWargearCostIncludedInBaseCost; set => SetValue(ref isWargearCostIncludedInBaseCost, value); }


        /// <summary>
        /// Needed for loading since transports don't know what unit entry they're from.
        /// </summary>
        [XmlAttribute]
        public int UnitEntryId { get; set; }

        [XmlArray]
        public ObservableCollection<Equipment> DefaultEquipment { get; set; } = new ObservableCollection<Equipment>();

        [XmlArray]
        public ObservableCollection<Equipment> Upgrades { get; set; } = new ObservableCollection<Equipment>();

        [XmlIgnore]
        public UnitEntry UnitEntry { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}