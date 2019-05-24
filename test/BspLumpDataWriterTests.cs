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
using System;
using System.IO;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullFilenames()
        {
            var writer = new BspLumpDataWriter((string)null, (string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorFirstFilenameNull()
        {
            var writer = new BspLumpDataWriter(null, OUTPUT_FILENAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorSecondFilenameNull()
        {
            var writer = new BspLumpDataWriter(TEMP_FILENAME, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullStreams()
        {
            var writer = new BspLumpDataWriter((Stream)null, (Stream)null);
        }

        [TestMethod]
        public void CtorFirstStreamNull()
        {
            var outputStream = new FileStream(OUTPUT_FILENAME, FileMode.CreateNew);

            var ex = ExceptionUtils.GetThrownException(() =>
            {
                var writer = new BspLumpDataWriter(null, outputStream);
            });

            outputStream.Close();
            Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void CtorSecondStreamNull()
        {
            if (File.Exists(TEMP_FILENAME))
                File.Delete(TEMP_FILENAME);

            var inputStream = new FileStream(TEMP_FILENAME, FileMode.CreateNew);

            var ex = ExceptionUtils.GetThrownException(() =>
            {
                var writer = new BspLumpDataWriter(inputStream,
                    (Stream)null);
            });

            inputStream.Close();
            Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
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
