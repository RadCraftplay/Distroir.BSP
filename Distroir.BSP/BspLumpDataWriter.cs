using System;
using System.IO;
namespace Distroir.Bsp
{
    public class BspLumpDataWriter
    {
        private BinaryWriter dataWriter;
        private BspReader reader;
        private BspInfo gatheredInfo;

        public BspLumpDataWriter(Stream stream)
        {
            InitializeWriter(stream);
        }

        public BspLumpDataWriter(string filename)
        {
            InitializeWriter(new FileStream(filename, FileMode.Open));
        }

        private void InitializeWriter(Stream stream)
        {
            reader = new BspReader(stream);
            gatheredInfo = reader.ReadInfo();
            
            dataWriter = new BinaryWriter(stream);
        }
        
        /*

        /// <summary>
        /// Writes lump data to FileStream
        /// </summary>
        /// <param name="input">Input stream (File)</param>
        /// <param name="data">New lump data</param>
        /// <param name="lumpId">Id of lump to overwrite</param>
        /// <param name="output">Destination stream</param>
        public static void WriteLumpData(Stream input, byte[] data, int lumpId, Stream output)
        {
            //Difference in length of data
            int difference = 0;

            //Save stream length
            long streamLength = input.Length;

            //Data before lump
            byte[] beforeLump;
            //Data after lump
            byte[] afterLump;

            //BSP informations
            BspInfo info;
            //Collection of all lumps
            BspLump[] lumps = new BspLump[64];

            //Old lump data
            BspLump old = new BspLump();

            //Read lump data
            using (BinaryReader r = new BinaryReader(input))
            {
                //Read BSP informations
                info = BspReader.ReadInfo(r);
                lumps = info.Lumps;

                //Get old lump
                old = lumps[lumpId];
                //Calculate difference
                difference = data.Length - old.FileLength;

                //Read data before changed lump
                beforeLump = ReadBytes(r, 1036, old.FileOffset - 1036);

                //Read data after lump
                input.Seek(old.FileOffset + old.FileLength, SeekOrigin.Begin);
                input.Seek(streamLength, SeekOrigin.End);
                afterLump = ReadBytes(r, old.FileOffset + old.FileLength, ((int)streamLength - (old.FileOffset + old.FileLength)));

                //Overwrite lump length
                old.FileLength = data.Length;

                //Save old lump informations
                lumps[lumpId] = old;
            }

            //Update lump informations
            for (int i = 0; i < 64; i++)
            {
                //Check if lump have bigger offset
                if (lumps[i].FileOffset > old.FileOffset)
                {
                    //If yes, change offset of lump
                    lumps[i].FileOffset += difference;
                }
            }

            // Apply lump changes to BSP info
            info.Lumps = lumps;

            //Write file
            using (BinaryWriter w = new BinaryWriter(output))
            {
                //Write file header
                WriteHeader(w, info);
                //Write data before modified lump
                w.Write(beforeLump);
                //Write modified data
                w.Write(data);
                //Write data after modified lump
                w.Write(afterLump);
            }
        }

        /// <summary>
        /// Reads byte array from binary reader
        /// </summary>
        /// <param name="r">Binary reader to read from</param>
        /// <param name="offset">Offset of</param>
        /// <param name="length"></param>
        /// <returns></returns>
        static byte[] ReadBytes(BinaryReader r, int offset, int length)
        {
            r.BaseStream.Position = offset;
            return r.ReadBytes(length);
        }
        */
    }
}
