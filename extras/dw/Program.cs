using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dw
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = Part1();
            var p2 = Part2();

            Console.WriteLine($"51{p1}0-af1{p2}");
        }

        private static string Part1()
        {
            List<int> primes = new List<int>();
            Dictionary<string, int> pairs = new Dictionary<string, int>();
            for (int i = 10000; i < 99999; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }

            foreach (var item in primes)
            {
                for (int i = 0; i < 4; i++)
                {
                    var pair = item.ToString().Substring(i, 2);
                    if (pairs.ContainsKey(pair))
                    {
                        pairs[pair]++;
                    }
                    else
                    {
                        pairs.Add(pair, 1);
                    }
                }
            }

            var mostRecurringPair = pairs.OrderByDescending(p => p.Value).First();
            Console.WriteLine($"{mostRecurringPair.Key}: {mostRecurringPair.Value}");

            return mostRecurringPair.Key;
        }

        private static string Part2()
        {
            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            string pi = string.Concat(lines).Trim();

            int zeroes = 0;
            int pos = 0;
            int sum = 0;
            while (zeroes < 3435)
            {
                int digit;
                if (int.TryParse(pi[pos].ToString(), out digit))
                {
                    if (digit == 0) zeroes++;
                    if (digit == 4) sum += 2;
                    if (digit == 3) sum += 17;
                }
                else
                {
                    pos++;
                    continue;
                }
                pos++;
            }

            return sum.ToString();
        }

        private static bool IsPrime(int candidate)
        {
            // Test whether the parameter is a prime number.
            if ((candidate & 1) == 0)
            {
                if (candidate == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Note:
            // ... This version was changed to test the square.
            // ... Original version tested against the square root.
            // ... Also we exclude 1 at the end.
            for (int i = 3; (i * i) <= candidate; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }
            return candidate != 1;
        }
    }
}
