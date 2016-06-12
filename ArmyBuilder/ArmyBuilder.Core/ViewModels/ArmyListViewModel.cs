using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;
using GalaSoft.MvvmLight.Command;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.ViewModels
{
    public class ArmyListViewModel : BindableBase
    {
        public ArmyList ArmyList { get; }

        public int PointsRemaining { get { return pointsRemaining; } set { SetValue(ref pointsRemaining, value); } }

        public int PointsUsed { get { return pointsUsed; } set { SetValue(ref pointsUsed, value); } }

        public int HqCount { get { return hqCount; } set { SetValue(ref hqCount, value); } }
        public int TroopCount { get { return troopCount; } set { SetValue(ref troopCount, value); } }
        public int EliteCount { get { return eliteCount; } set { SetValue(ref eliteCount, value); } }
        public int FastAttackCount { get { return fastAttackCount; } set { SetValue(ref fastAttackCount, value); } }
        public int HeavySupportCount { get { return heavySupportCount; } set { SetValue(ref heavySupportCount, value); } }
        public int LordOfWarCount { get { return lordOfWarCount; } set { SetValue(ref lordOfWarCount, value); } }

        public List<Unit> HqUnits => ArmyList.Army.Units.Where(u => u.ForceOrgSlot == 0).ToList();
        public List<Unit> TroopUnits => ArmyList.Army.Units.Where(u => u.ForceOrgSlot == 1).ToList();
        public List<Unit> EliteUnits => ArmyList.Army.Units.Where(u => u.ForceOrgSlot == 2).ToList();
        public List<Unit> FastAttackUnits => ArmyList.Army.Units.Where(u => u.ForceOrgSlot == 3).ToList();
        public List<Unit> HeavySupportUnits => ArmyList.Army.Units.Where(u => u.ForceOrgSlot == 4).ToList();
        public List<Unit> LordOfWarUnits => ArmyList.Army.Units.Where(u => u.ForceOrgSlot == 5).ToList();

        public ArmyListData SelectedUnit { get { return selectedUnit; } set { SetValue(ref selectedUnit, value); } }

        public ObservableCollection<ArmyListData> Units { get; } = new ObservableCollection<ArmyListData>();

        public ObservableCollection<ForceOrgGroup> ArmyListDataGroups { get; } =new ObservableCollection<ForceOrgGroup>();
        public ObservableCollection<ArmyUnitGroup> ArmyUnitGroups { get; } = new ObservableCollection<ArmyUnitGroup>();

        public RelayCommand<Unit> AddUnitCommand => new RelayCommand<Unit>(AddUnit);
        public RelayCommand<ArmyListData> RemoveUnitCommand => new RelayCommand<ArmyListData>(RemoveUnit);

        public bool IsUnitFlyoutOpened { get { return isUnitFlyoutOpened; } set { SetValue(ref isUnitFlyoutOpened, value); } }

        private int eliteCount;
        private int fastAttackCount;
        private int heavySupportCount;
        private int hqCount;
        private bool isUnitFlyoutOpened;
        private int lordOfWarCount;

        private ArmyListData selectedUnit;
        private int troopCount;
        private int pointsRemaining;
        private int pointsUsed;

        public ArmyListViewModel(ArmyList armyList)
        {
            ArmyList = armyList;
            UpdateArmyListDataSource();
            UpdateForceOrgCount();
            Units.CollectionChanged += UnitsOnCollectionChanged;
            UpdateArmyUnitsListDataSource();
            UpdatePointsTotal();
        }

        private void UnitsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateArmyListDataSource();
            UpdateForceOrgCount();
            UpdatePointsTotal();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems.Cast<ArmyListData>().ForEach(a=>a.PropertyChanged += UnitPropertyChanged);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UnitPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PointsTotal")
            {
                UpdatePointsTotal();
            }
        }

        private void UpdateArmyUnitsListDataSource()
        {
            var groups = ArmyList.Army.Units.GroupBy(i => new {i.ForceOrgSlot}).Select(i => new ArmyUnitGroup(i.ToList())).OrderBy(g => g.ForceOrgId);

            groups.ForEach(g => ArmyUnitGroups.Add(g));
        }

        private void UpdateArmyListDataSource()
        {
            var currentSelectedItem = SelectedUnit;
            var groups = Units.GroupBy(i => new {i.Unit.ForceOrgSlot}).Select(i => new ForceOrgGroup(i.ToList())).ToList();

            ArmyListDataGroups.Clear();
            groups.ForEach(g => ArmyListDataGroups.Add(g));
            SelectedUnit = currentSelectedItem;
            UpdateForceOrgCount();
        }

        private void UpdateForceOrgCount()
        {
            HqCount = Units.Count(u => u.Unit.ForceOrgSlot == 0 && u.Unit.CountsTowardForceOrg);
            TroopCount = Units.Count(u => u.Unit.ForceOrgSlot == 1 && u.Unit.CountsTowardForceOrg);
            EliteCount = Units.Count(u => u.Unit.ForceOrgSlot == 2 && u.Unit.CountsTowardForceOrg);
            FastAttackCount = Units.Count(u => u.Unit.ForceOrgSlot == 3 && u.Unit.CountsTowardForceOrg);
            HeavySupportCount = Units.Count(u => u.Unit.ForceOrgSlot == 4 && u.Unit.CountsTowardForceOrg);
            LordOfWarCount = Units.Count(u => u.Unit.ForceOrgSlot == 5 && u.Unit.CountsTowardForceOrg);
        }

        private void UpdatePointsTotal()
        {
            PointsUsed = Units.Sum(a => a.PointsTotal);
            PointsRemaining = ArmyList.PointsLimit - PointsUsed;
        }

        public void AddUnit(Unit unit)
        {
            if (unit != null)
            {
                
                Units.Add(new ArmyListData(unit, ArmyList.Id));
                SelectedUnit = Units.Last();
                IsUnitFlyoutOpened = false;
            }
        }

        private void RemoveUnit(ArmyListData unit)
        {
            Units.Remove(unit);
        }
    }
}