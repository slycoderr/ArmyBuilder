using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Debug.Assert(list.Name == "my test");
            Debug.Assert(list.PointsLimit == 1000);
            Debug.Assert(list.System == GameSystem.AoS);
            Debug.Assert(((AgeOfSigmarArmyList)list).Allegiance == Allegiance.Order);
        }
    }
}
