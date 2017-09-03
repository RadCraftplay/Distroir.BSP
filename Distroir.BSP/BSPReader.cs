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
    public class BSPReader
    {
        /// <summary>
        /// Reads BSP info from Stream Reader
        /// </summary>
        /// <param name="reader">Stream Reader to read from</param>
        public static BSPInfo ReadInfo(BinaryReader reader)
        {
            BSPInfo info = new BSPInfo();

            //Read identifier
            info.Identifier = reader.ReadInt32();

            //Validate identifier
            //Little-endian "VBSP"   0x50534256
            if (info.Identifier != 0x50534256)
            {
                throw new FileFormatException();
            }

            //Read version
            info.Version = reader.ReadInt32();

            //Read game lumps
            info.Lumps = new Lump[64];

            for (int i = 0; i < 64; i++)
            {
                info.Lumps[i] = ReadLump(reader);
            }

            //Read map revision number
            info.MapRevision = reader.ReadInt32();

            //Return value
            return info;
        }

        /// <summary>
        /// Reads BSP info from Stream Reader
        /// </summary>
        /// <param name="fs">FileStream to read from</param>
        public static BSPInfo ReadInfo(FileStream fs)
        {
            using (BinaryReader r = new BinaryReader(fs))
            {
                return ReadInfo(r);
            }
        }

        /// <summary>
        /// Reads BSP info from file
        /// </summary>
        /// <param name="filename">Name of file to open</param>
        public static BSPInfo ReadInfo(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    return ReadInfo(r);
                }
            }
        }

        /// <summary>
        /// Reads lump from Binary Reader
        /// </summary>
        /// <param name="r">Binary Reader to read from</param>
        /// <returns></returns>
        static Lump ReadLump(BinaryReader r)
        {
            //Create new lump
            Lump l = new Lump();

            //Read  lump data
            l.FileOffset = r.ReadInt32();
            l.FileLength = r.ReadInt32();
            l.Version = r.ReadInt32();
            l.fourCC = r.ReadInt32();

            //Return value
            return l;
        }

        /// <summary>
        /// Reads lump from Binary Reader
        /// </summary>
        /// <param name="reader">Binary Reader to read from</param>
        /// <param name="lumpId">Lump Id</param>
        /// <returns></returns>
        public static Lump ReadLump(BinaryReader reader, BSPLumps lumpId)
        {
            //Calculate and set offset
            reader.BaseStream.Position = BSPOffsets.CalculateLumpOffset(lumpId);
            //Read lump
            return ReadLump(reader);
        }

        /// <summary>
        /// Reads lump from Binary Reader
        /// </summary>
        /// <param name="reader">Binary Reader to read from</param>
        /// <param name="lumpId">Lump Id</param>
        /// <returns></returns>
        public static Lump ReadLump(BinaryReader reader, int lumpId)
        {
            //Calculate and set offset
            reader.BaseStream.Position = BSPOffsets.CalculateLumpOffset(lumpId);
            //Read lump
            return ReadLump(reader);
        }

        /// <summary>
        /// Reads lump data
        /// </summary>
        /// <param name="reader">Binary Reader to read from</param>
        /// <param name="lump">Lump informations</param>
        /// <returns></returns>
        public static byte[] ReadLumpData(BinaryReader reader, Lump lump)
        {
            //Set offset
            reader.BaseStream.Position = lump.FileOffset;
            //Return value
            return reader.ReadBytes(lump.FileLength);
        }
    }
}
