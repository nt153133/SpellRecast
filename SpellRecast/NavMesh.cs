using System;
using System.Runtime.InteropServices;
using hashTest.NavFileHelper;
using RecastSharp.DetourNative;

namespace SpellRecast
{
    public class NavMesh
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
                IntPtr result = IntPtr.Zero;
                var dataPtr = dtAllocDefault(dataSize);
                Marshal.Copy(data, 0, dataPtr, data.Length);
                var otherResult = DetourNative.dtwNavMesh_AddTile(Pointer, (byte*) dataPtr.ToPointer(), dataSize, 0x01, lastRef, result);
                return otherResult;
            }
        }
        
        internal static IntPtr dtAllocDefault(int size)
        {
            return Marshal.AllocHGlobal(size);
        }

        internal static void dtFree(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}