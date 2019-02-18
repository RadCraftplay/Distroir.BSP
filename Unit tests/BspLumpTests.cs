using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class BspLumpTests
    {
        [TestMethod]
        public void ExactComparison()
        {
            var lump1 = new BspLump()
            {
                Version = 1,
                FileLength = 2155,
                FileOffset = 309
            };

            var lump2 = new BspLump()
            {
                Version = 1,
                FileLength = 2155,
                FileOffset = 309
            };

            Assert.AreEqual(lump1, lump2);
        }

        [TestMethod]
        public void BothNullComaprision()
        {
            BspLump one = null;
            BspLump two = null;

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void OneNullComparision()
        {
            BspLump one = new BspLump();
            BspLump two = null;

            Assert.AreNotEqual(one, two);
        }
    }
}
