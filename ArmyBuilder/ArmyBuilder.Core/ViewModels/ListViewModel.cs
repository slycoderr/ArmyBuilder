using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyBuilder.Core.Models;
using GalaSoft.MvvmLight.Command;
using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.ViewModels
{
    public class ListViewModel : BindableBase
    {
        public IArmyList SelectedArmyList { get { return selectedArmyList; } set { SetValue(ref selectedArmyList, value); } }
        public DetachmentData SelectedDetachment { get { return selectedDetachment; } set { SetValue(ref selectedDetachment, value); } }
        public string SelectedDataItem { get { return selectedDataItem; } set { SetValue(ref selectedDataItem, value); } }


        public ObservableCollection<IArmyList> ArmyLists { get; } = new ObservableCollection<IArmyList>();

        public RelayCommand<ArmyList> RemoveListCommand => new RelayCommand<ArmyList>(RemoveList);
        public RelayCommand AddListCommand => new RelayCommand(()=>AddList(GameSystem.AoS));

        private IArmyList selectedArmyList;
        private DetachmentData selectedDetachment;


        private readonly MainViewModel mainViewModel;
        private string selectedDataItem;

        public ListViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            AddList(GameSystem.AoS);
        }

        private void RemoveList(ArmyList list)
        {
            ArmyLists?.Remove(list);
        }

        private void AddList(GameSystem system)
        {
            IArmyList list;

            switch (system)
            {
                case GameSystem.W40K:
                    list = new ArmyList() { Id = Guid.NewGuid(), Name = "New List"};
                    break;
                case GameSystem.AoS:
                    list = new AgeOfSigmarArmyList() { Name = "New List"};
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }

            ArmyLists.Add(list);
            SelectedArmyList = list;
            list.Data.Add("");
            SelectedDataItem = list.Data.Last();
        }

        public void UpdateListCost()
        {
            SelectedArmyList.CurrentPointsTotal = (uint)SelectedArmyList.Detachments.SelectMany(u => u.Units).Sum(u => u.Count * u.Unit.CostPerModel + u.Unit.BaseCost);
        }
    }
}
