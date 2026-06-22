using System;
using System.Runtime.InteropServices;
using hashTest.NavFileHelper;
using RecastSharp.DetourNative;

namespace SpellRecast
{
    public class NavMesh : IDisposable
    {
        public IntPtr Pointer;

        public DtwStatus Status;

        private DtNavMeshParams _dtNavMeshParams;

        public unsafe NavMesh(NavMeshSetHeader* header)
        {
            _dtNavMeshParams = new DtNavMeshParams()
                {
                    OrigX = header->dtparams.orig[0],
                    OrigY = header->dtparams.orig[1],
                    OrigZ = header->dtparams.orig[2],
                    TileWidth = header->dtparams.tileWidth,
                    TileHeight = header->dtparams.tileHeight,
                    MaxTiles = header->dtparams.maxTiles,
                    MaxPolys = header->dtparams.maxPolys
            };
            Pointer = DetourNative.dtwAllocNavMesh();
            Status = DetourNative.dtwNavMesh_Init(Pointer, ref _dtNavMeshParams);
        }

        public DtwStatus AddTile(byte[] data, int dataSize, int lastRef)
        {
            unsafe
            {
                // Tile data must come from Detour's allocator: the DT_TILE_FREE_DATA (0x01) flag
                // hands ownership to the navmesh, which releases it with dtFree on destruction.
                var dataPtr = DetourNative.dtwAlloc(dataSize);
                Marshal.Copy(data, 0, dataPtr, data.Length);
                return DetourNative.dtwNavMesh_AddTile(Pointer, (byte*) dataPtr.ToPointer(), dataSize, 0x01, lastRef, IntPtr.Zero);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Pointer != IntPtr.Zero)
            {
                DetourNative.dtwFreeNavMesh(Pointer);
                Pointer = IntPtr.Zero;
            }
        }

        ~NavMesh()
        {
            Dispose(false);
        }
    }
}
