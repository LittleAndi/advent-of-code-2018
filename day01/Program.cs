using System;
using System.Globalization;
using System.IO;

namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {
            int total = 0;
            using (TextReader tr = new StreamReader("input.txt"))
            {
                string s = "";
                while ((s = tr.ReadLine()) != null)
                {
                    var i = int.Parse(s, CultureInfo.InvariantCulture);
                    total += i;
                }
            }
            System.Console.WriteLine(total);
        }
    }
}
