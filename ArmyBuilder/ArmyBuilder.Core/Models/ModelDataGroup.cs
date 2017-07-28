using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    [XmlRoot(Namespace = "")]
    public class ModelDataGroup : BindableBase
    {
        [XmlAttribute]
        public int ModelId { get; set; }

        [XmlIgnore]
        public Unit Unit { get; private set; }

        [XmlIgnore]
        public ArmyListData ArmyListData { get; private set; }

        [XmlArray]
        public ObservableCollection<ModelData> Models { get; set; } = new ObservableCollection<ModelData>();

        [XmlIgnore]
        public int PointsCostTotal { get => pointsCostTotal; set => SetValue(ref pointsCostTotal, value); }

        [XmlIgnore]
        public int UpgradesCostTotal { get => upgradesCostTotal; set => SetValue(ref upgradesCostTotal, value); }

        [XmlAttribute]
        public int CurrentUnitSize
        {
            get => currentUnitSize;
            set
            {
                var oldValue = currentUnitSize;

                if (SetValue(ref currentUnitSize, value))
                {
                    if (Unit != null && oldValue > currentUnitSize)
                    {
                        Models.TakeLast(oldValue - currentUnitSize).ForEach(m => Models.Remove(m));
                        UpdatePointsTotal();
                    }

                    else if (Unit != null && oldValue < currentUnitSize)
                    {
                        for (var i = 0; i < currentUnitSize - oldValue; i++)
                        {
                            Models.Add(new ModelData(Unit));
                            Models.Last().PropertyChanged += ModelPropertyChanged;
                            UpdatePointsTotal();
                        }
                    }
                }
            }
        }

        private int currentUnitSize;

        private int pointsCostTotal;

        private int upgradesCostTotal;

        public ModelDataGroup()
        {
        }

        public ModelDataGroup(Unit m, ArmyListData data)
        {
            Unit = m;
            ArmyListData = data;
            CurrentUnitSize = Unit.Minimum;
            PropertyChanged += OnPropertyChanged;
            ModelId = Unit.Id;
            UpdatePointsTotal();
        }

        public void SetData(Unit m, ArmyListData data)
        {
            Unit = m;
            ArmyListData = data;

            Models.ForEach(mm =>
            {
                mm.SetData(m);
                mm.PropertyChanged += ModelPropertyChanged;
            });
            UpdatePointsTotal();
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PointsCostTotal")
            {
                UpdatePointsTotal();
            }
        }

        private void UpdatePointsTotal()
        {
            Models.ForEach(m=>m.CalculatePointsCost());
            UpgradesCostTotal = Models.Sum(m => m.PointsCostTotal);
            //PointsCostTotal = UpgradesCostTotal + (Unit.Minimum > 0 ? Unit.BaseCost : 0) + (CurrentUnitSize > Unit.Minimum ? Unit.CostPerModel * (CurrentUnitSize - Unit.Minimum) : 0);
            PointsCostTotal = UpgradesCostTotal + (Unit.BaseCost + (Unit.CostPerModel*CurrentUnitSize));
            var allEquipment = GetAllModelEquipment();

            if (Unit.UnitEntry.Units != null)
            {
                allEquipment.Where(e => e.Equipment.PerX > 0).ForEach(e =>
                {
                    var unitSize = Models.Count;

                    //count up all the models that have the same name just in case they are separated for leader upgrade purposes
                    if (unitSize >= e.Equipment.PerX)
                        {
                            e.TempLimit = e.Equipment.Limit + unitSize / e.Equipment.PerX;
                        }

                        else
                        {
                            e.TempLimit = e.Equipment.Limit;
                        }
                });
            }

            var equipGroup = allEquipment.Where(ee => ee.Equipment.MutualId != 0).GroupBy(ee => ee.Equipment.MutualId);

            foreach (var group in equipGroup)
            {
                var targetMutualId = group.Key;

                foreach (var equip in group)
                {
                    if (equip.TempLimit > 0)
                    {
                        var numTaken = group.Count(e => e.IsTaken);

                        if (numTaken == equip.TempLimit) //don't let more equipment be taken with the same id
                        {
                            allEquipment.Where(eee => eee.Equipment.MutualId == targetMutualId && (!eee.IsTaken && !eee.ParentEquipment.ReplacementOptions.Any(e => e.IsTaken))).ForEach(eee =>
                            {
                                GetAllEquipmentSubEquipment(eee).ForEach(eq => eq.CanAdd = false);
                                eee.CanAdd = false;
                            });

                            allEquipment.Where(eee => eee.Equipment.MutualId == targetMutualId && (eee.IsTaken)).ForEach(eee =>
                            {
                                GetAllEquipmentSubEquipment(eee).ForEach(eq => eq.CanAdd = true);
                                eee.CanAdd = true;
                            });
                        }

                        else if (numTaken > equip.TempLimit) //remove equipment until the rules are satisfied 
                        {
                            while (numTaken > equip.TempLimit)
                            {
                                var ee = allEquipment.FirstOrDefault(eee => eee.Equipment.MutualId == targetMutualId && eee.IsTaken);

                                GetAllEquipmentSubEquipment(ee).ForEach(eq => eq.CanAdd = false);
                                GetAllEquipmentSubEquipment(ee).ForEach(eq => eq.IsTaken = false);
                                ee.CanAdd = false;
                                ee.IsTaken = false;
                                numTaken--;
                            }
                        }

                        else if (numTaken < equip.TempLimit) //allow the equipment to be taken
                        {
                            allEquipment.Where(eee => eee.Equipment.MutualId == targetMutualId).ForEach(eee =>
                            {
                                GetAllEquipmentSubEquipment(eee).ForEach(eq => eq.CanAdd = true);
                                eee.CanAdd = true;
                            });
                        }
                    }
                }
            }
        }

        private List<EquipmentData> GetAllModelEquipment()
        {
            var allEquip = new List<EquipmentData>();
            var eq = Models.SelectMany(m => m.Equipment);
            var upg = Models.SelectMany(m => m.Upgrades);

            allEquip.AddRange(eq);
            allEquip.AddRange(upg);

            foreach (var e in allEquip.ToList())
            {
                allEquip.AddRange(GetAllEquipmentSubEquipment(e));
            }

            return allEquip;
        }

        private List<EquipmentData> GetAllEquipmentSubEquipment(EquipmentData e)
        {
            var allEquip = new List<EquipmentData>();

            allEquip.AddRange(e.GivenEquipment);
            allEquip.AddRange(e.ReplacementOptions);

            foreach (var ee in allEquip.ToList())
            {
                allEquip.AddRange(GetAllEquipmentSubEquipment(ee));
            }

            return allEquip;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentUnitSize))
            {
            }
        }
    }
}