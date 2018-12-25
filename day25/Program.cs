using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            List<Constellation> constellations = new List<Constellation>();
            List<Coord> coords = new List<Coord>();
            foreach (var line in lines)
            {
                var s = line.Split(',');

                var x = (int.Parse(s[0], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture));
                var y = (int.Parse(s[1], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture));
                var z = (int.Parse(s[2], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture));
                var t = (int.Parse(s[3], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture));

                var distance = Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(t);
                var coord = new Coord(x, y, z, t);
                coords.Add(coord);
            }

            var c = new Constellation();
            c.Distances.Add(coords[0]);
            constellations.Add(c);
            coords.RemoveAt(0);

            int lastCount = 0;
            while (coords.Count() > 0)
            {
                foreach (var coord in coords)
                {
                    foreach (var constellation in constellations)
                    {
                        if (constellation.InRange(coord))
                        {
                            constellation.Distances.Add(coord);
                        }
                    }
                }

                foreach (var constellation in constellations)
                {
                    foreach (var item in constellation.Distances)
                    {
                        coords.Remove(item);
                    }
                }

                if (coords.Count() == lastCount)
                {
                    var newC = new Constellation();
                    newC.Distances.Add(coords[0]);
                    constellations.Add(newC);
                    coords.RemoveAt(0);
                }

                lastCount = coords.Count();
            }

            //bool foundOne = false;
            //foreach (var constellation in constellations)
            //{
            //    if (constellation.InRange(coord))
            //    {
            //        foundOne = true;
            //        constellation.Distances.Add(coord);
            //    }
            //}
            //if (!foundOne)
            //{
            //    var constellation = new Constellation();
            //    constellation.Distances.Add(coord);
            //    constellations.Add(constellation);
            //}


            Console.WriteLine(constellations.Count());

            // 464 (too high!)
            // 598 (too high!)
        }
    }

    public class Constellation
    {
        public Constellation()
        {
            Distances = new List<Coord>();
        }

        public List<Coord> Distances { get; set; }

        public bool InRange(Coord coord) => (Distances.Where(c => Math.Abs(c.X - coord.X) + Math.Abs(c.Y - coord.Y) + Math.Abs(c.Z - coord.Z) + Math.Abs(c.T - coord.T) <= 3).Count() > 0);
    }

    public class Coord
    {
        public Coord(int x, int y, int z, int t)
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int T { get; set; }

        public override string ToString()
        {
            return $"{X},{Y},{Z},{T}";
        }
    }
}
