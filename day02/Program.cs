using System;
using System.Linq;

namespace day02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World again!");
        }
    }

    public class Box
    {
        public string ID { get; set; }

        public bool HasExactlyTwoOfAnyLetter
        {
            get
            {
                return HasOccurrenceOfN(ID, 2);
            }
        }

        public bool HasExactlyThreeOfAnyLetter
        {
            get
            {
                return HasOccurrenceOfN(ID, 3);
            }
        }

        private bool HasOccurrenceOfN(string input, int matchingRepeats)
        {
            foreach (char ch in input)
            {
                var occurrence = input.Count(c => c.Equals(ch));
                if (occurrence == matchingRepeats) return true;
            }
            return false;
        }
    }
}
