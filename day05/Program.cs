using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var polymerSequence = File.ReadAllLines("input.txt")[0];
            var polymer = new Polymer { PolymerSequence = polymerSequence };
            var polymerRemainder = polymer.React();
            System.Console.WriteLine(polymerRemainder);
            System.Console.WriteLine(polymerRemainder.Length);

            // 11754

            var shortestRemainder = polymer.ReactWithoutTroubelingType();
            System.Console.WriteLine(shortestRemainder);
            System.Console.WriteLine(shortestRemainder.Length);

            // 4098
        }
    }

    public class Polymer
    {
        public string PolymerSequence { get; set; }

        public string React()
        {
            var result = PolymerSequence;
            string previousResult = "";
            while (result != previousResult)
            {
                previousResult = result;
                result = removeFirst(result);
            }
            return result;
        }

        private string removeFirst(string input)
        {
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] != input[i-1] && (input[i] == input[i-1] + 32 || input[i] + 32 == input[i-1]))
                {
                    return input.Remove(i-1, 2);
                }
            }
            return input;
        }


        public string ReactWithoutTroubelingType()
        {
            var shortest = PolymerSequence;

            for (int i = 65; i <= 65 + 25; i++)
            {
                System.Console.Write($"Testing with {(char)i}: ");
                var test = PolymerSequence.Replace(((char)i).ToString(), string.Empty, true, CultureInfo.InvariantCulture);

                string previousResult = "";
                while (test != previousResult)
                {
                    previousResult = test;
                    test = removeFirst(test);
                }
                System.Console.WriteLine(test.Length);
                if (test.Length < shortest.Length) shortest = test;
            }

            return shortest;
        }
    }
}
