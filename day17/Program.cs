using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day17
{
    class Program
    {
        static char[,] map;
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input_mini.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();


            ProcessInput(lines);
        }

        private static void ProcessInput(List<string> lines)
        {
            List<Clay> clays = new List<Clay>();

            foreach (var item in lines)
            {
                var coords = item.Split(", ");
                var leftcoord = coords[0].Split('=');
                var leftdirecton = leftcoord[0];
                var leftvalue = leftcoord[1];
                var rightcoord = coords[1].Split('=');
                var rightdirection = rightcoord[0];
                var rightvalue = rightcoord[1];
                var leftrange = leftvalue.Split("..");
                var leftstart = int.Parse(leftrange[0]);
                var leftend = leftrange.Length > 1 ? int.Parse(leftrange[1]) : leftstart;
                var rightrange = rightvalue.Split("..");
                var rightstart = int.Parse(rightrange[0]);
                var rightend = rightrange.Length > 1 ? int.Parse(rightrange[1]) : rightstart;
                
                if (leftcoord.Equals('x'))
                {
                    var clay = new Clay { minX = leftstart, maxX = leftend, minY = rightstart, maxY = rightend };
                    clays.Add(clay);
                }
            }

            var maxX = clays.Max(c => c.maxX);
            var maxY = clays.Max(c => c.maxY);

            Console.WriteLine($"{maxX}x{maxY}");
        }
    }

    public class Clay
    {
        public int minX { get; set; }
        public int maxX { get; set; }
        public int minY { get; set; }
        public int maxY { get; set; }
    }
}
