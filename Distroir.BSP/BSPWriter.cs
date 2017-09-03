/*
Distroir.BSP
Copyright (C) 2017 Distroir

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System.IO;

namespace Distroir.BSP
{
    public class BSPWriter
    {
        /// <summary>
        /// Writes BSP File header
        /// </summary>
        /// <param name="w">Binary Writer to write to</param>
        /// <param name="i">Informations about BSP info</param>
        public static void WriteHeader(BinaryWriter w, BSPInfo i)
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
            writer.BaseStream.Position = BSPOffsets.CalculateLumpOffset(lumpId);
            //Write lump data
            WriteLump(writer, info);
        }

        /// <summary>
        /// Writes lump informations
        /// </summary>
        /// <param name="writer">Binary writer to write to</param>
        /// <param name="info">Lump informations</param>
        /// <param name="lumpId">Id of lump</param>
        public static void WriteLump(BinaryWriter writer, Lump info, BSPLumps lumpId)
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
            BSPInfo info;
            //Collection of all lumps
            Lump[] lumps = new Lump[64];

            //Old lump data
            Lump old = new Lump();

            //Read lump data
            using (BinaryReader r = new BinaryReader(input))
            {
                //Read BSP informations
                info = BSPReader.ReadInfo(r);
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
