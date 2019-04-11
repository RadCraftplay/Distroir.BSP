using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class BspLumpInfoTests
    {
        [TestMethod]
        public void ExactComparison()
        {
            var lump1 = new BspLumpInfo()
            {
                Version = 1,
                FileLength = 2155,
                FileOffset = 309
            };

            var lump2 = new BspLumpInfo()
            {
                Version = 1,
                FileLength = 2155,
                FileOffset = 309
            };

            Assert.AreEqual(lump1, lump2);
        }

        [TestMethod]
        public void DifferentComparison()
        {
            var lump1 = new BspLumpInfo()
            {
                Version = 1,
                FileLength = 2155,
                FileOffset = 309
            };

            var lump2 = new BspLumpInfo()
            {
                Version = 2,
                FileLength = 2155,
                FileOffset = 309
            };

            Assert.AreNotEqual(lump1, lump2);
        }

        [TestMethod]
        public void BothNullComparison()
        {
            BspLumpInfo one = null;
            BspLumpInfo two = null;

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void OneNullComparison()
        {
            BspLumpInfo one = new BspLumpInfo();
            BspLumpInfo two = null;

            Assert.AreNotEqual(one, two);
        }
    }
}
