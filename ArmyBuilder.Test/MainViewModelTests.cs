using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArmyBuilder.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            using (var stream = new FileStream(path, FileMode.Open))
            {
                mv.LoadArmyData(stream);
            }

            Assert.AreEqual(1, mv.Armies.Count);
            Assert.AreEqual(2, mv.Armies.FirstOrDefault().UnitEntries.Count);
            Assert.AreEqual(1, mv.Armies.FirstOrDefault().UnitEntries.FirstOrDefault().DedicatedTransports.Count);
            Assert.AreEqual(1, mv.Armies.FirstOrDefault().UnitEntries.FirstOrDefault().Units.Count);
            Assert.AreEqual(3, mv.Armies.FirstOrDefault().UnitEntries.FirstOrDefault().Units.FirstOrDefault().DefaultEquipment.Count);
        }
    }
}
