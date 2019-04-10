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
using System.Collections.Generic;
using System.Linq;

namespace Distroir.Bsp
{
    public class BspInfo
    {
        /// <summary>
        /// BSP file identifier
        /// </summary>
        public int Identifier { get; set; }
        /// <summary>
        /// BSP file version
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// lump directory array
        /// </summary>
        public BspLump[] Lumps { get; set; }
        /// <summary>
        /// Map's revision number
        /// </summary>
        public int MapRevision { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BspInfo info &&
                   Identifier == info.Identifier &&
                   Version == info.Version &&
                   Enumerable.SequenceEqual(Lumps, info.Lumps) &&
                   MapRevision == info.MapRevision;
        }

        public override int GetHashCode()
        {
            var hashCode = 2049534845;
            hashCode = hashCode * -1521134295 + Identifier.GetHashCode();
            hashCode = hashCode * -1521134295 + Version.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<BspLump[]>.Default.GetHashCode(Lumps);
            hashCode = hashCode * -1521134295 + MapRevision.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(BspInfo left, BspInfo right)
        {
            if (object.ReferenceEquals(null, left))
                return object.ReferenceEquals(null, right);
            else
                return left.Equals(right);
        }

        public static bool operator !=(BspInfo left, BspInfo right)
        {
            return !(left == right);
        }

        public BspInfo Clone()
        {
            var clonedInfo = new BspInfo
            {
                Identifier = Identifier,
                Version = Version,
                Lumps = new BspLump[64],
                MapRevision = MapRevision
            };

            for (int i = 0; i < 64; i++)
            {
                var oldLump = Lumps[i];
                clonedInfo.Lumps[i] = new BspLump()
                {
                    FileLength = oldLump.FileLength,
                    FileOffset = oldLump.FileOffset,
                    fourCC = oldLump.fourCC,
                    Version = oldLump.Version
                };
            }

            return clonedInfo;
        }
    }
}
