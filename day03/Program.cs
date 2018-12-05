using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day03
{
    class Program
    {
        static void Main(string[] args)
        {
            var fabrics = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => new Fabric { Line = l })
                .ToArray();

            System.Console.WriteLine(fabrics.Max(l => l.ID));
            System.Console.WriteLine(fabrics.Max(l => l.X));
            System.Console.WriteLine(fabrics.Max(l => l.Y));
            System.Console.WriteLine(fabrics.Max(l => l.W));
            System.Console.WriteLine(fabrics.Max(l => l.H));

            Dictionary<int, int> overlappingFabrics = new Dictionary<int, int>();

            int overlapping = 0;
            Dictionary<Coordinate, int> totalOverlappingCoordinates = new Dictionary<Coordinate, int>();

            for (int i = 0; i < fabrics.Length; i++)
            {
                for (int j = i+1; j < fabrics.Length; j++)
                {
                    var overlappingCoordiantes = fabrics[i].OverlappingCoordinates(fabrics[j]);
                    foreach (var overlappingCoord in overlappingCoordiantes)
                    {
                        if (totalOverlappingCoordinates.ContainsKey(overlappingCoord))
                        {
                            totalOverlappingCoordinates[overlappingCoord]++;
                        }
                        else
                        {
                            totalOverlappingCoordinates.Add(overlappingCoord, 1);
                        }
                    }
                    if (overlappingCoordiantes.Count > 0) {
                        System.Console.WriteLine($"{i},{j} - {fabrics[i].ID} overlapping {fabrics[j].ID}");
                        overlapping++;
                    }
                }
            }

            System.Console.WriteLine(overlapping);
            System.Console.WriteLine(totalOverlappingCoordinates.Count);
        }
    }

    public class Fabric
    {
        public string Line { get; set; }
        public int ID
        {
            get
            {
                var id = int.Parse(Line.Split(' ')[0].Replace("#", ""));
                return id;
            }
        }
        public int X
        {
            get
            {
                var x = int.Parse(Line.Split(' ')[2].Split(',')[0]);
                return x;
            }
        }
        public int Y
        {
            get
            {
                var y = int.Parse(Line.Split(' ')[2].Split(',')[1].Replace(":",""));
                return y;
            }
        }
        public int W
        {
            get
            {
                var w = int.Parse(Line.Split(' ')[3].Split('x')[0]);
                return w;
            }
        }
        public int H
        {
            get
            {
                var h = int.Parse(Line.Split(' ')[3].Split('x')[1]);
                return h;
            }
        }

        public bool Overlap(Fabric input)
        {
            // x1 <= y2 && y1 <= x2
            var objA = this;
            var objB = input;
            if (objA.X + 1 <= objB.X + 1 + objB.W - 1 && objB.X + 1 <= objA.X + 1 + objA.W - 1)
            {
                if (objA.Y + 1 <= objB.Y + 1 + objB.H - 1 && objB.Y + 1 <= objA.Y + 1 + objA.H - 1)
                {
                    return true;
                }
            }
            return false;
        }

        public int OverlappingArea(Fabric input)
        {   
            var aX = Enumerable.Range(this.X+1, this.W);
            var bX = Enumerable.Range(input.X+1, input.W);
            var aY = Enumerable.Range(this.Y+1, this.H);
            var bY = Enumerable.Range(input.Y+1, input.H);

            int overlappingX = 0;
            foreach (var x in aX)
            {
                if (bX.Contains(x)) overlappingX++;
            }
            int overlappingY = 0;
            foreach (var y in aY)
            {
                if (bY.Contains(y)) overlappingY++;
            }

            return overlappingX * overlappingY;
        }

        public List<Coordinate> OverlappingCoordinates(Fabric input)
        {   
            var overlappingCoordinates = new List<Coordinate>();
            var aX = Enumerable.Range(this.X+1, this.W);
            var bX = Enumerable.Range(input.X+1, input.W);
            var aY = Enumerable.Range(this.Y+1, this.H);
            var bY = Enumerable.Range(input.Y+1, input.H);

            foreach (var x in aX)
            {
                foreach (var y in aY)
                {
                    if (bX.Contains(x) && bY.Contains(y))
                    {
                        overlappingCoordinates.Add(new Coordinate{ X = x, Y = y});
                    }
                }
            }

            return overlappingCoordinates;
        }


    }

    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Coordinate other)
        {
            return other.X == X && other.Y == Y;
        }
    }
}
