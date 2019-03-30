using Distroir.Bsp;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class BspLumpDataWriterTests
    {
        const string MAP_FILENAME = "testmap.bsp";
        const string TEMP_FILENAME = "map.bsp.temp";
        const string OUTPUT_FILENAME = "map_modified.bsp.temp";
        private readonly byte[] exampleData = { 16, 32, 64, 128 };

        [TestInitialize]
        public void Init()
        {
            //Prepare files for tests
            if (File.Exists(TEMP_FILENAME))
                File.Delete(TEMP_FILENAME);

            if (File.Exists(OUTPUT_FILENAME))
                File.Delete(OUTPUT_FILENAME);

            File.Copy(MAP_FILENAME, TEMP_FILENAME);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(TEMP_FILENAME);
        }

        [TestMethod]
        public void CtorWriteToFile()
        {
            var writer = new BspLumpDataWriter(TEMP_FILENAME, OUTPUT_FILENAME);
            writer.Dispose();
        }
        
        [TestMethod]
        public void CtorWriteToStream()
        {
            var writer = new BspLumpDataWriter(new FileStream(TEMP_FILENAME, FileMode.Open),
                new FileStream(OUTPUT_FILENAME, FileMode.CreateNew));
            writer.Dispose();
        }

        [TestMethod]
        public void WriteData()
        {
            using (var writer = new BspLumpDataWriter(TEMP_FILENAME, OUTPUT_FILENAME))
            {
                writer.WriteLumpData(40, exampleData);
            }

            using (var reader = new BspReader(OUTPUT_FILENAME))
            {
                Assert.AreEqual(exampleData.Length, reader.ReadLumpData(40).Length);
            }
        }
    }
}
