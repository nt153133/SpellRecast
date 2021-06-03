using System;
using System.IO;
using System.Runtime.InteropServices;
using hashTest.NavFileHelper;

namespace SpellRecast.NavFileHelper
{
    public static class NavFileReader
    {
        private const int MAX_POLYS = 32;
        private const int MAX_SMOOTH = 2048;

        private const int NAVMESHSET_MAGIC = 'M' << 24 | 'S' << 16 | 'E' << 8 | 'T'; //'MSET'
        private const int NAVMESHSET_VERSION = 1;

        /*
        public static bool LoadFromFile(string fileName, out IntPtr navMesh)
        {
            unsafe
            {
                navMesh = IntPtr.Zero;

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

                //Finally just read the whole file at once
                var file = new Span<byte>(File.ReadAllBytes(fileName));
                NavMeshSetHeader* header;
                var pos = 0;

                //Grab the bytes for the header so the navMesh can be init
                fixed (byte* bp = file.Slice(pos, sizeof(NavMeshSetHeader)))
                {
                    header = (NavMeshSetHeader*) bp;
                    pos += sizeof(NavMeshSetHeader);
                }
                
                

                var navHeader = new DtNavMeshParams()
                {
                    OrigX = header->dtparams.orig[0],
                    OrigY = header->dtparams.orig[1],
                    OrigZ = header->dtparams.orig[2],
                    TileWidth = header->dtparams.tileWidth,
                    TileHeight = header->dtparams.tileHeight,
                    MaxTiles = header->dtparams.maxTiles,
                    MaxPolys = header->dtparams.maxPolys
                };
                //header->dtparams.orig, header->dtparams.tileWidth, header->dtparams.tileHeight, header->dtparams.maxTiles, header->dtparams.maxPolys);

                //Allocate the navMesh, this uses GlobalAlloc
                //Fuck if i know how to free it after, so don't
                navMesh = DetourNative.dtwAllocNavMesh();
                DetourNative.dtwNavMesh_Init(navMesh, ref navHeader);
                //Init the navmesh with the info from the file header including # of tiles and origin vector

                NavMeshTileHeader* tileHeader;
                IntPtr result;

                //Recreate all the tiles
                for (var i = 0; i < header->numTiles; ++i)
                {
                    result = IntPtr.Zero;
                    //Read the tile header to know the size of the tile data and ref #
                    fixed (byte* bp = file.Slice(pos, sizeof(NavMeshTileHeader)))
                    {
                        tileHeader = (NavMeshTileHeader*) bp;
                        //Console.WriteLine($"Tile {i} - Ref: {tileHeader->tileRef} Size: {tileHeader->dataSize}");
                        pos += sizeof(NavMeshTileHeader);
                    }

                    //Use detour allocate to allocate the tile data, copy over the file data to the new space and generate the tile
                    //DetourNative.dtwNavMesh_AddTile()

                    var data = (byte*) dtAllocDefault(tileHeader->dataSize);
                    var dataSpan = new Span<byte>(data, tileHeader->dataSize);
                    file.Slice(pos, tileHeader->dataSize).CopyTo(dataSpan);
                    var otherResult = DetourNative.dtwNavMesh_AddTile(navMesh, data, tileHeader->dataSize, 0x01, (int) tileHeader->tileRef, result);
                    //Console.WriteLine($"Tile {i} - Ref: {tileHeader->tileRef} Size: {tileHeader->dataSize} Result {result.ToString("X")} OtherResult {otherResult}");

                    //Just keep pushing the position along so the byte array can just be sliced
                    //I'm sure we could also be reading the file incrementally instead of all at once
                    pos += tileHeader->dataSize;
                }

                return true;
            }
        }
        */
        
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

                //Finally just read the whole file at once
                //var file = new Span<byte>(File.ReadAllBytes(fileName));
                
                NavMeshSetHeader* header;
                var pos = 0;
                using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(fileName)))
                {
                    //Grab the bytes for the header so the navMesh can be init
                    fixed (byte* bp = binaryReader.ReadBytes(sizeof(NavMeshSetHeader)))
                    {
                        header = (NavMeshSetHeader*) bp;
                        pos += sizeof(NavMeshSetHeader);
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
                            //Console.WriteLine($"Tile {i} - Ref: {tileHeader->tileRef} Size: {tileHeader->dataSize}");
                            pos += sizeof(NavMeshTileHeader);
                        }

                        navMesh.AddTile( binaryReader.ReadBytes(tileHeader->dataSize), tileHeader->dataSize, (int) tileHeader->tileRef);

                        //Just keep pushing the position along so the byte array can just be sliced
                        //I'm sure we could also be reading the file incrementally instead of all at once
                        pos += tileHeader->dataSize;
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