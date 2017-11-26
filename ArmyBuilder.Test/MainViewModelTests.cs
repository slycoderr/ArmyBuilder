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
        public void TestArmyDataLoad()
        {
            MainViewModel mv = new MainViewModel();
            string path = "C:\\Users\\adkerti\\OneDrive\\DarkEldar.xml";
            int checkUnitEntryId = 1;
            int checkArmyId = 4;
            int numUnits = 1;
            int numTransports = 1;
            int numDefaultEquip = 3;
            int numUpgrades = 10;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                mv.LoadArmyData(stream);
            }

            var army = mv.Armies.First(a => a.Id == checkArmyId); //make sure the army is there
            var entry = army.UnitEntries.First(u => u.Id == checkUnitEntryId);

            Assert.AreEqual(numTransports, entry.DedicatedTransports?.Count ?? 0);
            Assert.AreEqual(numUnits, entry.Units.Count);
            Assert.AreEqual(numDefaultEquip, entry.Units.First().DefaultEquipment.Count);
            Assert.AreEqual(numUpgrades, entry.Units.First().Upgrades.Count);
        }
    }
}
