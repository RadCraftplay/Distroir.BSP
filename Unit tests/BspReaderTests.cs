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
        public void CtorReadFromBinaryReader()
        {
            using (var stream = new FileStream(MAP_FILENAME, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    BspReader r = new BspReader(reader);
                }
            }
        }

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
        [ExpectedException(typeof(FileFormatException))]
        public void ReadInvalidHeader()
        {
            BspReader r = new BspReader("notabspfile.txt");
            r.ReadInfo();
            r.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadInvalidStream()
        {
            BspReader r = new BspReader(new MemoryStream());
            r.ReadInfo();
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
            var lump = r.ReadLumpInfo(BspLumpType.LUMP_PAKFILE);
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
            var data = r.ReadLumpData(BspLumpType.LUMP_PAKFILE);
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

        [TestMethod]
        public void ReadInCustomOrder()
        {
            BspReader reader = new BspReader(MAP_FILENAME);
            var reference = reader.ReadInfo();
            reader.Dispose();

            reader = new BspReader(MAP_FILENAME);
            var meaninglessVar = reader.ReadLumpInfo(BspLumpType.LUMP_PAKFILE);
            var tested = reader.ReadInfo();

            Assert.AreEqual(reference, tested);
        }
    }
}
