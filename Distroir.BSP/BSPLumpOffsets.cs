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
namespace Distroir.Bsp
{
    public enum BSPLumps : int
    {
        /// <summary>
        /// Map entities
        /// </summary>
        LUMP_ENTITIES = 0,
        /// <summary>
        /// Plane array
        /// </summary>
        LUMP_PLANES = 1,
        /// <summary>
        /// Index to texture names
        /// </summary>
        LUMP_TEXDATA = 2,
        /// <summary>
        /// Vertex array
        /// </summary>
        LUMP_VERTEXES = 3,
        /// <summary>
        /// Compressed visibility bit arrays
        /// </summary>
        LUMP_VISIBILITY = 4,
        /// <summary>
        /// BSP tree nodes
        /// </summary>
        LUMP_NODES = 5,
        /// <summary>
        /// Face texture array
        /// </summary>
        LUMP_TEXINFO = 6,
        /// <summary>
        /// Face array
        /// </summary>
        LUMP_FACES = 7,
        /// <summary>
        /// Lightmap samples
        /// </summary>
        LUMP_LIGHTING = 8,
        /// <summary>
        /// Occlusion polygons and vertices
        /// </summary>
        LUMP_OCCLUSION = 9,
        /// <summary>
        /// BSP tree leaf nodes
        /// </summary>
        LUMP_LEAFS = 10,
        /// <summary>
        /// Correlates between dfaces and Hammer face IDs. Also used as random seed for detail prop placement.
        /// </summary>
        LUMP_FACEIDS = 11,
        /// <summary>
        /// Edge array
        /// </summary>
        LUMP_EDGES = 12,
        /// <summary>
        /// Index of edges
        /// </summary>
        LUMP_SURFEDGES = 13,
        /// <summary>
        /// Brush models (geometry of brush entities)
        /// </summary>
        LUMP_MODELS = 14,
        /// <summary>
        /// Internal world lights converted from the entity lump
        /// </summary>
        LUMP_WORLDLIGHTS = 15,
        /// <summary>
        /// Index to faces in each leaf
        /// </summary>
        LUMP_LEAFFACES = 16,
        /// <summary>
        /// Index to brushes in each leaf
        /// </summary>
        LUMP_LEAFBRUSHES = 17,
        /// <summary>
        /// Brush array
        /// </summary>
        LUMP_BRUSHES = 18,
        /// <summary>
        /// Brushside array
        /// </summary>
        LUMP_BRUSHSIDES = 19,
        /// <summary>
        /// Area array
        /// </summary>
        LUMP_AREAS = 20,
        /// <summary>
        /// Portals between areas
        /// </summary>
        LUMP_AREAPORTALS = 21,
        /// <summary>
        /// Static props convex hull lists
        /// </summary>
        LUMP_PROPCOLLISION = 22,
        /// <summary>
        /// Static prop convex hulls
        /// </summary>
        LUMP_PROPHULLS = 23,
        /// <summary>
        /// Static prop collision vertices
        /// </summary>
        LUMP_PROPHULLVERTS = 24,
        /// <summary>
        /// Static prop per hull triangle index start/count
        /// </summary>
        LUMP_PROPTRIS = 25,
        /// <summary>
        /// Displacement surface array
        /// </summary>
        LUMP_DISPINFO = 26,
        /// <summary>
        /// Brush faces array before splitting
        /// </summary>
        LUMP_ORIGINALFACES = 27,
        /// <summary>
        /// Displacement physics collision data
        /// </summary>
        LUMP_PHYSDISP = 28,
        /// <summary>
        /// Physics collision data
        /// </summary>
        LUMP_PHYSCOLLIDE = 29,
        /// <summary>
        /// Face plane normals
        /// </summary>
        LUMP_VERTNORMALS = 30,
        /// <summary>
        /// Face plane normal index array
        /// </summary>
        LUMP_VERTNORMALINDICES = 31,
        /// <summary>
        /// Displacement lightmap alphas (unused/empty since Source 2006)
        /// </summary>
        LUMP_DISP_LIGHTMAP_ALPHAS = 32,
        /// <summary>
        /// Vertices of displacement surface meshes
        /// </summary>
        LUMP_DISP_VERTS = 33,
        /// <summary>
        /// Displacement lightmap sample positions
        /// </summary>
        LUMP_DISP_LIGHTMAP_SAMPLE_POSITIONS = 34,
        /// <summary>
        /// Game-specific data lump
        /// </summary>
        LUMP_GAME_LUMP = 35,
        /// <summary>
        /// Data for leaf nodes that are inside water
        /// </summary>
        LUMP_LEAFWATERDATA = 36,
        /// <summary>
        /// Water polygon data
        /// </summary>
        LUMP_PRIMITIVES = 37,
        /// <summary>
        /// Water polygon vertices
        /// </summary>
        LUMP_PRIMVERTS = 38,
        /// <summary>
        /// Water polygon vertex index array
        /// </summary>
        LUMP_PRIMINDICES = 39,
        /// <summary>
        /// Embedded uncompressed Zip-format file
        /// </summary>
        LUMP_PAKFILE = 40,
        /// <summary>
        /// Clipped portal polygon vertices
        /// </summary>
        LUMP_CLIPPORTALVERTS = 41,
        /// <summary>
        /// env_cubemap location array
        /// </summary>
        LUMP_CUBEMAPS = 42,
        /// <summary>
        /// Texture name data
        /// </summary>
        LUMP_TEXDATA_STRING_DATA = 43,
        /// <summary>
        /// Index array into texdata string data
        /// </summary>
        LUMP_TEXDATA_STRING_TABLE = 44,
        /// <summary>
        /// info_overlay data array
        /// </summary>
        LUMP_OVERLAYS = 45,
        /// <summary>
        /// Distance from leaves to water
        /// </summary>
        LUMP_LEAFMINDISTTOWATER = 46,
        /// <summary>
        /// Macro texture info for faces
        /// </summary>
        LUMP_FACE_MACRO_TEXTURE_INFO = 47,
        /// <summary>
        /// Displacement surface triangles
        /// </summary>
        LUMP_DISP_TRIS = 48,
        /// <summary>
        /// Static prop triangle and string data
        /// </summary>
        LUMP_PROP_BLOB = 49,
        /// <summary>
        /// info_overlay's on water faces?
        /// </summary>
        LUMP_WATEROVERLAYS = 50,
        /// <summary>
        /// Index of LUMP_LEAF_AMBIENT_LIGHTING_HDR
        /// </summary>
        LUMP_LEAF_AMBIENT_INDEX_HDR = 51,
        /// <summary>
        /// Index of LUMP_LEAF_AMBIENT_LIGHTING
        /// </summary>
        LUMP_LEAF_AMBIENT_INDEX = 52,
        /// <summary>
        /// HDR lightmap samples
        /// </summary>
        LUMP_LIGHTING_HDR = 53,
        /// <summary>
        /// Internal HDR world lights converted from the entity lump
        /// </summary>
        LUMP_WORLDLIGHTS_HDR = 54,
        /// <summary>
        /// Per-leaf ambient light samples (HDR)
        /// </summary>
        LUMP_LEAF_AMBIENT_LIGHTING_HDR = 55,
        /// <summary>
        /// Per-leaf ambient light samples (LDR)
        /// </summary>
        LUMP_LEAF_AMBIENT_LIGHTING = 56,
        /// <summary>
        /// XZip version of pak file for Xbox. Deprecated.
        /// </summary>
        LUMP_XZIPPAKFILE = 57,
        /// <summary>
        /// HDR maps may have different face data
        /// </summary>
        LUMP_FACES_HDR = 58,
        /// <summary>
        /// Extended level-wide flags. Not present in all levels.
        /// </summary>
        LUMP_MAP_FLAGS = 59,
        /// <summary>
        /// Fade distances for overlays
        /// </summary>
        LUMP_OVERLAY_FADES = 60,
        /// <summary>
        /// System level settings (min/max CPU & GPU to render this overlay)
        /// </summary>
        LUMP_OVERLAY_SYSTEM_LEVELS = 61,
        /// <summary>
        /// ???
        /// </summary>
        LUMP_PHYSLEVEL = 62,
        /// <summary>
        /// Displacement multiblend info
        /// </summary>
        LUMP_DISP_MULTIBLEND = 63
    }
}