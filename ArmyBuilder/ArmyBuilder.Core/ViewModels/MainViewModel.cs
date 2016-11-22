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

using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Models.Groups;
using GalaSoft.MvvmLight.Command;
using MoreLinq;
using Slycoder.MVVM;

namespace ArmyBuilder.Core.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ParserEngineViewModel ParserEngineViewModel { get; }


        public ArmyList SelectedArmyList { get { return selectedArmyList; } set { SetValue(ref selectedArmyList, value); } }
        public DetachmentData SelectedDetachment { get { return selectedDetachment; } set { SetValue(ref selectedDetachment, value); } }


        public ObservableCollection<Army> Armies { get; } = new ObservableCollection<Army>();

        public RelayCommand<ArmyList> RemoveListCommand => new RelayCommand<ArmyList>(RemoveSelectedList);
        public RelayCommand AddListCommand => new RelayCommand(AddList);

        public static SynchronizationContext UiContext; 

        private ArmyList selectedArmyList;
        private DetachmentData selectedDetachment;

        // ReSharper disable once EmptyConstructor
        public MainViewModel()
        {
            ParserEngineViewModel = new ParserEngineViewModel(this);
            PropertyChanged += MainViewModel_PropertyChanged;
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public void LoadDatabase(string path)
        {
            //database.Load(path);
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

                    //army.Configure();
                    
                    //ArmyLists.Where(a => a.ArmyId == army.Id).ForEach(a => a.Army = army);
                }
            }
        }

        private void RemoveSelectedList(ArmyList list)
        {
            //ArmyLists?.Remove(list);
        }

        private void AddList()
        {
            //ArmyLists.Add(new ArmyList { Id = Guid.NewGuid(), Name = "New List", PointsLimit = 1500});
            //SelectedArmyList = ArmyLists.Last();
        }

    }
}