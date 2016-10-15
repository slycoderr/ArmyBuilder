using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Slycoder.MVVM;

namespace ArmyBuilder.XMLEditor
{
    public class MainViewModel : BindableBase
    {
        private Army selectedArmy;
        private Equipment selectedEquipmentDefinition;
        private UnitEntry selectedUnitEntry;
        private Unit selectedUnit;
        private Equipment selectedDefaultEquipment;
        private Equipment selectedUpgrade;
        public Army SelectedArmy { get { return selectedArmy; } set { SetValue(ref selectedArmy, value); } }

        public Equipment SelectedEquipmentDefinition { get { return selectedEquipmentDefinition; } set { SetValue(ref selectedEquipmentDefinition, value); } }

        public UnitEntry SelectedUnitEntry { get { return selectedUnitEntry; } set { SetValue(ref selectedUnitEntry, value); } }

        public Unit SelectedUnit { get { return selectedUnit; } set { SetValue(ref selectedUnit, value); } }

        public Equipment SelectedDefaultEquipment { get { return selectedDefaultEquipment; } set { SetValue(ref selectedDefaultEquipment, value); } }

        public Equipment SelectedUpgrade { get { return selectedUpgrade; } set { SetValue(ref selectedUpgrade, value); } }

        public RelayCommand CreateArmyCommand => new RelayCommand(CreateArmy);
        public RelayCommand SaveXMLCommand => new RelayCommand(SaveXML);
        public RelayCommand AddUnitEntryCommand => new RelayCommand(AddUnitEntry);
        public RelayCommand RemoveUnitEntryCommand => new RelayCommand(RemoveUnitEntry);
        public RelayCommand AddUnitCommand => new RelayCommand(AddUnit);
        public RelayCommand RemoveUnitCommand => new RelayCommand(RemoveUnit);
        public RelayCommand AddEquipmentToDefinitionsCommand => new RelayCommand(AddEquipmentToDefinitions);
        public RelayCommand RemoveEquipmentToDefinitionsCommand => new RelayCommand(RemoveEquipmentToDefinitions);
        public RelayCommand AddToDefaultEquipmentReplacements => new RelayCommand(AddEquipmentToDefaultEquipReplacements);
        public RelayCommand AddToDefaultEquipmentGiven => new RelayCommand(AddEquipmentToDefaultEquipGiven);
        public RelayCommand AddToUpgradesReplacements => new RelayCommand(AddEquipmentToUpgradeEquipReplacements);
        public RelayCommand AddToUpgradesGiven => new RelayCommand(AddEquipmentToUpgradeEquipGiven);
        public RelayCommand<UnitEntry> AddDedicatedTransportCommand => new RelayCommand<UnitEntry>(AddDedicatedTransport);
        public RelayCommand<UnitEntry> RemoveDedicatedTransportCommand => new RelayCommand<UnitEntry>(RemoveDedicatedTransport);

        public Core.ViewModels.MainViewModel ArmyBuilderCore { get; } = new Core.ViewModels.MainViewModel();

        public MainViewModel()
        {
            var files = Directory.EnumerateFiles("DataFiles/").Where(f => f.ToLower().Contains(".xml")).ToList();
            var streams = files.Select(f => new FileStream(f, FileMode.Open)).ToList();

            ArmyBuilderCore.LoadArmyData(streams.ToArray());
            streams.ForEach(s=>s.Dispose());
        }

        private void CreateArmy()
        {
            ArmyBuilderCore.Armies.Add(new Army {Id = ArmyBuilderCore.Armies.Count > 0 ? ArmyBuilderCore.Armies.Max(a => a.Id) + 1 : 1, Name = "New Army"});
            SelectedArmy = ArmyBuilderCore.Armies.Last();
            SaveXML();
        }

