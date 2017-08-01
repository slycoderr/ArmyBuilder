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

        public DetachmentData SelectedDetachment { get => selectedDetachment; set => SetValue(ref selectedDetachment, value); }

        public ArmyListData SelectedUnit
        {
            get => selectedUnit;
            set
            {
                //a hack to make only one unit selected at once in mutiple listviews
                selectedUnit = null;
                OnPropertyChanged(nameof(SelectedUnit));
                SetValue(ref selectedUnit, value);

                //if a unit is selected in another detachment, select the detachment its in
                if (value != null)
                {
                    SetValue(ref selectedDetachment, value.Detachment);
                }
            }
        }

        public int PointsRemaining { get => pointsRemaining; set => SetValue(ref pointsRemaining, value); }
        public int PointsUsed { get => pointsUsed; set => SetValue(ref pointsUsed, value); }
        public int CommandPoints { get => commandPoints; set => SetValue(ref commandPoints, value); }



        public RelayCommand<Detachment> AddDetachmentToListCommand => new RelayCommand<Detachment>(AddDetachmentToList);
        public RelayCommand<UnitEntry> AddUnitEntryToDetachmentCommand => new RelayCommand<UnitEntry>((u)=> { AddUnitEntryToDetachment( SelectedDetachment, u); });
        public RelayCommand RemoveUnitCommand => new RelayCommand(()=> RemoveUnit(SelectedDetachment, SelectedUnit));
        public RelayCommand RemoveDetachmentCommand => new RelayCommand(()=> RemoveDetachment(ArmyList, SelectedDetachment));


        private ArmyListData selectedUnit;
        private int pointsRemaining;
        private int pointsUsed;
        private DetachmentData selectedDetachment;
        private readonly MainViewModel mainViewModel;
        private int commandPoints;

        public ArmyListViewModel( MainViewModel mv, ArmyList armyList)
        {
            mainViewModel = mv;
            ArmyList = armyList;

            PropertyChanged += OnPropertyChanged;
            SelectedDetachment = armyList.Detachments.FirstOrDefault();

            ArmyList.Detachments.SelectMany(d=>d.DetachmentRequirementData).ForEach(d=>d.Units.CollectionChanged += UnitsOnCollectionChanged);
            ArmyList.Detachments.CollectionChanged += DetachmentsOnCollectionChanged;
            UpdatePointsTotal();
        }

        private void DetachmentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePointsTotal();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems.Cast<DetachmentData>().SelectMany(d => d.DetachmentRequirementData).ForEach(d => d.Units.CollectionChanged += UnitsOnCollectionChanged);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems.Cast<DetachmentData>().SelectMany(d => d.DetachmentRequirementData).ForEach(d => d.Units.CollectionChanged -= UnitsOnCollectionChanged);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void AddUnitEntryToDetachment(DetachmentData detachment, UnitEntry unit)
        {
                detachment.DetachmentRequirementData.FirstOrDefault(d => d.Requirement.Slot == unit.ForceOrgSlot)?.Units?.Add(SelectedUnit = new ArmyListData(unit, detachment, unit.Army));
        }

        private void RemoveUnit(DetachmentData detach, ArmyListData unit)
        {
            detach.DetachmentRequirementData.FirstOrDefault(d => d.Requirement.Slot == unit.UnitEntry.ForceOrgSlot).Units.Remove(unit);
        }
        private void RemoveDetachment(ArmyList list, DetachmentData detachment)
        {
            list.Detachments.Remove(detachment);
        }


        internal void AddDetachmentToList(Detachment detachment)
        {

            ArmyList.Detachments.Add(SelectedDetachment = new DetachmentData(detachment));
            
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedDetachment))
            {
                

            }
        }

        private void UnitsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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


        private void UpdatePointsTotal()
        {
            PointsUsed = ArmyList.Detachments.SelectMany(l=>l.DetachmentRequirementData)?.SelectMany(d=>d.Units)?.Sum(a => a.PointsTotal) ?? 0;
            PointsRemaining = ArmyList.PointsLimit - PointsUsed;
            CommandPoints = 3 + ArmyList.Detachments.Sum(d => d.Detachment.BonusCommandPoints);
        }
    }
}