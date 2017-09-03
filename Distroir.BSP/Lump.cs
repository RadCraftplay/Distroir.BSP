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
    public class Lump
    {
        /// <summary>
        /// Offset into file (bytes)
        /// </summary>
        public int FileOffset;
        /// <summary>
        /// Length of lump (bytes)
        /// </summary>
        public int FileLength;
        /// <summary>
        /// Lump format version
        /// </summary>
        public int Version;
        /// <summary>
        /// Lump ident code
        /// </summary>
        public int fourCC;
    }
}
