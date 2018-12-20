using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var line = File.ReadAllLines("input_mini_1.txt").Where(l => !string.IsNullOrWhiteSpace(l)).First();
            //Console.WriteLine(line);

            var paths = CreateMapAndCountDoors(line);

            PrintMap(paths);
        }

        private static void PrintMap(List<Path> paths)
        {
            var offsetX = Math.Abs(paths.Min(p => p.X));
            var offsetY = Math.Abs(paths.Min(p => p.Y));

            foreach (var item in paths)
            {
                Console.SetCursorPosition(item.X + offsetX, item.Y + offsetY);
                Console.Write(item.Display);
            }
        }

        private static List<Path> CreateMapAndCountDoors(string line)
        {
            List<Path> paths = new List<Path>();
            bool done = false;
            int pos = 0;
            int x = 0;
            int y = 0;
            Path lastPath;
            while (!done)
            {
                var c = line[pos];
                switch (c)
                {
                    case '^':   // Start
                        paths.Add(new Path(x, y, 'X'));
                        break;
                    case 'N':
                        paths.Add(new Path(x, --y, '.'));
                        break;
                    case 'E':
                        paths.Add(new Path(++x, y, '.'));
                        break;
                    case 'S':
                        paths.Add(new Path(x, ++y, '.'));
                        break;
                    case 'W':
                        paths.Add(new Path(--x, y, '.'));
                        break;
                    case '(':
                        {
                            lastPath = paths.Last();
                            var childpaths = CreateMapAndCountDoors(line.Substring(pos + 1, line.Length - (pos + 1)));
                            lastPath.Paths.AddRange(childpaths);
                            pos += childpaths.Count;
                        }
                        break;
                    case '|':
                        {
                            lastPath = paths.Last();
                            if (lastPath.Paths.Count > 0)
                            {
                                var childpaths = CreateMapAndCountDoors(line.Substring(pos + 1, line.Length - (pos + 1)));
                                lastPath.Paths.AddRange(childpaths);
                                pos += childpaths.Count;
                            }
                            else
                            {
                                done = true;
                            }
                        }
                        break;
                    case ')':
                    case '$':
                        done = true;
                        break;
                    default:
                        break;
                }
                pos++;
            }
            return paths;
        }
    }

    public class Path
    {
        public Path(int x, int y, char display)
        {
            X = x;
            Y = y;
            Display = display;
            Paths = new List<Path>();
        }
        public int X { get; set; }
        public int Y { get; set; }
        public char Display { get; set; }
        public List<Path> Paths { get; set; }
    }
}
