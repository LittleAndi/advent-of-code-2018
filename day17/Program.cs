using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day17
{
    class Program
    {
        static bool print = false;
        static int counter = 0;

        static void Main(string[] args)
        {
            if (!print) Console.CursorVisible = false;

            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();


            var input = ProcessInput(lines);

            var map = CreateMap(input);
            var map_untouched = CreateMap(input);

            Console.SetBufferSize(Console.WindowWidth, 3000);

            PrintMap(map);

            //WriteMap(map);

            FillWater(map, map_untouched);

            PrintMap(map);

            int water = CountWater(map);

            WriteMap(map);

            Console.WriteLine($"Total water is {water} sqm");
            Console.ReadKey();

            // 408 (too low!)
        }

        private static int CountWater(Map map)
        {
            int water = 0;
            for (int y = map.MinY; y <= map.MaxY; y++)
            {
                for (int x = 0; x < map.Layout.GetLength(0); x++)
                {
                    if (map.Layout[x, y] == '~' || map.Layout[x, y] == '|') water++;

                }
            }

            return water;
        }

        private static void FillWater(Map map, Map map_untouched)
        {
            int x = 500;
            int y = 0;

            if (map.Layout[x, y + 1] == '.')
            {
                FillDown(map, map_untouched, x, y + 1);
            }
        }


        private static bool FillDown(Map map, Map map_untouched, int x, int y)
        {
            //PrintMap(map);

            bool foundWallToTheLeft = false;
            bool foundWallToTheRight = false;

            if (y + 1 >= map.Layout.GetLength(1))
            {
                map.Layout[x, y] = '|';
                PrintMap(map, x, y);
                return false;
            }
            if (map_untouched.Layout[x, y + 1] == '.' && map.Layout[x, y + 1] != '~')
            {
                map.Layout[x, y] = '|';
                PrintMap(map, x, y);
                var downResult = FillDown(map, map_untouched, x, y + 1);
                if (downResult)
                {
                    // Test left
                    if (map_untouched.Layout[x - 1, y] == '#')
                    {
                        foundWallToTheLeft = true;
                        //PrintMap(map);
                    }
                    else
                    {
                        foundWallToTheLeft = FillLeft(map, map_untouched, x - 1, y);
                    }

                    // Test right
                    if (map_untouched.Layout[x + 1, y] == '#')
                    {
                        foundWallToTheRight = true;
                        //PrintMap(map);
                    }
                    else
                    {
                        foundWallToTheRight = FillRight(map, map_untouched, x + 1, y);
                    }

                    if (foundWallToTheLeft && foundWallToTheRight)
                    {
                        map.Layout[x, y] = '~';
                        PrintMap(map, x, y);
                    }
                }
            }
            else if (map_untouched.Layout[x, y + 1] == '#' || map.Layout[x, y + 1] == '~')
            {
                map.Layout[x, y] = '|';
                PrintMap(map, x, y);

                // Test left
                if (map_untouched.Layout[x - 1, y] == '#')
                {
                    foundWallToTheLeft = true;
                    //PrintMap(map);
                }
                else
                {
                    foundWallToTheLeft = FillLeft(map, map_untouched, x - 1, y);
                }

                // Test right
                if (map_untouched.Layout[x + 1, y] == '#')
                {
                    foundWallToTheRight = true;
                    //PrintMap(map);
                }
                else
                {
                    foundWallToTheRight = FillRight(map, map_untouched, x + 1, y);
                }

                if (foundWallToTheLeft && foundWallToTheRight)
                {
                    map.Layout[x, y] = '~';
                    PrintMap(map, x, y);
                }
            }
            //else if ()
            //{
            //    map.Layout[x, y] = '|';
            //    PrintMap(map, x, y);
            //}

            return (foundWallToTheLeft && foundWallToTheRight);
        }

        private static bool FillRight(Map map, Map map_untouched, int x, int y)
        {
            //PrintMap(map);
            bool foundWall = false;

            // Test down
            if (map_untouched.Layout[x, y + 1] == '.' && map.Layout[x, y + 1] != '~')
            {
                foundWall = FillDown(map, map_untouched, x, y);
            }
            else
            {
                // Test right
                if (map_untouched.Layout[x + 1, y] == '#')
                {
                    foundWall = true;
                    map.Layout[x, y] = '~';
                    PrintMap(map, x, y);
                }
                else
                {
                    map.Layout[x, y] = '|';
                    PrintMap(map, x, y);
                    foundWall = FillRight(map, map_untouched, x + 1, y);
                    if (foundWall) { map.Layout[x, y] = '~'; PrintMap(map, x, y); }
                }
            }
            return foundWall;
        }

        private static bool FillLeft(Map map, Map map_untouched, int x, int y)
        {
            //PrintMap(map);
            bool foundWall = false;

            // Test down
            if (map_untouched.Layout[x, y + 1] == '.' && map.Layout[x, y + 1] != '~')
            {
                foundWall = FillDown(map, map_untouched, x, y);
            }
            else
            {
                // Test left
                if (map_untouched.Layout[x - 1, y] == '#')
                {
                    foundWall = true;
                    map.Layout[x, y] = '~';
                    PrintMap(map, x, y);
                    //PrintMap(map);
                }
                else
                {
                    map.Layout[x, y] = '|';
                    PrintMap(map, x, y);
                    foundWall = FillLeft(map, map_untouched, x - 1, y);
                    if (foundWall) { map.Layout[x, y] = '~'; PrintMap(map, x, y); }
                }
            }

            return foundWall;
        }

        private static Map CreateMap((List<Clay> clays, int minX, int maxX, int minY, int maxY) input)
        {
            var layout = new char[input.maxX + 2, input.maxY + 1];
            for (int x = 0; x < layout.GetLength(0); x++)
            {
                for (int y = 0; y < layout.GetLength(1); y++)
                {
                    layout[x, y] = '.';
                }
            }

            layout[500, 0] = '+';
            foreach (var clay in input.clays)
            {
                for (int y = clay.minY; y <= clay.maxY; y++)
                {
                    for (int x = clay.minX; x <= clay.maxX; x++)
                    {
                        layout[x, y] = '#';
                    }
                }
            }

            Map map = new Map { Layout = layout, MinX = input.minX, MaxX = input.maxX, MinY = input.minY, MaxY = input.maxY };
            return map;
        }

        private static (List<Clay> clays, int minX, int maxX, int minY, int maxY) ProcessInput(List<string> lines)
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

                if (leftdirecton.Equals("x"))
                {
                    var clay = new Clay { minX = leftstart, maxX = leftend, minY = rightstart, maxY = rightend };
                    clays.Add(clay);
                }
                else
                {
                    var clay = new Clay { minX = rightstart, maxX = rightend, minY = leftstart, maxY = leftend };
                    clays.Add(clay);
                }

            }

            var minX = clays.Min(c => c.minX);
            var maxX = clays.Max(c => c.maxX);
            var minY = clays.Min(c => c.minY);
            var maxY = clays.Max(c => c.maxY);

            //Console.WriteLine($"{maxX}x{maxY}");

            return (clays, minX, maxX, minY, maxY);
        }

        private static void PrintMap(Map map, int maxSizeY = 2000)
        {
            if (!print) { return; }

            Console.SetCursorPosition(0, 1);
            for (int y = 0; y < (map.Layout.GetLength(1) > maxSizeY ? maxSizeY : map.Layout.GetLength(1)); y++)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = map.MinX - 1; x < map.Layout.GetLength(0); x++)
                {
                    sb.Append(map.Layout[x, y]);
                }
                Console.WriteLine(sb.ToString());
            }
            Console.ReadKey();
        }

        private static void PrintMap(Map map, int x, int y, int maxSizeY = 60)
        {
            counter++;
            if (!print)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write($"{counter, 10} - x = {x,4}, y = {y,4}");
                return;
            }

            Console.SetCursorPosition(x - (map.MinX - 1), y + 1);
            Console.Write(map.Layout[x, y]);
            //Console.SetCursorPosition(0, 0);
            //Console.Write(counter);

            //Console.SetCursorPosition(x - (map.MinX - 1), y+1);
            //Thread.Sleep(2);
            //if (counter < 100000) Console.ReadKey();
        }

        private static void WriteMap(Map map)
        {
            using (TextWriter tw = new StreamWriter("output.txt", true))
            {
                for (int y = 0; y < 60; y++)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int x = map.MinX - 1; x < map.Layout.GetLength(0); x++)
                    {
                        sb.Append(map.Layout[x, y]);
                    }
                    tw.WriteLine(sb.ToString());
                }

                tw.WriteLine();
            }
        }
    }

    public class Clay
    {
        public int minX { get; set; }
        public int maxX { get; set; }
        public int minY { get; set; }
        public int maxY { get; set; }
    }

    public class Map
    {
        public char[,] Layout { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
    }
}