        private void SaveXML()
        {
            if (SelectedArmy != null)
            {
                SelectedArmy.Version++;

                try
                {
                    var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "DataFiles", $"{SelectedArmy.Name}.xml");

                    using (var s = new FileStream(path, FileMode.Truncate))
                    {
                        using (var reader = XmlWriter.Create(s))
                        {
                            var dsArmy = new XmlSerializer(typeof(Army));
                            dsArmy.Serialize(reader, SelectedArmy);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error - Failed to save XML", ex.Message);
                }
            }

            else
            {
                MessageBox.Show(nameof(SelectedArmy)+" is null.");
            }
        }

        private void AddUnitEntry()
        {
            if (SelectedArmy != null)
            {
                SelectedArmy.UnitEntries.Add(new UnitEntry { Id = SelectedArmy.UnitEntries.Count > 0 ? SelectedArmy.UnitEntries.Max(a => a.Id) + 1 : 1, Name = "New Entry"});
                SelectedUnitEntry = SelectedArmy.UnitEntries.Last();
            }

            else
            {
                MessageBox.Show(nameof(SelectedArmy) + " is null.");
            }
        }

        private void RemoveUnitEntry()
        {
            if (SelectedUnitEntry != null)
            {
                SelectedArmy.UnitEntries.Remove(SelectedUnitEntry);
            }

            else
            {
                MessageBox.Show(nameof(SelectedUnitEntry) + " is null.");
            }
        }

        private void AddUnit()
        {
            if (SelectedUnitEntry != null)
            {
                SelectedUnitEntry.Units.Add(new Unit { Id = SelectedUnitEntry.Units.Count > 0 ? SelectedUnitEntry.Units.Max(a => a.Id) + 1 : 1, Name = "New Unit", UnitEntryId = SelectedUnitEntry.Id});
                SelectedUnit = SelectedUnitEntry.Units.Last();
            }

            else
            {
                MessageBox.Show(nameof(SelectedUnitEntry) + " is null.");
            }
        }

        private void RemoveUnit()
        {
            if (SelectedUnit != null)
            {
                SelectedUnitEntry.Units.Remove(SelectedUnit);
            }

            else
            {
                MessageBox.Show(nameof(SelectedUnit) + " is null.");
            }
        }

        private void AddEquipmentToDefinitions()
        {
            if (SelectedArmy != null)
            {
                SelectedArmy.EquipmentDefinitions.Add(new Equipment { Id = SelectedArmy.EquipmentDefinitions.Count > 0 ? SelectedArmy.EquipmentDefinitions.Max(a => a.Id) + 1 : 1, Name = "New Equipment"});
                SelectedEquipmentDefinition = SelectedArmy.EquipmentDefinitions.Last();
            }

            else
            {
                MessageBox.Show(nameof(SelectedArmy) + " is null.");
            }
        }

        private void RemoveEquipmentToDefinitions()
        {
            if (SelectedEquipmentDefinition != null)
            {
                SelectedArmy.EquipmentDefinitions.Remove(SelectedEquipmentDefinition);
            }

            else
            {
                MessageBox.Show(nameof(SelectedEquipmentDefinition) + " is null.");
            }
        }

        /// <summary>
        /// Adds the selected equipment definition to the DefaultEquipment sections selected item's ReplacementOptions
        /// </summary>
        private void AddEquipmentToDefaultEquipReplacements()
        {
            if (SelectedEquipmentDefinition != null && SelectedUnit != null)
            {
                if (SelectedDefaultEquipment != null)
                {
                    SelectedDefaultEquipment.ReplacementOptions.Add(SelectedEquipmentDefinition);
                }

                else
                {
                    SelectedUnit.DefaultEquipment.Add(SelectedEquipmentDefinition);
                }
            }

            else
            {
                MessageBox.Show(nameof(SelectedEquipmentDefinition) + " is null.");
            }
        }

        /// <summary>
        /// Adds the selected equipment definition to the DefaultEquipment sections selected item's GivenEquipment
        /// </summary>
        private void AddEquipmentToDefaultEquipGiven()
        {
            if (SelectedEquipmentDefinition != null && SelectedUnit != null)
            {
                
                SelectedDefaultEquipment.GivenEquipment.Add(SelectedEquipmentDefinition);
            }

            else
            {
                MessageBox.Show(nameof(SelectedEquipmentDefinition) + " is null.");
            }
        }

        /// <summary>
        /// Adds the selected equipment definition to the Upgrades sections selected item's ReplacementOptions
        /// </summary>
        private void AddEquipmentToUpgradeEquipReplacements()
        {
            if (SelectedEquipmentDefinition != null && SelectedUnit != null)
            {
                if (SelectedUpgrade != null)
                {

                    SelectedUpgrade.ReplacementOptions.Add(SelectedEquipmentDefinition);
                }

                else
                {
                    SelectedUnit.Upgrades.Add(SelectedEquipmentDefinition);
                }
            }

            else
            {
                MessageBox.Show(nameof(SelectedEquipmentDefinition) + " is null.");
            }
        }

        /// <summary>
        /// Adds the selected equipment definition to the Upgrades sections selected item's GivenEquipment
        /// </summary>
        private void AddEquipmentToUpgradeEquipGiven()
        {
            if (SelectedEquipmentDefinition != null && SelectedUnit != null)
            {
                SelectedUpgrade.GivenEquipment.Add(SelectedEquipmentDefinition);
            }

            else
            {
                MessageBox.Show(nameof(SelectedEquipmentDefinition) + " is null.");
            }
        }

        private void AddDedicatedTransport(UnitEntry newTransport)
        {
            if (SelectedUnitEntry != null && newTransport != null)
            {
                SelectedUnitEntry.DedicatedTransports.Add(newTransport.Units.First());
                newTransport.Units.First().UnitEntryId = newTransport.Id;
            }
        }

        private void RemoveDedicatedTransport(UnitEntry transport)
        {
            if (SelectedUnitEntry != null && transport != null)
            {
                SelectedUnitEntry.DedicatedTransports.Remove(transport.Units.First());
            }
        }

        private void UpdateAllDedicatedTransports()
        {
            if (SelectedArmy != null)
            {
                foreach (var entry in SelectedArmy.UnitEntries)
                {
                    foreach (var tr in entry.DedicatedTransports.ToList())
                    {
                        
                    }   
                }
            }
        }
    }
}
