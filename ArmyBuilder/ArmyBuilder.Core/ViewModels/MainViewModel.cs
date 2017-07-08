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
        public string DataRootDirectory { get; set; }
        public string ArmyDataDirectory => Path.Combine(DataRootDirectory, "Data");
        public string ArmyListDirectory => Path.Combine(DataRootDirectory, "ArmyLists");

        public ArmyList SelectedArmyList { get => selectedArmyList; set => SetValue(ref selectedArmyList, value); }
        public DetachmentData SelectedDetachment { get => selectedDetachment; set => SetValue(ref selectedDetachment, value); }
        public ArmyListData SelectedUnit { get => selectedUnit; set => SetValue(ref selectedUnit, value); }

        public ObservableCollection<ArmyList> ArmyLists { get; } = new ObservableCollection<ArmyList>();
        public ObservableCollection<Army> Armies { get; } = new ObservableCollection<Army>();
        public ObservableCollection<UnitEntry> AvailableUnitEntries { get; } = new ObservableCollection<UnitEntry>();

        public RelayCommand<ArmyList> RemoveListCommand => new RelayCommand<ArmyList>(RemoveSelectedList);
        public RelayCommand<Detachment> AddDetachmentToListCommand => new RelayCommand<Detachment>(AddDetachmentToList);
        public RelayCommand<UnitEntry> AddUnitEntryToDetachmentCommand => new RelayCommand<UnitEntry>(AddUnitEntryToDetachment);
        public RelayCommand AddListCommand => new RelayCommand(AddList);

        public IPlatformService PlatformService { get; set; }
        public static SynchronizationContext UiContext;

        private ArmyList selectedArmyList;
        private DetachmentData selectedDetachment;
        private ArmyListData selectedUnit;

    

    // ReSharper disable once EmptyConstructor
        public MainViewModel()
        {
            PropertyChanged += MainViewModel_PropertyChanged;



        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public async Task Load(string baseDirectory)
        {
            DataRootDirectory = baseDirectory;
            PlatformService.CreateDirectory(ArmyDataDirectory);
            PlatformService.CreateDirectory(ArmyListDirectory);

            (await PlatformService.DiscoverXmlFiles(ArmyListDirectory)).Select(n => new ArmyList {Name = Path.GetFileNameWithoutExtension(n)}).ForEach(ArmyLists.Add);
            var dataFiles = (await PlatformService.DiscoverXmlFiles(ArmyDataDirectory));

            foreach (var file in dataFiles)
            {
                var army = await PlatformService.DeserializeXml<Army>(file);
                Armies.Add(army);

                army.Configure();

                ArmyLists.Where(a => a.ArmyId == army.Id).ForEach(a => a.Army = army);

                army.UnitEntries.ForEach(AvailableUnitEntries.Add);
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

        private void AddUnitEntryToDetachment(UnitEntry unit)
        {
            if (SelectedDetachment == null)
            {

            }

            else
            {
                SelectedDetachment.Units.Add(new ArmyListData(unit, Guid.Empty));
            }

            
        }

        private void AddDetachmentToList(Detachment detachment)
        {
            if (SelectedArmyList == null)
            {

            }

            else
            {
            var detach =new DetachmentData(detachment) ;
            
            

SelectedArmyList.Detachments.Add(detach);SelectedDetachment=detach; 
​


            }
        }

        public async Task SaveArmyList(ArmyList list)
        {
            await PlatformService.SerializeXml<ArmyList>(list, Path.Combine(ArmyListDirectory, list.Name));
        }
    }
}