using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class BspWriterTests
    {
        const string MAP_FILENAME = "testmap.bsp";
        const string TEMP_FILENAME = "map.bsp.temp";

        private BspInfo exampleInfo;
        private BspLump exampleLump;
        private byte[] exampleData = new byte[4] { 16, 32, 64, 128 };

        [TestInitialize]
        public void Init()
        {
            //Prepare files for tests
            if (File.Exists(TEMP_FILENAME))
                File.Delete(TEMP_FILENAME);

            File.Copy(MAP_FILENAME, TEMP_FILENAME);

            //Prepare variables
            exampleInfo = new BspInfo()
            {
                Identifier = 0x50534256,
                Version = 0,
                MapRevision = 0,
                Lumps = new BspLump[64]
            };

            for (int i = 0; i < 64; i++)
            {
                exampleInfo.Lumps[i] = new BspLump()
                {
                    FileLength = i,
                    FileOffset = i,
                    Version = i,
                    fourCC = i
                };
            }

            exampleLump = new BspLump()
            {
                FileLength = 1,
                FileOffset = 2,
                fourCC = 3,
                Version = 4
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(TEMP_FILENAME);
        }

        [TestMethod]
        public void CtorOverrideStream()
        {
            BspWriter w = new BspWriter(new MemoryStream());
            w.Dispose();
        }

        [TestMethod]
        public void CtorWriteToFile()
        {
            BspWriter w = new BspWriter(TEMP_FILENAME);
            w.Dispose();
        }

        [TestMethod]
        public void WriteBspInfo()
        {
            BspWriter writer = new BspWriter(TEMP_FILENAME);

            writer.WriteInfo(exampleInfo);
            writer.Dispose();
        }

        [TestMethod]
        public void WriteAndValidateBspInfo()
        {
            WriteBspInfo();

            BspReader reader = new BspReader(TEMP_FILENAME);
            var tempInfo = reader.ReadInfo();
            reader.Dispose();

            Assert.AreEqual(tempInfo, exampleInfo);
        }

        [TestMethod]
        public void WriteLumpInfo()
        {
            BspWriter writer = new BspWriter(TEMP_FILENAME);
            writer.WriteLump(0, exampleLump);
            writer.Dispose();
        }

        [TestMethod]
        public void WriteAndValidateLumpInfo()
        {
            WriteLumpInfo();

            BspReader reader = new BspReader(TEMP_FILENAME);
            var tempLumpInfo = reader.ReadLumpInfo(0);
            reader.Dispose();

            Assert.AreEqual(tempLumpInfo, exampleLump);
        }

        [TestMethod]
        public void WriteLumpData()
        {
            BspWriter writer = new BspWriter(TEMP_FILENAME);
            writer.WriteLumpData(1, exampleData);
            writer.Dispose();
        }

        [TestMethod]
        public void WriteAndTestLumpData()
        {
            WriteLumpData();

            BspReader reader = new BspReader(TEMP_FILENAME);
            var tempLumpData = reader.ReadLumpData(1);
            reader.Dispose();

            Assert.AreEqual(tempLumpData, exampleData);
        }
    }
}
