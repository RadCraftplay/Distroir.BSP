/*
MIT License

Copyright (c) 2017-2019 Distroir

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var one = new BspLumpInfo();
            BspLumpInfo two = null;

            Assert.AreNotEqual(one, two);
        }
    }
}
