using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            Regex rx = new Regex(@"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)");
            
            List<NanoBot> nanoBots = new List<NanoBot>();
            foreach (var item in lines)
            {
                MatchCollection matches = rx.Matches(item);

                foreach (Match match in matches)
                {
                    var groups = match.Groups;

                    long px = long.Parse(groups[1].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    long py = long.Parse(groups[2].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    long pz = long.Parse(groups[3].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    long r = long.Parse(groups[4].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);

                    nanoBots.Add(new NanoBot { X = px, Y = py, Z = pz, Radius = r });
                }
            }

            var bestRadius = nanoBots.Max(n => n.Radius);
            var nanoBot = nanoBots.First(n => n.Radius == bestRadius);

            Console.WriteLine(nanoBots.Where(n => n.ManhattanDistanceFrom(nanoBot) <= bestRadius).Count());

        }
    }

    public class NanoBot
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }
        public long Radius { get; set; }
        public long ManhattanDistance => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        public long ManhattanDistanceFrom(NanoBot nanoBot)
        {
            return Math.Abs(nanoBot.X - X) + Math.Abs(nanoBot.Y - Y) + Math.Abs(nanoBot.Z - Z);
        }
    }
}
