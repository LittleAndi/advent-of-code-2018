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
            HashSet<int> frequencies = new HashSet<int>();
            using (TextReader tr = new StreamReader("input.txt"))
            {
                string s = "";
                while ((s = tr.ReadLine()) != null)
                {
                    numbers.Add(int.Parse(s, CultureInfo.InvariantCulture));
                }
            }

            int total = 0;
            bool lookForRepeatedFrequency = true;
            int loops = 0;
            while (lookForRepeatedFrequency)
            {
                loops++;
                foreach (var i in numbers)
                {
                    total += i;
                    if (!frequencies.Add(total))
                    {
                        System.Console.WriteLine($"First repeated frequency is {total} in {loops} loops");
                        return;
                    }
                }
                System.Console.WriteLine($"Loop {loops} => {total}");
            }
        }
    }
}
