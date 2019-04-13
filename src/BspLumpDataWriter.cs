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
    public class BspLumpDataWriter : IDisposable
    {
        private BinaryWriter dataWriter;
        private BspInfoWriter infoWriter;
        private BspReader reader;
        private BspInfo gatheredInfo;
        private bool disposed = false;

        public BspLumpDataWriter(Stream input, Stream output)
        {
            InitializeWriter(input, output);
        }

        public BspLumpDataWriter(string inputFilename, string outputFilename)
        {
            InitializeWriter(new FileStream(inputFilename, FileMode.Open),
                new FileStream(outputFilename, FileMode.CreateNew));
        }

        public void Dispose()
        {
            dataWriter.Dispose();
            infoWriter.Dispose();
            reader.Dispose();
            gatheredInfo = null;
            disposed = true;
        }

        private void InitializeWriter(Stream input, Stream output)
        {
            dataWriter = new BinaryWriter(output);
            infoWriter = new BspInfoWriter(dataWriter);
            reader = new BspReader(input);
            gatheredInfo = reader.ReadInfo();
        }

        public void WriteLumpData(int lumpId, byte[] data)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(BspLumpDataWriter));

            var newBspInfo = gatheredInfo.Clone();
            var lumpToUpdate = newBspInfo.Lumps[lumpId];
            var sizeDifference = data.Length - lumpToUpdate.FileLength;

            newBspInfo.Lumps = UpdateLumpInfo(newBspInfo.Lumps, lumpToUpdate, sizeDifference);

            infoWriter.WriteInfo(newBspInfo);
            WriteDataToFile(gatheredInfo.Lumps, newBspInfo.Lumps, lumpId, data);
        }

        private BspLumpInfo[] UpdateLumpInfo(BspLumpInfo[] lumps, BspLumpInfo lumpToUpdate, int sizeDifference)
        {
            var updatedLumps = new BspLumpInfo[64];

            for (int i = 0; i < 64; i++)
            {
                var tempLump = lumps[i];

                if (lumps[i].FileOffset == lumpToUpdate.FileOffset)
                    tempLump.FileLength += sizeDifference;

                if (lumps[i].FileOffset > lumpToUpdate.FileOffset)
                    tempLump.FileOffset += sizeDifference;

                updatedLumps[i] = tempLump;
            }

            return updatedLumps;
        }

        private void WriteDataToFile(BspLumpInfo[] oldLumps, BspLumpInfo[] newLumps, int modifiedLumpId, byte[] modifiedLumpData)
        {
            for (int i = 0; i < 64; i++)
            {
                BspLumpInfo oldLump = oldLumps[i];
                BspLumpInfo newLump = newLumps[i];
                byte[] dataToWrite;

                if (i == modifiedLumpId)
                    dataToWrite = modifiedLumpData;
                else
                    dataToWrite = reader.ReadLumpData(i);

                dataWriter.BaseStream.Position = newLump.FileOffset;
                dataWriter.Write(dataToWrite);
            }
        }
    }
}
