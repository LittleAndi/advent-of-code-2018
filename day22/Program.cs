using System;
using System.Collections.Generic;

namespace day22
{
    class Program
    {
        static int[,] erosionLevels;

        static void Main(string[] args)
        {
            System.Console.WriteLine(CalculateRisk(510, 10, 10));
            System.Console.WriteLine(CalculateRisk(10647, 7, 770));


        }

        private static int CalculateRisk(int depth, int targetX, int targetY)
        {
            int totalRiskLevel = 0;
            erosionLevels = new int[targetX + 1, targetY + 1];

            for (int y = 0; y <= targetY; y++)
            {
                for (int x = 0; x <= targetX; x++)
                {
                    int geologicIndex = 0;
                    if (x == 0 && y == 0) geologicIndex = 0;
                    else if (x == targetX && y == targetY) geologicIndex = 0;
                    else if (y == 0) geologicIndex = x * 16807;
                    else if (x == 0) geologicIndex = y * 48271;
                    else
                    {
                        geologicIndex = erosionLevels[x - 1, y] * erosionLevels[x, y - 1];
                    }

                    int erosionLevel = (geologicIndex + depth) % 20183;

                    erosionLevels[x, y] = erosionLevel;

                    totalRiskLevel += erosionLevel % 3;
                }
            }

            return totalRiskLevel;
        }

        private static List<Coordinate> BreadthFirstSearch(Coordinate start, Coordinate end)
        {
            // https://en.wikipedia.org/wiki/Breadth-first_search

            Queue<Coordinate> openSet = new Queue<Coordinate>();
            HashSet<Coordinate> closedSet = new HashSet<Coordinate>();
            Dictionary<Coordinate, Coordinate> meta = new Dictionary<Coordinate, Coordinate>();

            var root = start;
            meta.Add(root, null);
            openSet.Enqueue(start);

            while (openSet.Count > 0)
            {
                var subTreeRoot = openSet.Dequeue();

                if (subTreeRoot.Equals(end))
                    return GetPath(subTreeRoot, meta);

                var children = new List<Coordinate>();
                // Test directions

                foreach (var child in children)
                {
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

            return null;
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
}
