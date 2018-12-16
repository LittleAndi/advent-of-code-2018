using System;
using System.IO;
using System.Linq;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

        }
    }
}
