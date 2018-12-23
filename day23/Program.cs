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

                    nanoBots.Add(new NanoBot(px, py, pz, r));
                }
            }

            var bestRadius = nanoBots.Max(n => n.Radius);
            var nanoBot = nanoBots.First(n => n.Radius == bestRadius);

            Console.WriteLine(nanoBots.Where(n => n.ManhattanDistanceFrom(nanoBot) <= bestRadius).Count());

            Random rand = new Random(DateTime.Now.Millisecond);

            //int minx = (int)nanoBots.Min(n => n.X);
            //int miny = (int)nanoBots.Min(n => n.Y);
            //int minz = (int)nanoBots.Min(n => n.Z);

            //int maxx = (int)nanoBots.Max(n => n.X);
            //int maxy = (int)nanoBots.Max(n => n.Y);
            //int maxz = (int)nanoBots.Max(n => n.Z);

            int minx = 18140327 - 400000; // (int)nanoBots.Min(n => n.X);
            int miny = 52767466 - 400000; // (int)nanoBots.Min(n => n.Y);
            int minz = 58536384 - 400000; // (int)nanoBots.Min(n => n.Z);

            int maxx = 18140327 + 400000; // (int)nanoBots.Min(n => n.X);
            int maxy = 52767466 + 400000; // (int)nanoBots.Min(n => n.Y);
            int maxz = 58536384 + 400000; // (int)nanoBots.Min(n => n.Z);

            int nanoBotCount = 0;
            long shortestDistance = long.MaxValue;
            while(nanoBotCount < 1000)
            {
                long x = rand.Next(minx, maxx);
                long y = rand.Next(miny, maxy);
                long z = rand.Next(minz, maxz);
                var testBot = new NanoBot(x, y, z, 0);
                var c = CountNanoBotInRange(testBot, nanoBots);
                if (c >= nanoBotCount && testBot.ManhattanDistance < shortestDistance)
                {
                    nanoBotCount = c;
                    shortestDistance = testBot.ManhattanDistance;
                    Console.WriteLine($"{x},{y},{z} - {nanoBotCount} - {shortestDistance}");

                }
            }

            // 133894881 (to high)
            // 19210606,52359779,59674126 - 878 - 131244511
            // 18887560,53186405,58963532 - 907 - 131037497
            // 18140327,52767466,58536384 - 910 - 129444177
            // 18174334,52961133,58308710 - 910 - 129444177
        }

        private static int CountNanoBotInRange(NanoBot coord, List<NanoBot> nanoBots)
        {
            int nanoBotsInRange = 0;

            foreach (var item in nanoBots)
            {
                if (item.ManhattanDistanceFrom(coord) <= item.Radius) nanoBotsInRange++;
            }

            return nanoBotsInRange;
        }
    }

    public class NanoBot
    {
        public NanoBot(long x, long y, long z, long radius)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
        }
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
