using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class BspReaderTests
    {
        const string MAP_FILENAME = "testmap.bsp";

        [TestMethod]
        public void CtorReadFile()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            r.Dispose();
        }

        [TestMethod]
        public void CtorReadFromFileStream()
        {
            BspReader r = new BspReader(new FileStream(MAP_FILENAME, FileMode.Open));
            r.Dispose();
        }

        [TestMethod]
        public void CtorReadFromStream()
        {
            BspReader r = new BspReader((Stream)(new FileStream(MAP_FILENAME, FileMode.Open)));
            r.Dispose();
        }

        [TestMethod]
        public void ReadInfo()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var info = r.ReadInfo();
            r.Dispose();

            Assert.IsInstanceOfType(info, typeof(BspInfo));
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void ReadLumpInfo()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var lump = r.ReadLumpInfo(BspLumps.LUMP_PAKFILE);
            r.Dispose();

            Assert.IsInstanceOfType(lump, typeof(BspLump));
            Assert.IsNotNull(lump);
        }

        [TestMethod]
        public void ReadLumpInfoById()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var lump = r.ReadLumpInfo(40);
            r.Dispose();

            Assert.IsInstanceOfType(lump, typeof(BspLump));
            Assert.IsNotNull(lump);
        }

        [TestMethod]
        public void ReadLumpData()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var data = r.ReadLumpData(BspLumps.LUMP_PAKFILE);
            r.Dispose();

            Assert.IsInstanceOfType(data, typeof(byte[]));
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void ReadLumpDataById()
        {
            BspReader r = new BspReader(MAP_FILENAME);
            var data = r.ReadLumpData(40);
            r.Dispose();

            Assert.IsInstanceOfType(data, typeof(byte[]));
            Assert.IsNotNull(data);
        }
    }
}
