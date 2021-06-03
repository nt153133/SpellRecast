using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SpellRecast;
using SpellRecast.NavFileHelper;

namespace SpellRecastTester
{
    internal class Program
    {
        public static void Main(string[] args1)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
                String resourceName = "AssemblyLoadingAndReflection." +
                                      new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            NavMesh navMesh; // = DetourCommon.dtAllocNavMesh();
            var path = "d2t1.nav";


            if (!NavFileReader.LoadFromFileNav(path, out navMesh))
            {
                Console.WriteLine($"Error reading {path}");
            }

            float polyFindRange = 3;
            float[] nearestArray = new float[3];
            int nearestRef = 0;
            int spos = 0, epos = 0;

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