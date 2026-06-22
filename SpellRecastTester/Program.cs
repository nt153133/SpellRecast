using System;
using System.Numerics;
using SpellRecast;
using SpellRecast.NavFileHelper;

namespace SpellRecastTester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string path = "d2t1.nav";

            if (!NavFileReader.LoadFromFileNav(path, out NavMesh navMesh))
            {
                Console.WriteLine($"Error reading {path}");
                return;
            }

            using (navMesh)
            using (var navMeshQuery = new NavMeshQuery(navMesh))
            {
                var start = new Vector3(80.63416f, 207.3399f, -29.44104f);
                var end = new Vector3(94.16319f, 215f, -122.0762f);

                var resultPath = navMeshQuery.FindSmoothPath(start, end, 100, out var path1);

                Console.WriteLine($"{resultPath} - Path Length: {path1.Length}");

                foreach (var point in path1)
                {
                    Console.WriteLine($"new Vector3({point.X}f, {point.Y}f, {point.Z}f),");
                }
            }
        }
    }
}
