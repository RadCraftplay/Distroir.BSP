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
            var newBspInfo = gatheredInfo;
            var lumpToUpdate = newBspInfo.Lumps[lumpId];
            var sizeDifference = data.Length - lumpToUpdate.FileLength;

            newBspInfo.Lumps = UpdateLumpSizes(newBspInfo.Lumps, lumpToUpdate, sizeDifference);

            infoWriter.WriteInfo(newBspInfo);
            WriteDataToFile(newBspInfo.Lumps, gatheredInfo.Lumps, lumpId, data);
        }

        private BspLump[] UpdateLumpSizes(BspLump[] lumps, BspLump lumpToUpdate, int sizeDifference)
        {
            var updatedLumps = new BspLump[64];

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

        private void WriteDataToFile(BspLump[] oldLumps, BspLump[] newLumps, int modifiedLumpId, byte[] modifiedLumpData)
        {
            for (int i = 0; i < 64; i++)
            {
                BspLump oldLump = oldLumps[i];
                BspLump newLump = newLumps[i];
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
