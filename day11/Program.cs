using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace day11
{

    class Program
    {
        static void Main(string[] args)
        {

            var gridSerialNumber = 1133;

            Console.WriteLine("Tests:");
            Rack r1 = new Rack { X = 122, Y = 79, GridSerialID = 57 };
            Rack r2 = new Rack { X = 217, Y = 196, GridSerialID = 39 };
            Rack r3 = new Rack { X = 101, Y = 153, GridSerialID = 71 };
            Rack r4 = new Rack { X = 3, Y = 5, GridSerialID = 8 };
            Console.WriteLine($"{r1.PowerLevel}, {r2.PowerLevel}, {r3.PowerLevel}, {r4.PowerLevel}");

            Console.WriteLine("\nPart 1:");
            Part1(gridSerialNumber);

            Console.WriteLine("\nPart 2:");
            Part2(gridSerialNumber);
        }

        private static void Part1(int gridSerialNumber)
        {
            int maxPower = -10;
            int maxX = 0;
            int maxY = 0;
            for (int x = 0; x < 300 - 3; x++)
            {
                for (int y = 0; y < 300 - 3; y++)
                {
                    var racks = new List<Rack>();
                    racks.Add(new Rack { X = x + 1, Y = y + 1, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 2, Y = y + 1, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 3, Y = y + 1, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 1, Y = y + 2, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 2, Y = y + 2, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 3, Y = y + 2, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 1, Y = y + 3, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 2, Y = y + 3, GridSerialID = gridSerialNumber });
                    racks.Add(new Rack { X = x + 3, Y = y + 3, GridSerialID = gridSerialNumber });
                    var totalPower = racks.Sum(r => r.PowerLevel);
                    if (totalPower > maxPower)
                    {
                        maxPower = totalPower;
                        maxX = x + 1;
                        maxY = y + 1;
                    }
                }
            }

            Console.WriteLine($"({maxX},{maxY})");

        }

        private static void Part2(int gridSerialNumber)
        {
            int maxPower = int.MinValue;
            int maxX = 0;
            int maxY = 0;
            int maxSize = 0;
            var racks = new List<Rack>();

            int[,] sat = new int[301,301];
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    var r = new Rack
                    {
                        X = x,
                        Y = y,
                        GridSerialID = gridSerialNumber
                    };
                    sat[x, y] = r.PowerLevel + sat[x - 1, y] + sat[x, y - 1] - sat[x - 1, y - 1];
                }
            }

            for (int size = 1; size <= 300; size++)
            {
                for (int x = size; x <= 300; x++)
                {
                    for (int y = size; y <= 300; y++)
                    {
                        var total = sat[x, y] + sat[x - size, y - size] - sat[x - size, y] - sat[x, y - size];
                        if (total > maxPower)
                        {
                            maxPower = total;
                            maxX = x;
                            maxY = y;
                            maxSize = size;
                        }
                    }
                }
            }

            Console.WriteLine($"({maxX-maxSize+1},{maxY-maxSize+1},{maxSize})");
        }
    }

    public class Rack
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int RackID => X + 10;
        public int GridSerialID { get; set; }

        public int PowerLevel
        {
            get
            {
                var power = RackID * Y;
                power += GridSerialID;
                power = power * RackID;
                power = power / 100 % 10;
                power -= 5;

                return power;
            }
        }
    }

}

