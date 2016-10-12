using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyBuilder.Core.Models;
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

        public Core.ViewModels.MainViewModel ArmyBuilderCore { get; } = new Core.ViewModels.MainViewModel();

        public MainViewModel()
        {
            using (var darkEldarStream = new FileStream("C:\\Users\\adkerti\\OneDrive\\DarkEldar.xml", FileMode.Open))
            {
                ArmyBuilderCore.LoadArmyData(darkEldarStream);
            }
        }

        private void SaveXML()
        {

        }

        private void AddUnitEntry()
        {
            
        }

        private void RemoveUnitEntry()
        {

        }

        private void AddUnit()
        {

        }

        private void RemoveUnit()
        {

        }

        private void AddEquipmentToDefinitions()
        {

        }

        private void RemoveEquipmentToDefinitions()
        {

        }
    }
}
