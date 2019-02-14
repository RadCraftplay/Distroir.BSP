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
using System.IO;

namespace Distroir.Bsp
{
    public class BspWriter
    {
        /// <summary>
        /// Writes BSP File header
        /// </summary>
        /// <param name="w">Binary Writer to write to</param>
        /// <param name="i">Informations about BSP info</param>
        public static void WriteHeader(BinaryWriter w, BspInfo i)
        {
            //Set offset
            w.BaseStream.Position = 0;

            //Write header informations
            w.Write(i.Identifier);
            w.Write(i.Version);
            //Write lump informations
            foreach (Lump l in i.Lumps)
                WriteLump(w, l);
            //Write map revision number
            w.Write(i.MapRevision);
        }

        /// <summary>
        /// Writes lump informations
        /// </summary>
        /// <param name="w">Binary writer to write to</param>
        /// <param name="l">Lump informations</param>
        static void WriteLump(BinaryWriter w, Lump l)
        {
            //Write lump data
            w.Write(l.FileOffset);
            w.Write(l.FileLength);
            w.Write(l.Version);
            w.Write(l.fourCC);
        }

        /// <summary>
        /// Writes lump informations
        /// </summary>
        /// <param name="writer">Binary writer to write to</param>
        /// <param name="info">Lump informations</param>
        /// <param name="lumpId">Id of lump</param>
        public static void WriteLump(BinaryWriter writer, Lump info, int lumpId)
        {
            //Set offset
            writer.BaseStream.Position = BspOffsets.CalculateLumpOffset(lumpId);
            //Write lump data
            WriteLump(writer, info);
        }

        /// <summary>
        /// Writes lump informations
        /// </summary>
        /// <param name="writer">Binary writer to write to</param>
        /// <param name="info">Lump informations</param>
        /// <param name="lumpId">Id of lump</param>
        public static void WriteLump(BinaryWriter writer, Lump info, BspLumps lumpId)
        {
            //Write lump informations
            WriteLump(writer, info, (int)lumpId);
        }

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
            Lump[] lumps = new Lump[64];

            //Old lump data
            Lump old = new Lump();

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
    }
}
