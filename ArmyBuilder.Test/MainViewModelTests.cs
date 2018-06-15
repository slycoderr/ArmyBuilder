using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Test
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public async Task TestArmyDataLoad()
        {
            MainViewModel mv = new MainViewModel();
            //get path to repo data folder
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.EnumerateDirectories()
                .First(d => d.Name.Contains("ArmyBuilder.Core"))
                .FullName;
            int checkUnitEntryId = 1;
            int checkArmyId = 4;
            int numUnits = 1;
            int numTransports = 1;
            int numDefaultEquip = 3;
            int numUpgrades = 10;

                await mv.Load(path);

            //var army = mv.Armies.First(a => a.Id == checkArmyId); //make sure the army is there
            //var entry = army.UnitEntries.First(u => u.Id == checkUnitEntryId);

            //Assert.AreEqual(numTransports, entry.DedicatedTransports?.Count ?? 0);
            //Assert.AreEqual(numUnits, entry.Units.Count);
            //Assert.AreEqual(numDefaultEquip, entry.Units.First().DefaultEquipment.Count);
            //Assert.AreEqual(numUpgrades, entry.Units.First().Upgrades.Count);
        }
    }
}
