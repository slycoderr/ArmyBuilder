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
    public class UnitTest1
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
            Assert.AreEqual(1, mv.Armies.FirstOrDefault().Units.Count);
        }
    }
}
