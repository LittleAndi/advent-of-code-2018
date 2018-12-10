using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var stars = File.ReadAllLines("input.txt")
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((l, i) => new Star() { Input = l, ID = i })
            .ToList();

            foreach (var item in stars)
            {
                item.ProcessInput();
            }

            bool keepCounting = true;
            int seconds = 0;
            while (keepCounting)
            {
                seconds++;

                //Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.Write($"{seconds, 5}");
                foreach (var item in stars)
                {
                    item.Move();
                }

                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"{stars.Min(s => s.Y), 7}");
                Console.WriteLine($"{stars.Max(s => s.Y), 7}");

                if (stars.Max(s => s.Y) - stars.Min(s => s.Y) < 10)
                {
                    var result = PrintStars(stars);
                    Console.SetCursorPosition(0, 20);
                    return;
                }

                // Print
            }

            //Console.WriteLine(stars.Count);
        }

        private static bool PrintStars(List<Star> stars)
        {
            var allInside = true;
            var minX = stars.Min(s => s.X);
            var minY = stars.Min(s => s.Y) - 3;
            var maxX = stars.Max(s => s.X);
            var maxY = stars.Max(s => s.Y);

            foreach (var item in stars)
            {
                if (item.X-minX < 0 || item.Y-minY < 0) { allInside = false; break; }
                if (item.X-minX > Console.WindowWidth || item.Y-minX > Console.WindowHeight) { allInside = false; break; }
                Console.SetCursorPosition(item.X-minX, item.Y-minY);
                Console.Write('X');
            }

            return allInside;
        }
    }

    public class Star
    {
        public int ID { get; set; }
        public string Input { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int VelocityX { get; private set; }
        public int VelocityY { get; private set; }

        public void ProcessInput()
        {
            var l = Input.Replace("position=", "");
            l = l.Replace("velocity=", "");
            var position = l.Split("> <")[0].Replace("<", "");
            var velocity = l.Split("> <")[1].Replace(">", "");
            X = int.Parse(position.Split(',')[0].Trim());
            Y = int.Parse(position.Split(',')[1].Trim());
            VelocityX = int.Parse(velocity.Split(',')[0].Trim());
            VelocityY = int.Parse(velocity.Split(',')[1].Trim());
        }

        public void Move()
        {
            X += VelocityX;
            Y += VelocityY;
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
