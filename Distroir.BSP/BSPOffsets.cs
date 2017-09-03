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
