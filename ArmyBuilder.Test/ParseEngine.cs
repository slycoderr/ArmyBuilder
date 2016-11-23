using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArmyBuilder.Test
{
    [TestClass]
    public class ParseEngine
    {
        [TestMethod]
        public void ParseListMetaData()
        {
            MainViewModel mv = new MainViewModel();

            List<string> listdata = new List<string>()
            {
                "system=aos",
                "points=1000",
                "Allegiance=Order",
                "name=my test",
            };

            var list = mv.ParserEngineViewModel.GenerateListFromText(listdata);

            Assert.AreEqual("my test", list.Name);
            Assert.AreEqual(1000, list.PointsLimit);
            Assert.AreEqual(GameSystem.AoS, list.System);
            Assert.AreEqual(Allegiance.Order, ((AgeOfSigmarArmyList)list).Allegiance);
        }

        [TestMethod]
        public void ParseListUnits()
        {
            MainViewModel mv = new MainViewModel();
            string path = "C:\\Users\\slyco\\Source\\Repos\\armybuilder\\ARmyData\\Sylvaneth.xml";

            using (var stream = new FileStream(path, FileMode.Open))
            {
                mv.LoadArmyData(stream);
            }

            List<string> listdata = new List<string>()
            {
                "system=aos",
                "points=2000",
                "Allegiance=Sylvaneth",
                "name=my test",
                "20^dryads"
            };

            mv.SelectedArmyList = mv.ParserEngineViewModel.GenerateListFromText(listdata);



            Assert.AreEqual((uint)(2*120), mv.SelectedArmyList.CurrentPointsTotal);

        }
    }
}
