using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l, CultureInfo.InvariantCulture))
                .ToList();

            int total = 0;
            int currentLoop = 0;
            HashSet<int> frequencies = new HashSet<int>();
            frequencies.Add(0); // Add zero as the first occuring frequency
            while (true)
            {
                currentLoop++;
                foreach (var i in numbers)
                {
                    total += i;
                    if (!frequencies.Add(total))
                    {
                        System.Console.WriteLine($"First repeated frequency is {total} in loop {currentLoop}");
                        return;
                    }
                }
                System.Console.WriteLine($"Loop {currentLoop} => {total}");
            }
        }
    }
}
