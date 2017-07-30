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
            }
        }

        public int PointsRemaining { get => pointsRemaining; set => SetValue(ref pointsRemaining, value); }
        public int PointsUsed { get => pointsUsed; set => SetValue(ref pointsUsed, value); }



        public RelayCommand<Detachment> AddDetachmentToListCommand => new RelayCommand<Detachment>(AddDetachmentToList);
        public RelayCommand<UnitEntry> AddUnitEntryToDetachmentCommand => new RelayCommand<UnitEntry>(AddUnitEntryToDetachment);
        public RelayCommand<ArmyListData> RemoveUnitCommand => new RelayCommand<ArmyListData>(RemoveUnit);


        private ArmyListData selectedUnit;
        private int pointsRemaining;
        private int pointsUsed;
        private DetachmentData selectedDetachment;
        private readonly MainViewModel mainViewModel;

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


        private void AddUnitEntryToDetachment(UnitEntry unit)
        {
            if (SelectedDetachment == null)
            {

            }

            else
            {
                SelectedDetachment.DetachmentRequirementData.FirstOrDefault(d => d.Requirement.Slot == unit.ForceOrgSlot)?.Units?.Add(SelectedUnit = new ArmyListData(unit));
            }


        }

        private void RemoveUnit(ArmyListData unit)
        {
            //SelectedDetachment?.DetachmentRequirementData.
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
            PointsUsed = SelectedDetachment?.DetachmentRequirementData.SelectMany(d=>d.Units)?.Sum(a => a.PointsTotal) ?? 0;
            PointsRemaining = ArmyList.PointsLimit - PointsUsed;
        }
    }
}