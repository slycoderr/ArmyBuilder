using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core.Database;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;
using GalaSoft.MvvmLight.Command;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ArmyListViewModel CurrentArmyListViewModel { get { return currentArmyListViewModel; } private set { SetValue(ref currentArmyListViewModel, value); } }
        public ArmyList SelectedArmyList { get { return selectedArmyList; } set { SetValue(ref selectedArmyList, value); } }

        public ObservableCollection<ArmyList> ArmyLists => database.ArmyLists;
        public ObservableCollection<ArmyListGroup> ArmyListGroups { get; } = new ObservableCollection<ArmyListGroup>();
        public ObservableCollection<Army> Armies { get; } = new ObservableCollection<Army>();

        public RelayCommand<ArmyList> RemoveSelectedListCommand => new RelayCommand<ArmyList>(RemoveSelectedList);
        public RelayCommand EditSelectedListCommand => new RelayCommand(EditSelectedArmyList);

        public static SynchronizationContext UiContext; 
        private readonly ArmyBuilderDatabase database = new ArmyBuilderDatabase();
        private ArmyListViewModel currentArmyListViewModel;
        private ArmyList selectedArmyList;

        // ReSharper disable once EmptyConstructor
        public MainViewModel(){ }

        public Task Load(string path, List<Stream> dataStreams)
        {
            return Task.Run(() =>
            {
                database.Load(path);
                LoadArmyData(dataStreams);
                ArmyLists.CollectionChanged += (sender, args) => { UpdateListSource(false); };
                UpdateListSource(true);
            });
        }

        private void LoadArmyData(List<Stream> dataStreams)
        {
            foreach (var s in dataStreams)
            {
                using (var reader = XmlReader.Create(s))
                {
                    var dsArmy = new XmlSerializer(typeof (Army));
                    var army = (Army) dsArmy.Deserialize(reader);

                    UiContext.Post(action=> { Armies.Add(army); }, null );

                    ArmyLists.Where(a=>a.ArmyId == army.Id).ForEach(a=> a.Army = army);
                }

                s.Dispose();
            }
        }

        private void RemoveSelectedList(ArmyList list)
        {
            ArmyLists?.Remove(list);
        }

        public void AddList(ArmyList newList)
        {
            ArmyLists?.Add(newList);
        }

        public void EditSelectedArmyList()
        {
            CurrentArmyListViewModel = null;

            if (SelectedArmyList != null)
            {
                CurrentArmyListViewModel = new ArmyListViewModel(selectedArmyList);
                database.ArmyListData.Where(a=>a.ArmyListId == SelectedArmyList.Id).ForEach(a =>
                {
                    a.SetData(SelectedArmyList.Army.Units.Single(u => u.Id == a.UnitId));
                    //CurrentArmyListViewModel.Units.Add(a);
                });
                //CurrentArmyListViewModel.Units.CollectionChanged += UnitsOnCollectionChanged;
            }
        }

        private void UnitsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems.Cast<ArmyListData>().ForEach(d=> database.ArmyListData.Add(d));
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems.Cast<ArmyListData>().ForEach(d => database.ArmyListData.Remove(d));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateListSource(bool cacheSelection)
        {
            var currentSelectedItem = SelectedArmyList;
            var groups = ArmyLists?.OrderBy(i => i.Army.Name).GroupBy(i => new {i.ArmyId}).Select(i => new ArmyListGroup(i.ToList())).ToList();

            UiContext.Post(action =>
            {
                ArmyListGroups.Clear();
                groups.ForEach(g => ArmyListGroups.Add(g));
            }, null);

            SelectedArmyList = cacheSelection ? currentSelectedItem : null;
        }
    }
}