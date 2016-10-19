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

        public ObservableCollection<UnitEntry> HqUnits { get; } = new ObservableCollection<UnitEntry>();
        public ObservableCollection<UnitEntry> TroopUnits { get; } = new ObservableCollection<UnitEntry>();
        public ObservableCollection<UnitEntry> EliteUnits { get; } = new ObservableCollection<UnitEntry>();
        public ObservableCollection<UnitEntry> FastAttackUnits { get; } = new ObservableCollection<UnitEntry>();
        public ObservableCollection<UnitEntry> HeavySupportUnits { get; } = new ObservableCollection<UnitEntry>();
        public ObservableCollection<UnitEntry> LordOfWarUnits { get; } = new ObservableCollection<UnitEntry>();
        public ObservableCollection<UnitEntry> FortificationUnits { get; } = new ObservableCollection<UnitEntry>();

        public ArmyListData SelectedUnit { get { return selectedUnit; } set { SetValue(ref selectedUnit, value); } }

        public ObservableCollection<ForceOrgGroup> ArmyListDataGroups { get; } =new ObservableCollection<ForceOrgGroup>();
        public ObservableCollection<ArmyUnitGroup> ArmyUnitGroups { get; } = new ObservableCollection<ArmyUnitGroup>();

        public RelayCommand<UnitEntry> AddUnitCommand => new RelayCommand<UnitEntry>(AddUnit);
        public RelayCommand<Detachment> AddSubDetachmentCommand => new RelayCommand<Detachment>(AddSubDetachment);
        public RelayCommand<Detachment> AddDetachmentCommand => new RelayCommand<Detachment>(AddDetachment);
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
            

            PropertyChanged += OnPropertyChanged;
            SelectedDetachment = armyList.Detachments.FirstOrDefault();

            ArmyList.Detachments.ForEach(d=>d.Units.CollectionChanged += UnitsOnCollectionChanged);
            UpdatePointsTotal();
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

            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.HQ).ForEach(u=>HqUnits.Add(u));
            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.Troop).ForEach(u=> TroopUnits.Add(u));
            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.Elite).ForEach(u=> EliteUnits.Add(u));
            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.FastAttack).ForEach(u=> FastAttackUnits.Add(u));
            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.HeavySupport).ForEach(u=> HeavySupportUnits.Add(u));
            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.LordOfWar).ForEach(u=> LordOfWarUnits.Add(u));
            SelectedDetachment?.Army?.UnitEntries.Where(u => u.ForceOrgSlot == ForceOrgSlot.Fortification).ForEach(u=> FortificationUnits.Add(u));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedDetachment))
            {


                if (SelectedDetachment != null && SelectedDetachment.Detachment.Type == DetachmentType.BattleForged)
                {
                    //SelectedDetachment = SelectedDetachment.SelectedDetachments.FirstOrDefault();
                }

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
            var groups = ArmyList.Army.UnitEntries.GroupBy(i => new {i.ForceOrgSlot}).Select(i => new ArmyUnitGroup(i.ToList())).OrderBy(g => g.ForceOrgId);

            groups.ForEach(g => ArmyUnitGroups.Add(g));
        }

        private void UpdateArmyListDataSource()
        {
            var currentSelectedItem = SelectedUnit;
            var groups = SelectedDetachment?.Units?.GroupBy(i => new {i.UnitEntry.ForceOrgSlot}).Select(i => new ForceOrgGroup(i.ToList()))?.ToList();

            ArmyListDataGroups.Clear();
            groups?.ForEach(g => ArmyListDataGroups.Add(g));
            SelectedUnit = currentSelectedItem;
            UpdateForceOrgCount();
        }

        private void UpdateForceOrgCount()
        {

            //ForceOrgCount.HqCount = UnitEntries.Count(u => u.UnitEntry.ForceOrgSlot == 0 && u.UnitEntry.CountsTowardForceOrg);
            //ForceOrgCount.TroopCount = UnitEntries.Count(u => u.UnitEntry.ForceOrgSlot == 1 && u.UnitEntry.CountsTowardForceOrg);
            //EliteCount = UnitEntries.Count(u => u.UnitEntry.ForceOrgSlot == 2 && u.UnitEntry.CountsTowardForceOrg);
            //FastAttackCount = UnitEntries.Count(u => u.UnitEntry.ForceOrgSlot == 3 && u.UnitEntry.CountsTowardForceOrg);
            //HeavySupportCount = UnitEntries.Count(u => u.UnitEntry.ForceOrgSlot == 4 && u.UnitEntry.CountsTowardForceOrg);
            //LordOfWarCount = UnitEntries.Count(u => u.UnitEntry.ForceOrgSlot == 5 && u.UnitEntry.CountsTowardForceOrg);
        }

        private void UpdatePointsTotal()
        {
            PointsUsed = SelectedDetachment?.Units?.Sum(a => a.PointsTotal) ?? 0;
            PointsRemaining = ArmyList.PointsLimit - PointsUsed;
        }

        public void AddUnit(UnitEntry unitEntry)
        {
            if (unitEntry != null)
            {
                SelectedDetachment.Units.Add(new ArmyListData(unitEntry, ArmyList.Id));
                SelectedUnit = SelectedDetachment.Units.Last();
                IsUnitFlyoutOpened = false;

            }
        }

        private void AddDetachment(Detachment detachment)
        {
            ArmyList?.Detachments?.Add(new DetachmentData(detachment, ArmyList.Army));
        }

        private void AddSubDetachment(Detachment detachment)
        {
            //SelectedDetachment?.SelectedDetachments.Add(new DetachmentData(detachment, SelectedDetachment.Army));
        }

        private void RemoveUnit(ArmyListData unit)
        {
            //SelectedDetachment?.Units?.Remove(unit);
        }
    }
}