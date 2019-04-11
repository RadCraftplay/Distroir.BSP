using System;
using System.Collections.Generic;
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
