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

namespace Distroir.Bsp
{
    public class BspLumpInfo
    {
        /// <summary>
        /// Offset into file (bytes)
        /// </summary>
        public int FileOffset { get; set; }
        /// <summary>
        /// Length of lump (bytes)
        /// </summary>
        public int FileLength { get; set; }
        /// <summary>
        /// Lump format version
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Lump ident code
        /// </summary>
        public int fourCC { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BspLumpInfo lump &&
                   FileOffset == lump.FileOffset &&
                   FileLength == lump.FileLength &&
                   Version == lump.Version &&
                   fourCC == lump.fourCC;
        }

        public override int GetHashCode()
        {
            var hashCode = -345481539;
            hashCode = hashCode * -1521134295 + FileOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + FileLength.GetHashCode();
            hashCode = hashCode * -1521134295 + Version.GetHashCode();
            hashCode = hashCode * -1521134295 + fourCC.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(BspLumpInfo left, BspLumpInfo right)
        {
            if (object.ReferenceEquals(null, left))
                return object.ReferenceEquals(null, right);
            else
                return left.Equals(right);
        }

        public static bool operator !=(BspLumpInfo left, BspLumpInfo right)
        {
            return !(left == right);
        }
    }
}
