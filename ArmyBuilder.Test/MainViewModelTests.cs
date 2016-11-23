﻿using System;
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
            string path = "C:\\Users\\slyco\\Source\\Repos\\armybuilder\\ARmyData\\Sylvaneth.xml";
            int checkArmyId = 10;
            int numUnits = 11;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                mv.LoadArmyData(stream);
            }

            var army = mv.Armies.First(a => a.Id == checkArmyId); //make sure the army is there

            Assert.AreEqual(numUnits, army.Units.Count);
            Assert.AreEqual(120, army.Units.First().BaseCost);
            Assert.AreEqual(10, army.Units.First().Minimum);
            Assert.AreEqual(30, army.Units.First().Maximum);
            Assert.AreEqual(120, army.Units.First().IncrementCost);

        }
    }
}