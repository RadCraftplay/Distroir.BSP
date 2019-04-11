using Distroir.Bsp;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class BspLumpDataWriterTests
    {
        const string MAP_FILENAME = "Test resources/testmap.bsp";
        const string TEMP_FILENAME = "Test resources/map.bsp.temp";
        const string OUTPUT_FILENAME = "Test resources/map_modified.bsp.temp";
        private readonly byte[] exampleData = { 16, 32, 64, 128 };
        private string exceptionMessage;
        private string ExceptionMessage => exceptionMessage;

        [TestInitialize]
        public void Init()
        {
            //Prepare files for tests
            if (File.Exists(TEMP_FILENAME))
                File.Delete(TEMP_FILENAME);

            if (File.Exists(OUTPUT_FILENAME))
                File.Delete(OUTPUT_FILENAME);

            File.Copy(MAP_FILENAME, TEMP_FILENAME);

            try
            {
                throw new ObjectDisposedException(nameof(BspLumpDataWriter));
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }
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
            var lumpToEdit = 39;

            using (var writer = new BspLumpDataWriter(TEMP_FILENAME, OUTPUT_FILENAME))
            {
                writer.WriteLumpData(lumpToEdit, exampleData);
            }

            using (var reader = new BspReader(OUTPUT_FILENAME))
            {
                Assert.AreEqual(exampleData.Length, reader.ReadLumpData(lumpToEdit).Length);
            }
        }

        [TestMethod]
        public void WriteAndValidateOtherData()
        {
            var lumpToEdit = 39;
            byte[] oldData;

            using (var reader = new BspReader(TEMP_FILENAME))
            {
                oldData = reader.ReadLumpData(40);
            }

            using (var writer = new BspLumpDataWriter(TEMP_FILENAME, OUTPUT_FILENAME))
            {
                writer.WriteLumpData(lumpToEdit, exampleData);
            }

            using (var reader = new BspReader(OUTPUT_FILENAME))
            {
                var newData = reader.ReadLumpData(40);

                for (int i = 0; i < newData.Length; i++)
                {
                    byte oldByte = oldData[i];
                    byte newByte = newData[i];

                    Assert.AreEqual(oldByte, newByte);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void WriteDataDisposed()
        {
            var writer = new BspLumpDataWriter(TEMP_FILENAME, OUTPUT_FILENAME);
            writer.Dispose();
            writer.WriteLumpData(0, new byte[1] { 1 });
        }

        [TestMethod]
        public void WriteDataDisposedMessage()
        {
            var writer = new BspLumpDataWriter(TEMP_FILENAME, OUTPUT_FILENAME);
            writer.Dispose();
            try
            {
                writer.WriteLumpData(0, new byte[1] { 1 });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exceptionMessage);
            }
        }
    }
}
