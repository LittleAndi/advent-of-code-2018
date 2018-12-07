using System;
using System.IO;
using System.Linq;

namespace day07
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input_mini.txt")
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => new Step() { From = l.Substring(5, 1)[0], To = l.Substring(36, 1) });

            foreach (var item in lines.OrderBy(l => l.From))
            {
                System.Console.WriteLine($"{item.From}=>{item.To}");
            }
        }
    }

    public class Step
    {
        public Step()
        {
            Done = false;
        }
        public char From { get; set; }
        public char To { get; set; }
        public bool Done { get; set; }
    }
}
