﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArmyBuilder.Core.Models;
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

        public ObservableCollection<Detachment> AvailableDetachments { get; } = new ObservableCollection<Detachment>();
        public ObservableCollection<ArmyList> ArmyLists { get; } = new ObservableCollection<ArmyList>();
        public ObservableCollection<Army> Armies { get; } = new ObservableCollection<Army>();
        public ObservableCollection<UnitEntry> AvailableUnitEntries { get; } = new ObservableCollection<UnitEntry>();
        public ArmyListViewModel ArmyListEditor { get => armyListEditor; private set => SetValue(ref armyListEditor, value); }

        public RelayCommand<ArmyList> RemoveListCommand => new RelayCommand<ArmyList>(RemoveSelectedList);
        public RelayCommand<ArmyList> SaveListCommand => new RelayCommand<ArmyList>(async s => await SaveArmyList(s));

        public RelayCommand AddListCommand => new RelayCommand(async ()=>await AddList());

        public IPlatformService PlatformService { get; set; }

        private ArmyList selectedArmyList;
        private ArmyListViewModel armyListEditor;

        public MainViewModel()
        {
            PropertyChanged += MainViewModel_PropertyChanged;
        }

        private async void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedArmyList) && SelectedArmyList != null)
            {

                if ((await PlatformService.DiscoverXmlFiles(ArmyListDirectory)).Any(l => l.Contains(SelectedArmyList.Name)))
                {
                    var data = await PlatformService.DeserializeXml<ArmyList>(Path.Combine(ArmyListDirectory, SelectedArmyList.Name + ".xml"));
                    SelectedArmyList.Detachments = data.Detachments;
                    SelectedArmyList.PointsLimit = data.PointsLimit;

                    SelectedArmyList.Load(AvailableDetachments);

                    foreach (var unit in SelectedArmyList.Detachments.SelectMany(d=>d.DetachmentRequirementData).SelectMany(u=>u.Units))
                    {
                        unit.SetData(Armies.First(a=>a.Id == unit.ArmyId).UnitEntries.First(u=>u.Id == unit.UnitId));
                    }
                }

                ArmyListEditor = new ArmyListViewModel(this, SelectedArmyList);

            }
        }

        public async Task Load(string baseDirectory)
        {
            DataRootDirectory = baseDirectory;
            PlatformService.CreateDirectory(ArmyDataDirectory);
            PlatformService.CreateDirectory(ArmyListDirectory);

            (await PlatformService.DiscoverXmlFiles(ArmyListDirectory)).Select(n => new ArmyList {Name = Path.GetFileNameWithoutExtension(n)}).ForEach(ArmyLists.Add);
            var dataFiles = await PlatformService.DiscoverXmlFiles(ArmyDataDirectory);

            foreach (var file in dataFiles.Where(f=>!f.Contains("Detachments")))
            {
                var army = await PlatformService.DeserializeXml<Army>(file);
                Armies.Add(army);

                army.Configure();

                army.UnitEntries.ForEach(AvailableUnitEntries.Add);
            }

            var detachmentFile = dataFiles.FirstOrDefault(f => f.Contains("Detachments"));
            (await PlatformService.DeserializeXml<DetachmentCollection>(detachmentFile)).Detachments.ForEach(AvailableDetachments.Add);


            if (Debugger.IsAttached)
            {
                await AddList();
                ArmyListEditor.AddDetachmentToList(AvailableDetachments.ElementAt(1));
            }
        }

        private void RemoveSelectedList(ArmyList list)
        {
            ArmyLists?.Remove(list);
        }


        private async Task AddList()
        {
            var files = await PlatformService.DiscoverXmlFiles(ArmyListDirectory);
            var count = 0;

            while (files.Select(Path.GetFileNameWithoutExtension).Any(f => f == "New List " + count))
            {
                count++;
            }

            ArmyLists.Add(SelectedArmyList = new ArmyList {Name = "New List " + count, PointsLimit = 1500});
            
        }


        internal async Task SaveArmyList(ArmyList list, string oldName = "")
        {
            await PlatformService.SerializeXml<ArmyList>(list, ArmyListDirectory, oldName, list.Name);
        }
    }
}