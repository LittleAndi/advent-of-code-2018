using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var locations = File.ReadAllLines("input.txt")
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((l, i) => new Location { LocationId = i, X = int.Parse(l.Split(", ")[0]), Y = int.Parse(l.Split(", ")[1]) })
            .ToList();

            foreach (var item in locations)
            {
                System.Console.WriteLine($"{item.LocationId}: {item.X}, {item.Y}");
            }

            // Fill'er up!
            var coordinates = new List<Coordinate>();
            var minX = locations.OrderBy(l => l.X).First().X;
            var maxX = locations.OrderByDescending(l => l.X).First().X;
            var minY = locations.OrderBy(l => l.Y).First().Y;
            var maxY = locations.OrderByDescending(l => l.Y).First().Y;
            for (int y = minY - minY; y <= maxY + minY; y++)
            {
                for (int x = minX - minX; x <= maxX + minX; x++)
                {
                    var coordinate = new Coordinate { X = x, Y = y, IsEdge = (x == minX - minX || x == maxX + minX || y == minY - minY || y == maxY + minY), IsLocation = false };

                    var l = new Location { X = x, Y = y };
                    if (locations.Contains(l))
                    {
                        //Console.Write(locations.First(m => m.Equals(l)).LocationId);
                        var location = locations.First(m => m.Equals(l));
                        coordinate.IsLocation = true;
                        foreach (var testLocation in locations)
                        {
                            var distance = Math.Abs(testLocation.X - x) + Math.Abs(testLocation.Y - y);
                            coordinate.LocationDistances.Add(testLocation.LocationId, distance);
                        }
                        //tw.Write(coordiante.NearestLocationId);
                    }
                    else
                    {
                        foreach (var testLocation in locations)
                        {
                            var distance = Math.Abs(testLocation.X - x) + Math.Abs(testLocation.Y - y);
                            coordinate.LocationDistances.Add(testLocation.LocationId, distance);
                        }
                        //tw.Write(coordiante.NearestLocationId.ToString().ToLower());
                    }

                    coordinates.Add(coordinate);
                }
                //tw.WriteLine();
            }

            var area = new Area { Coordinates = coordinates };
            // Output
            //area.WriteFileOutput();

            // Part 1
            var largestSubArea = area.LargestSubArea;
            System.Console.WriteLine($"Location with id {area.LargestSubAreaId} has the largest area of {largestSubArea}");
            // 95472 (too high)
            // 163152 (well...)
            // 3260 (yes...)

            // Part 2
            var closestAreaToAllLocations = area.ClosestAreaToAllLocations(10000);
            System.Console.WriteLine($"Size of area closest to all locations is {closestAreaToAllLocations}");
            // 42561 (too high)
            // 42535 (yay!)

        }
    }

    public class Location : IEquatable<Location>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int LocationId { get; set; }

        public bool Equals(Location other)
        {
            return (other.X == this.X && other.Y == this.Y);
        }
    }

    public class Coordinate
    {
        public Coordinate()
        {
            LocationDistances = new Dictionary<int, int>();
        }
        public int X { get; set; }
        public int Y { get; set; }
        public Dictionary<int, int> LocationDistances { get; set; }
        public int NearestLocationId
        {
            get
            {
                if (LocationDistances.Count == 0) return 0;
                if (LocationDistances.Count < 2) return LocationDistances.First().Key;
                var first = LocationDistances.OrderBy(l => l.Value).First();
                var second = LocationDistances.OrderBy(l => l.Value).Skip(1).First();
                if (first.Value == second.Value) return 0;
                return first.Key;
            }
        }
        public bool IsEdge { get; set; }
        public bool IsLocation { get; set; }
    }

    public class Area
    {
        private int _largestSubAreaId = 0;
        public Area()
        {
            Coordinates = new List<Coordinate>();
        }
        public List<Coordinate> Coordinates { get; set; }
        public int LargestSubAreaId
        {
            get
            {
                return _largestSubAreaId;
            }
        }
        public int LargestSubArea
        {
            get
            {
                var coordinates = Coordinates.ToList();
                System.Console.WriteLine(coordinates.Count);
                var removeThese = Coordinates.Where(c => c.IsEdge).ToList();
                System.Console.WriteLine($"Removing {removeThese.Count} coords...");
                HashSet<int> removedIds = new HashSet<int>();
                foreach (var removeThis in removeThese)
                {
                    int nearestId = removeThis.NearestLocationId;
                    if (!removedIds.Contains(nearestId))
                    {
                        System.Console.Write($"({removeThis.X},{removeThis.Y}): {nearestId} - ");
                        coordinates.RemoveAll(p => p.NearestLocationId == nearestId);
                        System.Console.WriteLine(coordinates.Count);
                        removedIds.Add(nearestId);
                    }
                }

                var distinctCoords = coordinates
                .GroupBy(c => c.NearestLocationId)
                .Select(g => g.First())
                .ToList();

                Dictionary<int, int> aggregation = new Dictionary<int, int>();
                foreach (var distinctCoord in distinctCoords)
                {
                    var nId = distinctCoord.NearestLocationId;
                    aggregation.Add(nId, coordinates.Count(c => c.NearestLocationId.Equals(nId)));
                }

                var largestSubArea = aggregation.OrderByDescending(a => a.Value).First();
                _largestSubAreaId = largestSubArea.Key;
                return largestSubArea.Value;
            }
        }

        public int ClosestAreaToAllLocations(int maxDistance)
        {
            var t = Coordinates.Where(c => c.LocationDistances.Sum(l => l.Value) < maxDistance);
            // foreach (var c in t)
            // {
            //     System.Console.WriteLine($"({c.X},{c.Y})");
            // }
            return t.Count();
        }

        public void WriteFileOutput()
        {
            var minX = Coordinates.Min(c => c.X);
            var maxX = Coordinates.Max(c => c.X);
            var minY = Coordinates.Min(c => c.Y);
            var maxY = Coordinates.Max(c => c.Y);

            using (TextWriter tw = new StreamWriter("output_mini.txt", false))
            {
                for (int y = minY; y <= maxY; y++)
                {
                    StringBuilder row = new StringBuilder();
                    for (int x = minX; x <= maxX; x++)
                    {
                        row.Append((Coordinates.First(c => c.X == x && c.Y == y).NearestLocationId));
                    }
                    tw.WriteLine(row.ToString());
                }
            }
        }
    }
}
