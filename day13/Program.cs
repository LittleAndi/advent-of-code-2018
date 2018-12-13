using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            // map
            char[,] map = new char[lines.Max(l => l.Length), lines.Count];
            List<Cart> carts = new List<Cart>();
            
            int y = 0;
            foreach (string row in lines)
            {
                int x = 0;
                foreach (char col in row)
                {
                    switch (col)
                    {
                        case '>':
                            map[x, y] = '-';
                            carts.Add(new Cart { X = x, Y = y, Direction = col, NextTurn = 'L', Crashed = false });
                            break;
                        case 'v':
                            map[x, y] = '|';
                            carts.Add(new Cart { X = x, Y = y, Direction = col, NextTurn = 'L', Crashed = false });
                            break;
                        case '<':
                            map[x, y] = '-';
                            carts.Add(new Cart { X = x, Y = y, Direction = col, NextTurn = 'L', Crashed = false });
                            break;
                        case '^':
                            map[x, y] = '|';
                            carts.Add(new Cart { X = x, Y = y, Direction = col, NextTurn = 'L', Crashed = false });
                            break;
                        default:
                            map[x, y] = col;
                            break;
                    }
                    x++;
                }
                y++;
            }

            //System.Console.WriteLine(carts.Count);
            //PrintAll(map, carts, true);
            //Console.ReadKey();
            bool done = false;
            int tick = 0;
            while (!done)
            {
                tick++;
                int curY = Console.CursorTop;

                Console.SetCursorPosition(0, 10);
                Console.Write($"{tick, 5}");

                //Console.SetCursorPosition(0, curY > 0 ? curY : 11);

                // Move all carts
                carts = carts.Where(c => !c.Crashed).OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
                foreach (var cart in carts)
                {
                    var result = cart.Move(map, carts.Where(c => !c.Crashed).ToList());
                    if (!result)
                    {
                        foreach (var item in carts.Where(c => c.X == cart.X && c.Y == cart.Y))
                        {
                            item.Crashed = true;
                            //Console.SetCursorPosition(item.X, item.Y);
                            //Console.Write(map[item.X, item.Y]);
                        }
                        //Console.SetCursorPosition(7, 0);
                        //Console.WriteLine($"Collission {cart.X},{cart.Y}");
                        //break;
                    }
                }
                //PrintAll(map, carts.Where(c => !c.Crashed).ToList());
                if (carts.Count(c => !c.Crashed) == 1)
                {
                    Cart survivor = carts.Where(c => !c.Crashed).First();
                    Console.SetCursorPosition(0, 10);
                    Console.WriteLine($"Survivor {survivor.X},{survivor.Y}");
                    done = true;
                    Console.ReadKey();
                }
                //Thread.Sleep(200);
                //Console.ReadKey();
                //Console.SetCursorPosition(0, 5);
                //Console.Write($"{carts.Count,2}");
            }

            // Part 1
            // 138,93 (wrong!)
            // 115,138 (yay!)


            // Part 2
            // 32,129
        }

        private static void PrintAll(char[,] map, List<Cart> carts, bool printMap = false)
        {
            return;
            if (printMap)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(map[x, y]);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var cart in carts)
            {
                Console.SetCursorPosition(cart.X, cart.Y);
                Console.Write(cart.Direction);
            }
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
        }
    }

    public class Cart
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Direction { get; set; }
        public char NextTurn { get; set; }
        public bool Crashed { get; set; }

        public bool Move(char[,] map, List<Cart> carts)
        {
            //Console.SetCursorPosition(X, Y);
            //Console.Write(map[X, Y]);
            char nextPos = 'X';
            switch (Direction)
            {
                case '>':
                    nextPos = map[X + 1, Y];
                    break;
                case 'v':
                    nextPos = map[X, Y + 1];
                    break;
                case '<':
                    nextPos = map[X - 1, Y];
                    break;
                case '^':
                    nextPos = map[X, Y - 1];
                    break;
            }

            switch (nextPos)
            {
                case '-':
                    // Direction unchanged
                    if (Direction.Equals('>')) X++;
                    else X--;
                    break;
                case '|':
                    // Direction unchanged
                    if (Direction.Equals('v')) Y++;
                    else Y--;
                    break;
                case '/':
                    // Turn
                    switch (Direction)
                    {
                        case '>':
                            X++;
                            Direction = '^';
                            break;
                        case 'v':
                            Y++;
                            Direction = '<';
                            break;
                        case '<':
                            X--;
                            Direction = 'v';
                            break;
                        case '^':
                            Y--;
                            Direction = '>';
                            break;
                    }
                    break;
                case '\\':
                    // Turn
                    switch (Direction)
                    {
                        case '>':
                            X++;
                            Direction = 'v';
                            break;
                        case 'v':
                            Y++;
                            Direction = '>';
                            break;
                        case '<':
                            X--;
                            Direction = '^';
                            break;
                        case '^':
                            Y--;
                            Direction = '<';
                            break;
                    }
                    break;
                case '+':
                    // Turn?
                    switch (Direction)
                    {
                        case '>':
                            X++;
                            break;
                        case 'v':
                            Y++;
                            break;
                        case '<':
                            X--;
                            break;
                        case '^':
                            Y--;
                            break;
                    }
                    switch (NextTurn)
                    {
                        case 'L':
                            switch (Direction)
                            {
                                case '>':
                                    Direction = '^';
                                    break;
                                case 'v':
                                    Direction = '>';
                                    break;
                                case '<':
                                    Direction = 'v';
                                    break;
                                case '^':
                                    Direction = '<';
                                    break;
                            }
                            NextTurn = 'C';
                            break;
                        case 'R':
                            switch (Direction)
                            {
                                case '>':
                                    Direction = 'v';
                                    break;
                                case 'v':
                                    Direction = '<';
                                    break;
                                case '<':
                                    Direction = '^';
                                    break;
                                case '^':
                                    Direction = '>';
                                    break;
                            }
                            NextTurn = 'L';
                            break;
                        case 'C':
                            NextTurn = 'R';
                            break;
                    }
                    break;
            }

            if (carts.Count(c => c.X == X && c.Y == Y) > 1)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{X},{Y} {Direction}";
        }
    }
}
