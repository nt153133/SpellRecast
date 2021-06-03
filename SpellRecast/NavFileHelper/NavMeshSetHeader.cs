using RecastSharp;

namespace hashTest.NavFileHelper
{
    public struct NavMeshSetHeader
    {
        public int magic;
        public int version;
        public int numTiles;
        public dtNavMeshParams dtparams;
    }
    
    public unsafe struct dtNavMeshParams
    {
        public fixed float orig[3]; ///< The world space origin of the navigation mesh's tile space. [(x, y, z)]
        public float tileWidth; ///< The width of each tile. (Along the x-axis.)
        public float tileHeight; ///< The height of each tile. (Along the z-axis.)
        public int maxTiles; ///< The maximum number of tiles the navigation mesh can contain. This and maxPolys are used to calculate how many bits are needed to identify tiles and polygons uniquely.
        public int maxPolys; ///< The maximum number of polygons each tile can contain. This and maxTiles are used to calculate how many bits are needed to identify tiles and polygons uniquely.
    }
}