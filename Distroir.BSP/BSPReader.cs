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
    public class BspReader : IDisposable
    {
        private BinaryReader reader;
        private BspInfo cachedInfo;

        public BspReader(Stream input)
        {
            reader = new BinaryReader(input);
        }

        public BspReader(string filename)
        {
            reader = new BinaryReader(new FileStream(filename, FileMode.Open));
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        /// <summary>
        /// Reads BSP info from file
        /// </summary>
        public BspInfo ReadInfo()
        {
            if (cachedInfo == null)
                cachedInfo = RefreshInfo();

            return cachedInfo;
        }

        private BspInfo RefreshInfo()
        {
            BspInfo info = new BspInfo();

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
            info.Lumps = new BspLump[64];

            for (int i = 0; i < 64; i++)
            {
                info.Lumps[i] = ReadLump();
            }

            //Read map revision number
            info.MapRevision = reader.ReadInt32();

            //Return value
            return info;
        }
        /// <summary>
        /// Reads lump from Binary Reader
        /// </summary>
        /// <param name="r">Binary Reader to read from</param>
        /// <returns></returns>
        private BspLump ReadLump()
        {
            //Create new lump
            BspLump lump = new BspLump();

            //Read  lump data
            lump.FileOffset = reader.ReadInt32();
            lump.FileLength = reader.ReadInt32();
            lump.Version = reader.ReadInt32();
            lump.fourCC = reader.ReadInt32();

            //Return value
            return lump;
        }

        /// <summary>
        /// Reads lump from Binary Reader
        /// </summary>
        /// <param name="reader">Binary Reader to read from</param>
        /// <param name="lumpId">Lump Id</param>
        /// <returns></returns>
        public static BspLump ReadLump(BinaryReader reader, BspLumps lumpId)
        {
            //Calculate and set offset
            reader.BaseStream.Position = BspOffsets.CalculateLumpOffset(lumpId);
            //Read lump
            return ReadLump(reader);
        }

        /// <summary>
        /// Reads lump from Binary Reader
        /// </summary>
        /// <param name="reader">Binary Reader to read from</param>
        /// <param name="lumpId">Lump Id</param>
        /// <returns></returns>
        public static BspLump ReadLump(BinaryReader reader, int lumpId)
        {
            //Calculate and set offset
            reader.BaseStream.Position = BspOffsets.CalculateLumpOffset(lumpId);
            //Read lump
            return ReadLump(reader);
        }

        /// <summary>
        /// Reads lump data
        /// </summary>
        /// <param name="reader">Binary Reader to read from</param>
        /// <param name="lump">Lump informations</param>
        /// <returns></returns>
        public static byte[] ReadLumpData(BinaryReader reader, BspLump lump)
        {
            //Set offset
            reader.BaseStream.Position = lump.FileOffset;
            //Return value
            return reader.ReadBytes(lump.FileLength);
        }
    }
}
