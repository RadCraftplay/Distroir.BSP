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
using System;
using System.IO;

namespace Distroir.Bsp
{
    public class BspInfoWriter : IDisposable
    {
        BinaryWriter writer;
        private bool disposed = false;

        public BspInfoWriter(BinaryWriter writer)
        {
            this.writer = writer;
        }

        public BspInfoWriter(Stream outputStream)
        {
            writer = new BinaryWriter(outputStream);
        }

        public BspInfoWriter(string filename)
        {
            writer = new BinaryWriter(new FileStream(filename, FileMode.Open));
        }

        public void Dispose()
        {
            writer.Dispose();
            disposed = true;
        }

        /// <summary>
        /// Writes BspInfo (file header) to file
        /// </summary>
        public void WriteInfo(BspInfo i)
        {
            ThrowExceptionIfDisposed();

            writer.BaseStream.Position = 0;

            writer.Write(i.Identifier);
            writer.Write(i.Version);

            foreach (BspLumpInfo l in i.Lumps)
                WriteBspLumpInfo(l);

            writer.Write(i.MapRevision);
        }

        private void WriteBspLumpInfo(BspLumpInfo info)
        {
            ThrowExceptionIfDisposed();

            writer.Write(info.FileOffset);
            writer.Write(info.FileLength);
            writer.Write(info.Version);
            writer.Write(info.fourCC);
        }

        /// <summary>
        /// Writes lump info
        /// </summary>
        /// <param name="lumpId">Id of lump to override</param>
        public void WriteBspLumpInfo(BspLumpInfo info, int lumpId)
        {
            ThrowExceptionIfDisposed();

            //Set offset
            writer.BaseStream.Position = BspOffsets.CalculateLumpOffset(lumpId);
            //Write lump data
            WriteBspLumpInfo(info);
        }

        /// <summary>
        /// Writes lump info
        /// </summary>
        /// <param name="lumpId">Id of lump to override</param>
        public void WriteBspLumpInfo(BspLumpInfo info, BspLumpType lumpId)
        {
            //Write lump informations
            WriteBspLumpInfo(info, (int)lumpId);
        }

        private void ThrowExceptionIfDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(BspInfoWriter));
        }
    }
}
