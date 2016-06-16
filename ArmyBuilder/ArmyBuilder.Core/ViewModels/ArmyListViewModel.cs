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

        public DetachmentData SelectedDetachment { get { return selectedDetachment; } set { SetValue(ref selectedDetachment, value); } }

        public int PointsRemaining { get { return pointsRemaining; } set { SetValue(ref pointsRemaining, value); } }

        public int PointsUsed { get { return pointsUsed; } set { SetValue(ref pointsUsed, value); } }

        public ForceOrgCounter ForceOrgCount { get; } = new ForceOrgCounter();

        public ObservableCollection<Unit> HqUnits { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> TroopUnits { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> EliteUnits { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> FastAttackUnits { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> HeavySupportUnits { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> LordOfWarUnits { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> FortificationUnits { get; } = new ObservableCollection<Unit>();

        public ArmyListData SelectedUnit { get { return selectedUnit; } set { SetValue(ref selectedUnit, value); } }

        public ObservableCollection<ForceOrgGroup> ArmyListDataGroups { get; } =new ObservableCollection<ForceOrgGroup>();
        public ObservableCollection<ArmyUnitGroup> ArmyUnitGroups { get; } = new ObservableCollection<ArmyUnitGroup>();

        public RelayCommand<Unit> AddUnitCommand => new RelayCommand<Unit>(AddUnit);
        public RelayCommand<ArmyListData> RemoveUnitCommand => new RelayCommand<ArmyListData>(RemoveUnit);

        public bool IsUnitFlyoutOpened { get { return isUnitFlyoutOpened; } set { SetValue(ref isUnitFlyoutOpened, value); } }
        private bool isUnitFlyoutOpened;

        private ArmyListData selectedUnit;
        private int pointsRemaining;
        private int pointsUsed;
        private DetachmentData selectedDetachment;

        public ArmyListViewModel(ArmyList armyList)
        {
            ArmyList = armyList;

            UpdateArmyListDataSource();
            
            UpdatePointsTotal();
            PropertyChanged += OnPropertyChanged;
            SelectedDetachment = armyList.Detachments.First();

            ArmyList.Detachments.ForEach(d=>d.Units.CollectionChanged += UnitsOnCollectionChanged);
        }

        private void UpdateUnitLists()
        {
            HqUnits.Clear();
            TroopUnits.Clear();
            EliteUnits.Clear();
            FastAttackUnits.Clear();
            HeavySupportUnits.Clear();
            LordOfWarUnits.Clear();
            FortificationUnits.Clear();

            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 0).ForEach(u=>HqUnits.Add(u));
            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 1).ForEach(u=> TroopUnits.Add(u));
            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 2).ForEach(u=> EliteUnits.Add(u));
            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 3).ForEach(u=> FastAttackUnits.Add(u));
            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 4).ForEach(u=> HeavySupportUnits.Add(u));
            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 5).ForEach(u=> LordOfWarUnits.Add(u));
            SelectedDetachment.Army.Units.Where(u => u.ForceOrgSlot == 6).ForEach(u=> FortificationUnits.Add(u));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedDetachment))
            {
                UpdateArmyUnitsListDataSource();
                UpdateForceOrgCount();
                UpdateUnitLists();
            }
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
            var groups = SelectedDetachment.Units.GroupBy(i => new {i.Unit.ForceOrgSlot}).Select(i => new ForceOrgGroup(i.ToList())).ToList();

            ArmyListDataGroups.Clear();
            groups.ForEach(g => ArmyListDataGroups.Add(g));
            SelectedUnit = currentSelectedItem;
            UpdateForceOrgCount();
        }

        private void UpdateForceOrgCount()
        {

            //ForceOrgCount.HqCount = Units.Count(u => u.Unit.ForceOrgSlot == 0 && u.Unit.CountsTowardForceOrg);
            //ForceOrgCount.TroopCount = Units.Count(u => u.Unit.ForceOrgSlot == 1 && u.Unit.CountsTowardForceOrg);
            //EliteCount = Units.Count(u => u.Unit.ForceOrgSlot == 2 && u.Unit.CountsTowardForceOrg);
            //FastAttackCount = Units.Count(u => u.Unit.ForceOrgSlot == 3 && u.Unit.CountsTowardForceOrg);
            //HeavySupportCount = Units.Count(u => u.Unit.ForceOrgSlot == 4 && u.Unit.CountsTowardForceOrg);
            //LordOfWarCount = Units.Count(u => u.Unit.ForceOrgSlot == 5 && u.Unit.CountsTowardForceOrg);
        }

        private void UpdatePointsTotal()
        {
            PointsUsed = SelectedDetachment.Units.Sum(a => a.PointsTotal);
            PointsRemaining = ArmyList.PointsLimit - PointsUsed;
        }

        public void AddUnit(Unit unit)
        {
            if (unit != null)
            {
                SelectedDetachment.Units.Add(new ArmyListData(unit, ArmyList.Id));
                SelectedUnit = SelectedDetachment.Units.Last();
                IsUnitFlyoutOpened = false;

            }
        }

        private void RemoveUnit(ArmyListData unit)
        {
            SelectedDetachment?.Units?.Remove(unit);
        }
    }
}