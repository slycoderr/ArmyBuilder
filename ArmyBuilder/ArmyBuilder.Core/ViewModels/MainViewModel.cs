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
        public ArmyList SelectedArmyList { get { return selectedArmyList; } set { SetValue(ref selectedArmyList, value); } }

        public ObservableCollection<ArmyList> ArmyLists => database.ArmyLists;
        public ObservableCollection<Army> Armies { get; } = new ObservableCollection<Army>();

        public RelayCommand<ArmyList> RemoveListCommand => new RelayCommand<ArmyList>(RemoveSelectedList);
        public RelayCommand AddListCommand => new RelayCommand(AddList);

        public static SynchronizationContext UiContext; 
        private readonly ArmyBuilderDatabase database = new ArmyBuilderDatabase();
        private ArmyList selectedArmyList;

        // ReSharper disable once EmptyConstructor
        public MainViewModel(){ }

        public Task LoadDatabase(string path)
        {
            return Task.Run(() =>
            {
                database.Load(path);

            });
        }

        public void LoadArmyData(params Stream[] dataStreams)
        {
            if (dataStreams == null || dataStreams.Length == 0)
            {
                throw new ArgumentException("Army data cannot be null or empty.");
            }

            foreach (var s in dataStreams)
            {
                using (var reader = XmlReader.Create(s))
                {
                    var dsArmy = new XmlSerializer(typeof(Army));
                    var army = (Army) dsArmy.Deserialize(reader);

                    Armies.Add(army); 

                    army.Configure();
                    
                    ArmyLists.Where(a => a.ArmyId == army.Id).ForEach(a => a.Army = army);
                }
            }
        }

        private void RemoveSelectedList(ArmyList list)
        {
            ArmyLists?.Remove(list);
        }

        private void AddList()
        {
            ArmyLists.Add(new ArmyList { Id = Guid.NewGuid(), Name = "New List", PointsLimit = 1500});
            SelectedArmyList = ArmyLists.Last();
        }

        private void UnitsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems.Cast<ArmyListData>().ForEach(d=> database.ArmyListData.Add(d));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems.Cast<ArmyListData>().ForEach(d => database.ArmyListData.Remove(d));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}