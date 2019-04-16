﻿using Distroir.Bsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class BspInfoWriterTests
    {
        const string MAP_FILENAME = "Test resources/testmap.bsp";
        const string TEMP_FILENAME = "Test resources/map.bsp.temp";

        private BspInfo exampleInfo;
        private BspLumpInfo exampleLump;
        private byte[] exampleData = new byte[4] { 16, 32, 64, 128 };
        private string exceptionMessage;
        private string ExceptionMessage => exceptionMessage;

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

            try
            {
                throw new ObjectDisposedException(nameof(BspInfoWriter));
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
        public void CtorOverrideStream()
        {
            var w = new BspInfoWriter(new MemoryStream());
            w.Dispose();
        }

        [TestMethod]
        public void CtorWriteToFile()
        {
            var w = new BspInfoWriter(TEMP_FILENAME);
            w.Dispose();
        }

        [TestMethod]
        public void WriteBspInfo()
        {
            var writer = new BspInfoWriter(TEMP_FILENAME);

            writer.WriteInfo(exampleInfo);
            writer.Dispose();
        }

        [TestMethod]
        public void WriteAndValidateBspInfo()
        {
            WriteBspInfo();

            var reader = new BspReader(TEMP_FILENAME);
            var tempInfo = reader.ReadInfo();
            reader.Dispose();

            Assert.AreEqual(tempInfo, exampleInfo);
        }

        [TestMethod]
        public void WriteLumpInfo()
        {
            var writer = new BspInfoWriter(TEMP_FILENAME);
            writer.WriteBspLumpInfo(0, exampleLump);
            writer.Dispose();
        }

        [TestMethod]
        public void WriteAndValidateLumpInfo()
        {
            WriteLumpInfo();

            var reader = new BspReader(TEMP_FILENAME);
            var tempLumpInfo = reader.ReadLumpInfo(0);
            reader.Dispose();

            Assert.AreEqual(tempLumpInfo, exampleLump);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void WriteInfoDisposed()
        {
            var infoWriter = new BspInfoWriter(TEMP_FILENAME);
            infoWriter.Dispose();

            infoWriter.WriteInfo(new BspInfo());
        }

        [TestMethod]
        public void WriteInfoDisposedMessage()
        {
            var infoWriter = new BspInfoWriter(TEMP_FILENAME);
            infoWriter.Dispose();

            try
            {
                infoWriter.WriteInfo(new BspInfo());
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exceptionMessage);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void WriteBspLumpInfoDisposed()
        {
            var infoWriter = new BspInfoWriter(TEMP_FILENAME);
            infoWriter.Dispose();

            infoWriter.WriteBspLumpInfo(0, new BspLumpInfo());
        }

        [TestMethod]
        public void WriteBspLumpInfoDisposedMessage()
        {
            var infoWriter = new BspInfoWriter(TEMP_FILENAME);
            infoWriter.Dispose();

            try
            {
                infoWriter.WriteBspLumpInfo(0, new BspLumpInfo());
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exceptionMessage);
            }
        }
    }
}
