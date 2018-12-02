using System;

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
                return false;
            }
        }

        public bool HasExactlyThreeOfAnyLetter
        {
            get
            {
                return false;
            }
        }

    }
}
