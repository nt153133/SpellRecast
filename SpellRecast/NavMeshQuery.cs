using System;
using System.Numerics;
using RecastSharp.DetourNative;

namespace SpellRecast
{
    public class NavMeshQuery
    {
        public IntPtr Pointer;
        private IntPtr filter;

        private float[] tempArray = new float[3];
        private float polyFindRange = 3;
        private float[] nearestArray = new float[3];
        private float[] closest = new float[3];
        private bool tempBool = false;
        int maxPath = 32;

        private float[] StartPos = new float[3];
        private float[] EndPos = new float[3];

        public NavMeshQuery(ref NavMesh navMesh)
        {
            Pointer = DetourNative.dtwAllocNavMeshQuery();
            DetourNative.dtwNavMeshQuery_Init(Pointer, navMesh.Pointer, 2048);
            filter = DetourNative.dtwAllocQueryFilter();
        }

        public DtwStatus FindNearestPoly(Vector3 pos, out int nearestPoly)
        {
            nearestPoly = 0;
            tempArray[0] = pos.X;
            tempArray[1] = pos.Y;
            tempArray[2] = pos.Z;
            DtwStatus result = DetourNative.dtwNavMeshQuery_FindNearestPoly(Pointer, ref tempArray[0], ref polyFindRange, filter, ref nearestPoly, ref nearestArray[0]);
            return result;
        }

        public Vector3 FindNearestPointBoundary(Vector3 pos, int polyRef)
        {
            tempArray[0] = pos.X;
            tempArray[1] = pos.Y;
            tempArray[2] = pos.Z;
            var result =  DetourNative.dtwNavMeshQuery_closestPointOnPolyBoundary(Pointer, polyRef, ref tempArray[0], ref closest[0]);
            return result == DtwStatus.Success ? new Vector3(closest[0], closest[1], closest[2]) : Vector3.Zero;
        }
        
        public Vector3 FindNearestPoint(Vector3 pos, int polyRef)
        {
            tempArray[0] = pos.X;
            tempArray[1] = pos.Y;
            tempArray[2] = pos.Z;
            var result =  DetourNative.dtwNavMeshQuery_closestPointOnPoly(Pointer, polyRef, ref tempArray[0], ref closest[0], ref tempBool);
            return result == DtwStatus.Success ? new Vector3(closest[0], closest[1], closest[2]) : Vector3.Zero;
        }

        //var pathResult = DetourNative.dtwNavMeshQuery_FindPath(naviMeshQuery, spos, epos, ref startArray[0], ref endArray[0], filter, ref navPath[0], out int pathCount, maxPath);

        public DtwStatus FindPath(Vector3 start, Vector3 end, int maxPath, out int[] path)
        {
            int[] tempPath = new int[maxPath];
            int spos = -1;
            int epos = -1;
            path = null;
            
            if (FindNearestPoly(start, out spos) != DtwStatus.Success)
            {
                return DtwStatus.Failure;
            }

            //Console.WriteLine($"{tempArray[0]} {tempArray[1]} {tempArray[2]} ");
            if (FindNearestPoly(end, out epos) != DtwStatus.Success)
            {
                return DtwStatus.Failure;
            }

            SetStart(start);
            SetEnd(end);
            //Console.WriteLine($"{tempArray[0]} {tempArray[1]} {tempArray[2]} ");
            
            var pathResult = DetourNative.dtwNavMeshQuery_FindPath(this.Pointer, spos, epos, ref StartPos[0], ref EndPos[0], filter, ref tempPath[0], out int pathCount, maxPath);

            if (pathResult == DtwStatus.Success)
            {
                path = new int[pathCount];
                for (var i = 0; i < pathCount; i++)
                {
                    path[i] = tempPath[i];
                }
            }

            return pathResult;
        }

        
        public DtwStatus FindSmoothPath(Vector3 start, Vector3 end, int maxPath, out Vector3[] pathout)
        {
            var result = FindPath(start, end, 100, out var path);

            float[] smoothpath = new float[maxPath * 3];
            int smoothPathLength = 0;
            
            if (result != DtwStatus.Success)
            {
                pathout = null;
                return DtwStatus.Failure;
            }

            SetStart(start);
            SetEnd(end);

            result = DetourNative.findSmoothPath(this.Pointer, this.filter, ref StartPos[0], ref EndPos[0], ref path[0], path.Length, ref smoothpath[0], ref smoothPathLength, maxPath);

            Console.WriteLine($"{result} {smoothPathLength}");
            pathout = new Vector3[smoothPathLength];
            int pos = 0;
            for (int i = 0; i < smoothPathLength; i++)
            {
                pathout[i] = new Vector3(smoothpath[pos], smoothpath[pos + 1], smoothpath[pos +2]);
                pos += 3;
            }
            return result;
        }

        private void SetStart(Vector3 pos)
        {
            StartPos[0] = pos.X;
            StartPos[1] = pos.Y;
            StartPos[2] = pos.Z;
        }
        
        private void SetEnd(Vector3 pos)
        {
            EndPos[0] = pos.X;
            EndPos[1] = pos.Y;
            EndPos[2] = pos.Z;
        }
    }
}