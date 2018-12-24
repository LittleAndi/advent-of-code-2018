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
            var lines = File.ReadAllLines("4mbQp63d.txt")
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

            int minx = (int)nanoBots.Min(n => n.X);
            int miny = (int)nanoBots.Min(n => n.Y);
            int minz = (int)nanoBots.Min(n => n.Z);

            int maxx = (int)nanoBots.Max(n => n.X);
            int maxy = (int)nanoBots.Max(n => n.Y);
            int maxz = (int)nanoBots.Max(n => n.Z);

            // 29607097,24714116,21505713 - 792 - 75826926
            // 29525842,25526982,20611543 - 794 - 75664367
            // 28954468,28192597,18942472 - 806 - 76089537
            // 30692673,27853845,20342109 - 807 - 78888627
            // 29673844,27828295,19297564 - 807 - 76799703
            // 28676693,28559797,19031893 - 807 - 76268383
            // 30098528,28132078,20026039 - 810 - 78256645
            // 31200425,28626385,21622286 - 820 - 81449096
            // 30961419,28330291,21087113 - 828 - 80378823
            // 32307507,27879562,21982488 - 831 - 82169557
            // 31910164,28404804,25446586 - 897 - 85761554
            // 31558845,28696915,25505789 - 898 - 85761549
            // 31215521,29323815,25222212 - 900 - 85761548
            // 31181550,29395104,25184889 - 901 - 85761543
            // 31767774,29734171,24259597 - 901 - 85761542
            // 31178263,29401527,25181752 - 901 - 85761542
            // 32026134,28928588,24806818 - 901 - 85761540
            // 31752012,28713402,25296134 - 904 - 85761548
            // 31582286,28898937,25280322 - 904 - 85761545
            // 32127425,28488582,25145539 - 905 - 85761546
            // 32743749,28288752,24729044 - 908 - 85761545
            // 32602674,28633059,24525809 - 908 - 85761542
            // 32929611,28476388,24355543 - 908 - 85761542

            int startx = (32602674 + 32743749 + 32929611) / 3;
            int starty = (28633059 + 28288752 + 28476388) / 3;
            int startz = (24525809 + 24729044 + 24355543) / 3;
            int halfwidth = 500000;

            int nanoBotCount = 908;
            long shortestDistance = long.MaxValue;
            while(nanoBotCount < 1000)
            {
                long x = rand.Next(startx - halfwidth, startx + halfwidth);
                long y = rand.Next(starty - halfwidth, starty + halfwidth);
                long z = rand.Next(startz - halfwidth, startz + halfwidth);
                var testBot = new NanoBot(x, y, z, 0);
                var c = CountNanoBotInRange(testBot, nanoBots);
                if (c >= nanoBotCount && testBot.ManhattanDistance < shortestDistance)
                {
                    if (c >= nanoBotCount)
                    {
                        // reposition
                        startx = (int)x;
                        starty = (int)y;
                        startz = (int)z;
                    }

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

            // 3654844,7903105,14525020 - 531 - 26082969
            // 4651153,6962954,14314250 - 531 - 25928357
            // 4628654,6976695,14313107 - 531 - 25918456
            // 4648990,6989456,14277033 - 531 - 25915479
            // 4738112,7013694,14337128 - 534 - 26088934
            // 4839722,7156773,14335572 - 535 - 26332067

            // 17732928,20129093,26296159 - 718 - 64158180
            // 24512362,22993877,19580797 - 739 - 67087036
            // 22417948,24190862,28762700 - 770 - 75371510
            // 26392236,25640054,23209918 - 778 - 75242208
            // 28801176,24012893,21959728 - 782 - 74773797
            // 28946359,24168195,21473178 - 782 - 74587732
            // 28818169,23602818,21828061 - 783 - 74249048
            // 28786236,24154454,21244513 - 783 - 74185203
            // 28993326,23903816,21702251 - 785 - 74599393
            // 28881321,23629961,21864044 - 785 - 74375326
            // 28777677,23754010,21636354 - 785 - 74168041
            // 28747597,23732545,21627731 - 785 - 74107873

            // 29024953,22909993,22727675 - 787 - 74662621
            // 29024953,22909993,22727675 - 787 - 74662621
            // 29045274,24010858,21647095 - 790 - 74703227
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
