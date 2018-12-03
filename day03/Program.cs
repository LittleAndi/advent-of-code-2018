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
            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => new Fabric { Line = l })
                .ToArray();

            System.Console.WriteLine(lines.Max(l => l.ID));
            System.Console.WriteLine(lines.Max(l => l.X));
            System.Console.WriteLine(lines.Max(l => l.Y));
            System.Console.WriteLine(lines.Max(l => l.W));
            System.Console.WriteLine(lines.Max(l => l.H));

            //int[,,] area = new int[1050, 1050, 1];

            var testlines = new List<Fabric>();
            testlines.Add(new Fabric{ Line = "#1 @ 1,3: 4x4" });
            testlines.Add(new Fabric{ Line = "#2 @ 3,1: 4x4" });
            testlines.Add(new Fabric{ Line = "#3 @ 5,5: 2x2" });
            testlines.Add(new Fabric{ Line = "#806 @ 728,868: 16x18"});
            testlines.Add(new Fabric{ Line = "#811 @ 731,816: 12x17"});

            System.Console.WriteLine(testlines[0].Overlap(testlines[1]));
            System.Console.WriteLine(testlines[0].Overlap(testlines[2]));
            System.Console.WriteLine(testlines[1].Overlap(testlines[2]));
            System.Console.WriteLine(testlines[3].Overlap(testlines[4]));

            int overlapping = 0;            
            for (int i = 0; i < 1335; i++)
            {
                for (int j = i+1; j < 1335; j++)
                {
                    var overlap = lines[i].Overlap(lines[j]);
                    if (overlap) {
                        //System.Console.WriteLine($"{i},{j} - {lines[i].ID} overlapping {lines[j].ID}");
                        overlapping++;
                    }
                }
            }

            System.Console.WriteLine(overlapping);
            
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
            if (objA.X <= objB.X + objB.W - 1 && objB.X <= objA.X + objA.W - 1)
            {
                if (objA.Y < objB.Y + objB.H - 1 && objB.Y < objA.Y + objA.H - 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
