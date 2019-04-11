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
        const string MAP_FILENAME = "Test resources/testmap.bsp";

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

        [TestMethod]
        public void DifferentComparison()
        {
            BspInfo info = new BspInfo();
            info.Version = 1;
            info.Identifier = 1;
            info.Lumps = new BspLumpInfo[1]
            {
                new BspLumpInfo()
                {
                    Version = 1,
                    FileLength = 1,
                    FileOffset = 1,
                    fourCC = 1
                }
            };
            info.MapRevision = 1;

            BspInfo info2 = new BspInfo();
            info2.Version = 1;
            info2.Identifier = 1;
            info2.Lumps = new BspLumpInfo[1]
            {
                new BspLumpInfo()
                {
                    Version = 1,
                    FileLength = 1,
                    FileOffset = 1,
                    fourCC = 1
                }
            };
            info2.MapRevision = 1;

            Assert.AreEqual(info, info2);

            info2.MapRevision = 37;
            Assert.AreNotEqual(info, info2);
        }

        [TestMethod]
        public void OneNullComparison()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var info1 = r.ReadInfo();
            r.Dispose();

            Assert.AreNotEqual(info1, null);
        }

        [TestMethod]
        public void BothNullComparison()
        {
            BspInfo info1 = null;

            Assert.AreEqual(info1, null);
        }
    }
}
