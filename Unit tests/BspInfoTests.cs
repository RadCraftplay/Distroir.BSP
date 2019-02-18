using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class BspInfoTests
    {
        const string MAP_FILENAME = "testmap.bsp";

        [TestMethod]
        public void ComparisonTest()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var info1 = r.ReadInfo();
            r.Dispose();

            r = new BspReader(MAP_FILENAME);
            var info2 = r.ReadInfo();
            r.Dispose();

            Assert.AreEqual(info1, info2);
        }
    }
}
