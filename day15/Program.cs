﻿using Pastel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day15
{
    class Program
    {
        static char[,] map;
        static int currentMessageRow = 0;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            List<Player> players = ReadMap("input.txt");

            PrintMap();
            PrintPlayers(players);
            PrintStats(players);
            Console.ReadKey();
            var result = Fight((List<Player>)players.Clone());
            Console.WriteLine(result);

            bool done = false;
            int elfAttackPower = 25; // Start at 25...
            while (!done)
            {
                var boostedResult = Fight((List<Player>)players.Clone(), elfAttackPower: elfAttackPower, allowDeadElves: false, pause: false, pauseEveryPlayer: false);

                if (boostedResult.winner.Equals('E'))
                {
                    done = true;
                }
                elfAttackPower++;
            }

            // 183885 (too high!)
            // 184470 (too high!)
            // 183300
        }

        private static (char winner, int result) Fight(List<Player> players, int elfAttackPower = 3, bool allowDeadElves = true, bool pause = true, bool pauseEveryPlayer = true)
        {
            bool done = false;
            int round = 0;

            // Adjust Elf attack power
            players.ForEach((player) =>
            {
                if (player.Type.Equals('E')) player.AttackPower = elfAttackPower;
            });

            while (!done)
            {
                foreach (var player in players.Where(p => p.IsAlive).OrderBy(p => p.Coordinate.Y).ThenBy(p => p.Coordinate.X))
                {
                    // Skip if player died during this round
                    if (player.IsDead) continue;

                    PrintPlayer(player);
                    var enemies = players.Where(p => p.IsAlive && !p.Type.Equals(player.Type));
                    if (enemies.Count() == 0) { done = true; break; }
                    if (enemies.Count(e => player.IsNearby(e)) == 0)
                    {
                        // Move phase
                        Dictionary<Coordinate, List<Coordinate>> inRangeSpots = new Dictionary<Coordinate, List<Coordinate>>();
                        foreach (var enemy in enemies)
                        {
                            foreach (var inRangeSpot in enemy.InRange(map, players))
                            {
                                Console.SetCursorPosition(inRangeSpot.X, inRangeSpot.Y);
                                Console.Write('?');

                                var path = BreadthFirstSearch(players, player.Coordinate, inRangeSpot);

                                if (path != null)
                                {
                                    path.Add(inRangeSpot);
                                    if (!inRangeSpots.ContainsKey(inRangeSpot))
                                    {
                                        inRangeSpots.Add(inRangeSpot, path.Skip(1).ToList());
                                    }
                                    //foreach (var item in path)
                                    //{
                                    //    Console.SetCursorPosition(item.X, item.Y);
                                    //}
                                }
                                //else
                                //{
                                //    PrintMessage($"{player.ToString()} => {enemy.ToString()} - No path to enemy");
                                //}
                            }
                        }

                        // Now we got a list of possible spots to move to, choose one!
                        if (inRangeSpots.Count > 0)
                        {
                            var shortestDistance = inRangeSpots.Min(s => s.Value.Count);
                            var nearestSpots = inRangeSpots.Where(s => s.Value.Count.Equals(shortestDistance)).OrderBy(s => s.Key.Y).ThenBy(s => s.Key.X);
                            var chosenSpot = nearestSpots.First();
                            //Console.SetCursorPosition(chosenSpot.Key.X, chosenSpot.Key.Y);
                            player.MoveTo(chosenSpot.Value.First());
                        }
                        PrintMap();
                        PrintPlayers(players);
                    }

                    // Attack phase
                    var enemyToAttack = enemies.Where(e => player.IsNearby(e)).OrderBy(e => e.HitPoints).ThenBy(e => e.Coordinate.Y).ThenBy(e => e.Coordinate.X).FirstOrDefault();
                    if (enemyToAttack != null)
                    {
                        enemyToAttack.TakeHitFrom(player);
                        if (!allowDeadElves && enemyToAttack.Type.Equals('E') && enemyToAttack.IsDead)
                        {
                            return ('X', 0);
                        }
                    }

                    if (pauseEveryPlayer)
                    {
                        PrintStats(players, round);
                        var keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.Escape) pauseEveryPlayer = false;
                    }

                    //PrintMessage($"Player {player.Type} - enemies: {enemies.Count()}");
                }
                PrintMap();
                PrintPlayers(players);
                PrintStats(players, ++round, elfAttackPower);
                if (pause)
                {
                    var keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Escape) pause = false;
                }
            }

            var winningType = players.Where(p => p.IsAlive).First().Type;
            var hitpoints = players.Where(p => p.IsAlive).Sum(p => p.HitPoints);
            var result = (round - 1) * hitpoints;
            PrintMessage($"Result after {round - 1}*{hitpoints} full rounds: {result}");
            Console.WriteLine();

            return (winningType, result);
        }

        private static void PrintMessage(string message)
        {
            if (currentMessageRow == 0 || currentMessageRow >= Console.WindowHeight) currentMessageRow = map.GetLength(1) + 1;
            Console.SetCursorPosition(0, currentMessageRow++);
            Console.Write(message);
        }

        private static List<Player> ReadMap(string file)
        {
            List<Player> players = new List<Player>();

            var lines = File.ReadAllLines(file)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Trim())
                .ToList();

            map = new char[lines[0].Length, lines.Count];

            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                foreach (var col in line)
                {
                    switch (col)
                    {
                        case 'E':
                        case 'G':
                            map[x, y] = '.';
                            players.Add(new Player { Coordinate = new Coordinate { X = x, Y = y }, Type = col });
                            break;
                        default:
                            map[x, y] = col;
                            break;
                    }
                    x++;
                }
                y++;
            }

            return players;
        }

        private static List<Coordinate> BreadthFirstSearch(List<Player> players, Coordinate start, Coordinate end)
        {
            // https://en.wikipedia.org/wiki/Breadth-first_search

            Queue<Coordinate> openSet = new Queue<Coordinate>();
            HashSet<Coordinate> closedSet = new HashSet<Coordinate>();
            Dictionary<Coordinate, Coordinate> meta = new Dictionary<Coordinate, Coordinate>();
            List<List<Coordinate>> allPaths = new List<List<Coordinate>>();

            var root = start;
            meta.Add(root, null);
            openSet.Enqueue(start);

            while (openSet.Count > 0)
            {
                var subTreeRoot = openSet.Dequeue();
                //Console.SetCursorPosition(subTreeRoot.X, subTreeRoot.Y);

                if (subTreeRoot.Equals(end))
                {
                    var p = GetPath(subTreeRoot, meta);
                    if (p != null) allPaths.Add(p);
                    break;
                }

                var children = new List<Coordinate>();
                // Test directions in this order: up, left, right, down to first find a path in "reading order"
                if (map[subTreeRoot.X, subTreeRoot.Y - 1].Equals('.') && !(players.Where(p => p.IsAlive && p.Coordinate.X == subTreeRoot.X && p.Coordinate.Y == subTreeRoot.Y - 1).Count() > 0)) children.Add(new Coordinate { X = subTreeRoot.X, Y = subTreeRoot.Y - 1 });
                if (map[subTreeRoot.X - 1, subTreeRoot.Y].Equals('.') && !(players.Where(p => p.IsAlive && p.Coordinate.X == subTreeRoot.X - 1 && p.Coordinate.Y == subTreeRoot.Y).Count() > 0)) children.Add(new Coordinate { X = subTreeRoot.X - 1, Y = subTreeRoot.Y });
                if (map[subTreeRoot.X + 1, subTreeRoot.Y].Equals('.') && !(players.Where(p => p.IsAlive && p.Coordinate.X == subTreeRoot.X + 1 && p.Coordinate.Y == subTreeRoot.Y).Count() > 0)) children.Add(new Coordinate { X = subTreeRoot.X + 1, Y = subTreeRoot.Y });
                if (map[subTreeRoot.X, subTreeRoot.Y + 1].Equals('.') && !(players.Where(p => p.IsAlive && p.Coordinate.X == subTreeRoot.X && p.Coordinate.Y == subTreeRoot.Y + 1).Count() > 0)) children.Add(new Coordinate { X = subTreeRoot.X, Y = subTreeRoot.Y + 1 });

                foreach (var child in children)
                {
                    //Console.SetCursorPosition(child.X, child.Y);

                    // Already included, skip
                    if (closedSet.Contains(child))
                        continue;

                    if (!openSet.Contains(child))
                    {
                        meta.Add(child, subTreeRoot);
                        openSet.Enqueue(child);
                    }
                }

                closedSet.Add(subTreeRoot);
            }

            var chosenPath = new List<Coordinate>();

            if (allPaths.Count == 0) return null;
            if (allPaths.Count == 1)
            {
                chosenPath = allPaths.First();
            }
            else
            {
                Console.WriteLine("FOUND MULTIPLE PATHS!");
                Console.ReadKey();
            }

            return chosenPath;
        }

        private static List<Coordinate> GetPath(Coordinate coordinate, Dictionary<Coordinate, Coordinate> meta)
        {
            List<Coordinate> path = new List<Coordinate>();

            while (meta[coordinate] != null)
            {
                path.Add(meta[coordinate]);
                coordinate = meta[coordinate];
            }
            path.Reverse();
            return path;
        }

        private static void PrintMap()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Console.SetCursorPosition(0, y);
                var line = new StringBuilder();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    line.Append(map[x, y].ToString().Pastel(map[x, y].Equals('.') ? System.Drawing.Color.DarkGray : System.Drawing.Color.White));
                }
                Console.Write(line);
            }

            Console.SetCursorPosition(0, map.GetLength(1) + 1);
        }

        private static void PrintPlayers(List<Player> players)
        {
            foreach (var player in players.Where(p => p.IsAlive))
            {
                var color = player.Type.Equals('E') ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                Console.SetCursorPosition(player.Coordinate.X, player.Coordinate.Y);
                Console.Write(player.Type.ToString().Pastel(color));
            }

            Console.SetCursorPosition(0, map.GetLength(1) + 1);
        }

        private static void PrintPlayer(Player player)
        {
            var curX = Console.CursorLeft;
            var curY = Console.CursorTop;
            var color = player.Type.Equals('E') ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            Console.SetCursorPosition(player.Coordinate.X, player.Coordinate.Y);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(player.Type.ToString().Pastel(color));
            Console.ResetColor();
            Console.SetCursorPosition(curX, curY);
        }

        private static void PrintStats(List<Player> players, int round = 0, int elfAttackPower = 3)
        {
            var curX = Console.CursorLeft;
            var curY = Console.CursorTop;

            var statLeft = map.GetLength(0) + 5;
            if (statLeft < 35) statLeft = 35;
            var statTop = 0;
            Console.SetCursorPosition(statLeft, statTop++);
            Console.Write($"Players:\t{players.Count.ToString().Pastel(System.Drawing.Color.Red)}");
            Console.Write($"\tElf Attack Power:\t{elfAttackPower}");
            Console.SetCursorPosition(statLeft, statTop++);
            Console.Write($"Round:\t{round.ToString().Pastel(System.Drawing.Color.Red)}");
            statTop++;

            foreach (var player in players.OrderBy(p => p.Coordinate.Y).ThenBy(p => p.Coordinate.X))
            {
                Console.SetCursorPosition(statLeft, statTop++);
                if (player.IsAlive)
                {
                    var color = player.Type.Equals('E') ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                    Console.Write($"Player {player.Type.ToString().Pastel(color)} {player.ToString()}: {player.HitPoints,3} HP  ");
                }
                else
                {
                    Console.Write($"Player {player.Type} {player.ToString()}: {"DECEASED".Pastel(System.Drawing.Color.Red)}");
                }
            }

            Console.SetCursorPosition(0, map.GetLength(1) + 1);
        }
    }

    public class Player : ICloneable
    {
        public char Type { get; set; }
        public Coordinate Coordinate { get; set; }
        public int HitPoints { get; set; }
        public int AttackPower { get; set; }

        public Player()
        {
            Coordinate = new Coordinate();
            HitPoints = 200;
            AttackPower = 3;
        }

        public List<Coordinate> InRange(char[,] map, List<Player> players)
        {
            var inRange = new List<Coordinate>();

            if (map[Coordinate.X, Coordinate.Y - 1].Equals('.') && players.Where(p => p.IsAlive && p.Coordinate.X.Equals(Coordinate.X) && p.Coordinate.Y.Equals(Coordinate.Y - 1)).Count() == 0) inRange.Add(new Coordinate { X = Coordinate.X, Y = Coordinate.Y - 1 });
            if (map[Coordinate.X + 1, Coordinate.Y].Equals('.') && players.Where(p => p.IsAlive && p.Coordinate.X.Equals(Coordinate.X + 1) && p.Coordinate.Y.Equals(Coordinate.Y)).Count() == 0) inRange.Add(new Coordinate { X = Coordinate.X + 1, Y = Coordinate.Y });
            if (map[Coordinate.X - 1, Coordinate.Y].Equals('.') && players.Where(p => p.IsAlive && p.Coordinate.X.Equals(Coordinate.X - 1) && p.Coordinate.Y.Equals(Coordinate.Y)).Count() == 0) inRange.Add(new Coordinate { X = Coordinate.X - 1, Y = Coordinate.Y });
            if (map[Coordinate.X, Coordinate.Y + 1].Equals('.') && players.Where(p => p.IsAlive && p.Coordinate.X.Equals(Coordinate.X) && p.Coordinate.Y.Equals(Coordinate.Y + 1)).Count() == 0) inRange.Add(new Coordinate { X = Coordinate.X, Y = Coordinate.Y + 1 });

            return inRange;
        }

        public override string ToString()
        {
            return $"({Type}:{Coordinate.X,2},{Coordinate.Y,2})";
        }

        internal bool IsNearby(Player enemy)
        {
            return (
                (Coordinate.X.Equals(enemy.Coordinate.X - 1) && Coordinate.Y.Equals(enemy.Coordinate.Y)) ||
                (Coordinate.X.Equals(enemy.Coordinate.X + 1) && Coordinate.Y.Equals(enemy.Coordinate.Y)) ||
                (Coordinate.X.Equals(enemy.Coordinate.X) && Coordinate.Y.Equals(enemy.Coordinate.Y - 1)) ||
                (Coordinate.X.Equals(enemy.Coordinate.X) && Coordinate.Y.Equals(enemy.Coordinate.Y + 1))
            );
        }

        internal bool TakeHitFrom(Player player)
        {
            return ((HitPoints -= player.AttackPower) <= 0);
        }

        internal void MoveTo(Coordinate coordinate)
        {
            Coordinate = coordinate;
        }

        public object Clone()
        {
            var p = new Player();
            p.Type = Type;
            p.Coordinate.X = Coordinate.X;
            p.Coordinate.Y = Coordinate.Y;

            return p;
        }

        internal bool IsDead => HitPoints <= 0;
        internal bool IsAlive => HitPoints > 0;
    }

    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Coordinate other)
        {
            if (other is null) return false;

            return other.X == X && other.Y == Y;
        }
        public override bool Equals(object obj) => Equals(obj as Coordinate);
        public override int GetHashCode() => (X, Y).GetHashCode();
        public override string ToString() => $"({X},{Y})";

    }

    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}

