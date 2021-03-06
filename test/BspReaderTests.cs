﻿/*
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
    public class BspReaderTests
    {
        const string MAP_FILENAME = "Test resources/testmap.bsp";
        const string NOT_BSP_FILE_FILENAME = "Test resources/notabspfile.txt";
        const string EMPTY_FILE_FILENAME = "Test resources/emptyfile.bsp";
        private string exceptionMessage;
        private string ExceptionMessage => exceptionMessage;

        [TestInitialize]
        public void Init()
        {
            try
            {
                throw new ObjectDisposedException(nameof(BspReader));
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }
        }

        [TestMethod]
        public void CtorReadFromBinaryReader()
        {
            using (var stream = new FileStream(MAP_FILENAME, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var r = new BspReader(reader);
                }
            }
        }

        [TestMethod]
        public void CtorReadFile()
        {
            var r = new BspReader(MAP_FILENAME);
            r.Dispose();
        }

        [TestMethod]
        public void CtorReadFromFileStream()
        {
            var r = new BspReader(new FileStream(MAP_FILENAME, FileMode.Open));
            r.Dispose();
        }

        [TestMethod]
        public void CtorReadFromStream()
        {
            var r = new BspReader((Stream)(new FileStream(MAP_FILENAME, FileMode.Open)));
            r.Dispose();
        }


        [TestMethod]
        [ExpectedException(typeof(FileFormatException))]
        public void ReadInvalidHeader()
        {
            var r = new BspReader(NOT_BSP_FILE_FILENAME);
            r.ReadInfo();
            r.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(FileFormatException))]
        public void ReadEmptyFile()
        {
            var r = new BspReader(EMPTY_FILE_FILENAME);
            r.ReadInfo();
            r.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(FileFormatException))]
        public void ReadInvalidStream()
        {
            var r = new BspReader(new MemoryStream());
            r.ReadInfo();
            r.Dispose();
        }

        [TestMethod]
        public void ReadInfo()
        {
            var r = new BspReader(MAP_FILENAME);
            var info = r.ReadInfo();
            r.Dispose();

            Assert.IsInstanceOfType(info, typeof(BspInfo));
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void ReadLumpInfo()
        {
            var r = new BspReader(MAP_FILENAME);
            var lump = r.ReadLumpInfo(BspLumpType.LUMP_PAKFILE);
            r.Dispose();

            Assert.IsInstanceOfType(lump, typeof(BspLumpInfo));
            Assert.IsNotNull(lump);
        }

        [TestMethod]
        public void ReadLumpInfoById()
        {
            var r = new BspReader(MAP_FILENAME);
            var lump = r.ReadLumpInfo(40);
            r.Dispose();

            Assert.IsInstanceOfType(lump, typeof(BspLumpInfo));
            Assert.IsNotNull(lump);
        }

        [TestMethod]
        public void ReadLumpData()
        {
            var r = new BspReader(MAP_FILENAME);
            var data = r.ReadLumpData(BspLumpType.LUMP_PAKFILE);
            r.Dispose();

            Assert.IsInstanceOfType(data, typeof(byte[]));
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void ReadLumpDataById()
        {
            var r = new BspReader(MAP_FILENAME);
            var data = r.ReadLumpData(40);
            r.Dispose();

            Assert.IsInstanceOfType(data, typeof(byte[]));
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void ReadInCustomOrder()
        {
            var reader = new BspReader(MAP_FILENAME);
            var reference = reader.ReadInfo();
            reader.Dispose();

            reader = new BspReader(MAP_FILENAME);
            var meaninglessVar = reader.ReadLumpInfo(BspLumpType.LUMP_PAKFILE);
            var tested = reader.ReadInfo();
            reader.Dispose();

            Assert.AreEqual(reference, tested);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ReadInfoDisposed()
        {
            var reader = new BspReader(MAP_FILENAME);
            reader.Dispose();
            reader.ReadInfo();
        }

        [TestMethod]
        public void ReadInfoDisposedMessage()
        {
            var reader = new BspReader(MAP_FILENAME);
            reader.Dispose();
            try
            {
                reader.ReadInfo();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exceptionMessage);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ReadLumpInfoDisposed()
        {
            var reader = new BspReader(MAP_FILENAME);
            reader.Dispose();
            reader.ReadLumpInfo(0);
        }

        [TestMethod]
        public void ReadLumpInfoDisposedMessage()
        {
            var reader = new BspReader(MAP_FILENAME);
            reader.Dispose();
            try
            {
                reader.ReadLumpInfo(0);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exceptionMessage);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ReadLumpDataDisposed()
        {
            var reader = new BspReader(MAP_FILENAME);
            reader.Dispose();
            reader.ReadLumpData(0);
        }

        [TestMethod]
        public void ReadLumpDataDisposedMessage()
        {
            var reader = new BspReader(MAP_FILENAME);
            reader.Dispose();
            try
            {
                reader.ReadLumpData(0);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exceptionMessage);
            }
        }
    }
}
