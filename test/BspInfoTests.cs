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
    public class BspInfoTests
    {
        const string MAP_FILENAME = "Test resources/testmap.bsp";

        [TestMethod]
        public void ComparisonTest()
        {
            var r = new BspReader(MAP_FILENAME);
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
            var info = new BspInfo();
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

            var info2 = new BspInfo();
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
            var r = new BspReader(MAP_FILENAME);
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
