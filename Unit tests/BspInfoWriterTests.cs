using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class BspInfoWriterTests
    {
        const string MAP_FILENAME = "testmap.bsp";
        const string TEMP_FILENAME = "map.bsp.temp";

        private BspInfo exampleInfo;
        private BspLumpInfo exampleLump;
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
                Lumps = new BspLumpInfo[64]
            };

            for (int i = 0; i < 64; i++)
            {
                exampleInfo.Lumps[i] = new BspLumpInfo()
                {
                    FileLength = i,
                    FileOffset = i,
                    Version = i,
                    fourCC = i
                };
            }

            exampleLump = new BspLumpInfo()
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
            BspInfoWriter w = new BspInfoWriter(new MemoryStream());
            w.Dispose();
        }

        [TestMethod]
        public void CtorWriteToFile()
        {
            BspInfoWriter w = new BspInfoWriter(TEMP_FILENAME);
            w.Dispose();
        }

        [TestMethod]
        public void WriteBspInfo()
        {
            BspInfoWriter writer = new BspInfoWriter(TEMP_FILENAME);

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
            BspInfoWriter writer = new BspInfoWriter(TEMP_FILENAME);
            writer.WriteBspLumpInfo(exampleLump, 0);
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
        [ExpectedException(typeof(System.ObjectDisposedException))]
        public void WriteInfoDisposed()
        {
            var infoWriter = new BspInfoWriter(TEMP_FILENAME);
            infoWriter.Dispose();

            infoWriter.WriteInfo(new BspInfo());
        }

        [TestMethod]
        [ExpectedException(typeof(System.ObjectDisposedException))]
        public void WriteBspLumpInfoDisposed()
        {
            var infoWriter = new BspInfoWriter(TEMP_FILENAME);
            infoWriter.Dispose();

            infoWriter.WriteBspLumpInfo(new BspLumpInfo(), 0);
        }
    }
}
