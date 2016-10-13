using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace ArmyBuilder.XMLEditor
{
    public class MainViewModel
    {
        public Army SelectedArmy { get; set; }
        public Equipment SelectedEquipmentDefinition { get; set; }
        public UnitEntry SelectedUnitEntry { get; set; }
        public Unit SelectedUnit { get; set; }
        public Equipment SelectedDefaultEquipment { get; set; }
        public Equipment SelectedUpgrade { get; set; }

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

        public Core.ViewModels.MainViewModel ArmyBuilderCore { get; } = new Core.ViewModels.MainViewModel();

        public MainViewModel()
        {
            using (var darkEldarStream = new FileStream("C:\\Users\\slyco\\OneDrive\\DarkEldar.xml", FileMode.Open))
            {
                ArmyBuilderCore.LoadArmyData(darkEldarStream);
            }
        }

        private void CreateArmy()
        {
            SelectedArmy = new Army {Id = ArmyBuilderCore.Armies.Max(a => a.Id) + 1};
            SaveXML();
        }

        private void SaveXML()
        {
            if (SelectedArmy != null)
            {
                SelectedArmy.Version++;

                try
                {
                    using (var s = new FileStream($"{SelectedArmy.Name} - {SelectedArmy.Version}.xml", FileMode.Truncate))
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
                SelectedArmy.UnitEntries.Add(new UnitEntry { Id = SelectedArmy.UnitEntries.Max(a => a.Id) + 1 });
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
                SelectedUnitEntry.Units.Add(new Unit { Id = SelectedUnitEntry.Units.Max(a => a.Id) + 1 });
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
                SelectedArmy.EquipmentDefinitions.Add(new Equipment { Id = SelectedArmy.EquipmentDefinitions.Max(a => a.Id) + 1 });
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
                SelectedDefaultEquipment.ReplacementOptions.Add(SelectedEquipmentDefinition);
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
                SelectedUpgrade.ReplacementOptions.Add(SelectedEquipmentDefinition);
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
    }
}
