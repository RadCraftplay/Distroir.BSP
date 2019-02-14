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
namespace Distroir.BSP
{
    public class BSPOffsets
    {
        /// <summary>
        /// Calculates offset of lump
        /// </summary>
        /// <param name="lumpId">Id of lump (from 0 to 63)</param>
        /// <returns></returns>
        public static int CalculateLumpOffset(int lumpId)
        {
            return 8 + 16 * lumpId;
        }

        /// <summary>
        /// Calculates offset of lump
        /// </summary>
        /// <param name="lump">BSP Lump</param>
        /// <returns></returns>
        public static int CalculateLumpOffset(BSPLumps lump)
        {
            return CalculateLumpOffset((int)lump);
        }
    }
}
