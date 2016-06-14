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
        public Model Model { get; private set; }

        [XmlIgnore]
        public ArmyListData Unit { get; private set; }

        [XmlArray]
        public ObservableCollection<ModelData> Models { get; set; } = new ObservableCollection<ModelData>();

        [XmlIgnore]
        public int PointsCostTotal { get { return pointsCostTotal; } set { SetValue(ref pointsCostTotal, value); } }

        [XmlIgnore]
        public int UpgradesCostTotal { get { return upgradesCostTotal; } set { SetValue(ref upgradesCostTotal, value); } }

        [XmlAttribute]
        public int CurrentUnitSize
        {
            get { return currentUnitSize; }
            set
            {
                var oldValue = currentUnitSize;

                if (SetValue(ref currentUnitSize, value))
                {
                    if (Model != null && oldValue > currentUnitSize)
                    {
                        Models.TakeLast(oldValue - currentUnitSize).ForEach(m => Models.Remove(m));
                        UpdatePointsTotal();
                    }

                    else if (Model != null && oldValue < currentUnitSize)
                    {
                        for (var i = 0; i < currentUnitSize - oldValue; i++)
                        {
                            Models.Add(new ModelData(Model));
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

        public ModelDataGroup(Model m, ArmyListData unit)
        {
            Model = m;
            Unit = unit;
            CurrentUnitSize = Model.Minimum;
            PropertyChanged += OnPropertyChanged;
            ModelId = Model.Id;
            UpdatePointsTotal();
        }

        public void SetData(Model m, ArmyListData unit)
        {
            Model = m;
            Unit = unit;

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
            UpgradesCostTotal = Models.Sum(m => m.PointsCostTotal);
            PointsCostTotal = UpgradesCostTotal + (Model.Minimum > 0 ? Model.BaseCost : 0) + (CurrentUnitSize > Model.Minimum ? Model.IncrementCost*(CurrentUnitSize - Model.Minimum) : 0);
            var allEquipment = GetAllModelEquipment();

            allEquipment.Where(e=>e.Equipment.PerX > 0).ForEach(e =>
            { //count up all the models that have the same name just in case they are separated for leader upgrade purposes 
                if (Unit.Models.Where(u=>u.Model.Name == Model.Name).Sum(m=>m.CurrentUnitSize) >= e.Equipment.PerX)
                {
                    e.TempLimit = e.Equipment.Limit + (int) (CurrentUnitSize/e.Equipment.PerX);
                }

                else
                {
                    e.TempLimit = e.Equipment.Limit;
                }
            });

            var equipGroup = allEquipment.Where(ee => ee.Equipment.MutualId != 0).GroupBy(ee => ee.Equipment.MutualId);

            foreach (var group in equipGroup)
            {
                foreach (var equip in group)
                {
                    if (equip.TempLimit > 0 &&
                        allEquipment.Count(e => e.Equipment.MutualId == equip.Equipment.MutualId && e.IsTaken) ==
                        equip.TempLimit)
                    {
                        allEquipment.Where(eee => eee.Equipment.MutualId == group.Key && !eee.IsTaken)
                            .ForEach(eee =>
                            {
                                GetAllEquipmentSubEquipment(eee).ForEach(eq => eq.CanAdd = false);
                                eee.CanAdd = false;
                            });

                        allEquipment.Where(eee => eee.Equipment.MutualId == group.Key && eee.IsTaken)
                            .ForEach(eee =>
                            {
                                GetAllEquipmentSubEquipment(eee).ForEach(eq => eq.CanAdd = true);
                                eee.CanAdd = true;
                            });
                    }

                    else if (equip.TempLimit > 0 &&
                             allEquipment.Count(e => e.Equipment.MutualId == equip.Equipment.MutualId && e.IsTaken) <
                             equip.TempLimit)
                    {
                        allEquipment.Where(eee => eee.Equipment.MutualId == group.Key)
                            .ForEach(eee =>
                            {
                                GetAllEquipmentSubEquipment(eee).ForEach(eq => eq.CanAdd = true);
                                eee.CanAdd = true;
                            });
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