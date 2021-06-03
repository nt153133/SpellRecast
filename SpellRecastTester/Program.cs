using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using RecastSharp.DetourNative;
using SpellRecast;
using SpellRecast.NavFileHelper;

namespace SpellRecastTester
{
    internal class Program
    {
        public static void Main(string[] args1)
        {

            NavMesh navMesh; // = DetourCommon.dtAllocNavMesh();
            var path = "d2t1.nav";


            if (!NavFileReader.LoadFromFileNav(path, out navMesh))
            {
                Console.WriteLine($"Error reading {path}");
            }

            NavMeshQuery navMeshQuery = new NavMeshQuery(ref navMesh);
            
             Vector3 start = new Vector3(80.63416f, 207.3399f, -29.44104f);
             Vector3 end = new Vector3( 94.16319f, 215f, -122.0762f );

             var resultPath = navMeshQuery.FindSmoothPath(start, end, 100, out var path1);
             
             Console.WriteLine($"{resultPath} - Path Length: {path1.Length}");

             for (var index = 0; index < path1.Length; index++)
             {
                 var vector3 = path1[index];
                 if (index + 1 < path1.Length)
                 {
                     //Console.WriteLine($"{Vector3.Distance(vector3, path1[index + 1])}");
                     
                 }

                 Console.WriteLine($"new Vector3({vector3.X}f, {vector3.Y}f, {vector3.Z}f),");
                 //Console.WriteLine(vector3);
             }
             //var resultStart = navMeshQuery.FindNearestPoly(start, out var spos);

             //Console.WriteLine($"{resultStart} - {spos}");

             //resultStart = navMeshQuery.FindNearestPoly(end, out var epos);

             // Console.WriteLine($"{resultStart} - {epos}");
             //var resultPath = navMeshQuery.FindPath(start, end, 100, out var navPath);
             /*
             Console.WriteLine($"{resultPath} - Path Length: {navPath.Length}");
 
 
             float[] startPos = new[] { 80.63416f, 207.3399f, -29.44104f };
             float[] closest = new float[3];
             bool overPoly = false;
 
             DtwStatus result;
             //result = DetourNative.dtwNavMeshQuery_closestPointOnPoly(navMeshQuery.Pointer, navPath[0], ref startPos[0], ref closest[0], ref overPoly);
             //Console.WriteLine($"{result} {closest[0]}, {closest[1]}, {closest[2]} {overPoly}");
             Console.WriteLine($"new Vector3({start.X}f, {start.Y}f, {start.Z}f),");
             foreach (var polyRef in navPath)
             {
                 start = navMeshQuery.FindNearestPoint(start, polyRef);
                 Console.WriteLine($"new Vector3({start.X}f, {start.Y}f, {start.Z}f),");
             }
             Console.WriteLine($"new Vector3({end.X}f, {end.Y}f, {end.Z}f)");
             */


             // var nextPoint = navMeshQuery.FindNearestPointBoundary(start, navPath[0]);
             // Console.WriteLine(nextPoint);
             /*IntPtr naviMeshQuery = DetourNative.dtwAllocNavMeshQuery();
             DetourNative.dtwNavMeshQuery_Init(naviMeshQuery, navMesh.Pointer, 2048);
 
 
             // Vector3 start = new Vector3(80.63416f, 207.3399f, -29.44104f);
             // Vector3 end = new Vector3(36.01461f, 208.2336f, -51.62287f);
 
             //-78.33846f, 205.784f, -98.63499f
             //94.16319f, 215f, -122.0762f
 
             float[] startArray = new float[] { 80.63416f, 207.3399f, -29.44104f };
             float[] endArray = new float[] { 94.16319f, 215f, -122.0762f };
             int maxPath = 32;
             int[] navPath = new int[maxPath];
 
             var filter = DetourNative.dtwAllocQueryFilter();
 
             var result = DetourNative.dtwNavMeshQuery_FindNearestPoly(naviMeshQuery, ref startArray[0], ref polyFindRange, filter, ref spos, ref nearestArray[0]);
             Console.WriteLine($"Start Location: {result} - poly#: {spos} Nearest point ({nearestArray[0]} {nearestArray[1]} {nearestArray[2]})");
             result = DetourNative.dtwNavMeshQuery_FindNearestPoly(naviMeshQuery, ref endArray[0], ref polyFindRange, filter, ref epos, ref nearestArray[0]);
             Console.WriteLine($"Ending Location: {result} - poly#: {epos} Nearest point ({nearestArray[0]} {nearestArray[1]} {nearestArray[2]})");
 
             var pathResult = DetourNative.dtwNavMeshQuery_FindPath(naviMeshQuery, spos, epos, ref startArray[0], ref endArray[0], filter, ref navPath[0], out int pathCount, maxPath);
             Console.WriteLine($"Path Generation: {pathResult}");
             Console.WriteLine($"Num of polygons in path: {pathCount}");*/
        }
    }
}