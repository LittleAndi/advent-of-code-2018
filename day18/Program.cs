using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day18
{
    class Program
    {
        static char[,] map;
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input_mini.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            Map map = new Map(ProcessInput(lines));

            map.PrintMap();
            for (int minute = 0; minute < 10; minute++)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write($"{minute+1}");
                map.ModifyMap();
                map.PrintMap();
            }
        }


        private static char[,] ProcessInput(List<string> lines)
        {
            map = new char[lines[0].Length, lines.Count];
            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                foreach (var col in line)
                {
                    map[x, y] = col;
                    x++;
                }
                y++;
            }

            return map;
        }
    }

    public class Map
    {
        char[,] map;

        public Map(char[,] initmap)
        {
            map = initmap;
        }

        public bool CountTrees(int posx, int posy)
        {
            int trees = 0;
            for (int x = posx - 1; x <= posx + 1; x++)
            {
                if (x < 0 || x >= map.GetLength(0)) continue;
                for (int y = posy - 1; y <= posy + 1; y++)
                {
                    if (y < 0 || y >= map.GetLength(1) || (y == posy && y == posy)) continue;
                    if (map[x, y] == '|') trees++;
                }
            }
            return (trees >= 3);
        }

        public bool CountLumberyards(int posx, int posy)
        {
            int lumberyards = 0;
            for (int x = posx - 1; x <= posx + 1; x++)
            {
                if (x < 0 || x >= map.GetLength(0)) continue;
                for (int y = posy - 1; y <= posy + 1; y++)
                {
                    if (y < 0 || y >= map.GetLength(1) || (x == posx && y == posy)) continue;
                    if (map[x, y] == '#') lumberyards++;
                }
            }
            return (lumberyards >= 3);
        }

        public bool CountTreesAndLumberyards(int posx, int posy)
        {
            int trees = 0;
            int lumberyards = 0;
            for (int x = posx - 1; x <= posx + 1; x++)
            {
                if (x < 0 || x >= map.GetLength(0)) continue;
                for (int y = posy - 1; y <= posy + 1; y++)
                {
                    if (y < 0 || y >= map.GetLength(1) || (y == posy && y == posy)) continue;
                    if (map[x, y] == '|') trees++;
                    if (map[x, y] == '#') lumberyards++;
                }
            }
            return (trees >= 1 && lumberyards >= 1);
        }

        public void ModifyMap()
        {
            var newMap = new char[map.GetLength(0), map.GetLength(1)];

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    switch (map[x, y])
                    {
                        case '.':   // open
                            if (CountTrees(x, y))
                            {
                                newMap[x, y] = '|';
                            }
                            else
                            {
                                newMap[x, y] = map[x, y];
                            }
                            break;
                        case '|':   // tree
                            if (CountLumberyards(x, y))
                            {
                                newMap[x, y] = '#';
                            }
                            else
                            {
                                newMap[x, y] = map[x, y];
                            }
                            break;
                        case '#':   // lumberyard
                            if (CountTreesAndLumberyards(x, y))
                            {
                                newMap[x, y] = '#';
                            }
                            else
                            {
                                newMap[x, y] = '.';
                            }
                            break;
                    }
                    //PrintMap(newMap);
                }
            }

            map = newMap;
        }

        public void PrintMap()
        {
            Console.SetCursorPosition(0, 2);
            for (int y = 0; y < map.GetLength(1); y++)
            {
                StringBuilder line = new StringBuilder();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    line.Append(map[x, y]);
                }
                Console.WriteLine(line.ToString());
            }
            Console.ReadKey();
        }

        private void PrintMap(char[,] newMap)
        {
            for (int y = 0; y < newMap.GetLength(1); y++)
            {
                Console.SetCursorPosition(15, y + 2);
                StringBuilder line = new StringBuilder();
                for (int x = 0; x < newMap.GetLength(0); x++)
                {
                    line.Append(newMap[x, y]);
                }
                Console.WriteLine(line.ToString());
            }
            Console.ReadKey();
        }

    }
}
