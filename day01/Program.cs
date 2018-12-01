using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>();
            using (TextReader tr = new StreamReader("input.txt"))
            {
                string s = "";
                while ((s = tr.ReadLine()) != null)
                {
                    numbers.Add(int.Parse(s, CultureInfo.InvariantCulture));
                }
            }

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
