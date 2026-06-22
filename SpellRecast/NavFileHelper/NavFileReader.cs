using System;
using System.IO;
using System.Runtime.InteropServices;
using hashTest.NavFileHelper;

namespace SpellRecast.NavFileHelper
{
    public static class NavFileReader
    {
        private const int NAVMESHSET_MAGIC = 'M' << 24 | 'S' << 16 | 'E' << 8 | 'T'; //'MSET'
        private const int NAVMESHSET_VERSION = 1;

        public static bool LoadFromFileNav(string fileName, out NavMesh navMesh)
        {
            unsafe
            {
                navMesh = null;

                if (!File.Exists(fileName))
                {
                    return false;
                }

                //Read the first 64 bytes of the file first to make sure it's a nav file
                using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(fileName)))
                {
                    //Magic number doesn't match
                    if (binaryReader.ReadInt32() != NAVMESHSET_MAGIC)
                    {
                        return false;
                    }

                    //File type version is wrong
                    if (binaryReader.ReadInt32() != NAVMESHSET_VERSION)
                    {
                        return false;
                    }
                }

                NavMeshSetHeader* header;
                using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(fileName)))
                {
                    //Grab the bytes for the header so the navMesh can be init
                    fixed (byte* bp = binaryReader.ReadBytes(sizeof(NavMeshSetHeader)))
                    {
                        header = (NavMeshSetHeader*) bp;
                    }

                    navMesh = new NavMesh(header);

                    //Init the navmesh with the info from the file header including # of tiles and origin vector

                    NavMeshTileHeader* tileHeader;

                    //Recreate all the tiles
                    for (var i = 0; i < header->numTiles; ++i)
                    {
                        //Read the tile header to know the size of the tile data and ref #
                        fixed (byte* bp = binaryReader.ReadBytes(sizeof(NavMeshTileHeader)))
                        {
                            tileHeader = (NavMeshTileHeader*) bp;
                        }

                        navMesh.AddTile(binaryReader.ReadBytes(tileHeader->dataSize), tileHeader->dataSize, (int) tileHeader->tileRef);
                    }
                }

                return true;
            }
        }

        internal static IntPtr dtAllocDefault(int size)
        {
            return Marshal.AllocHGlobal(size);
        }
    }
}
