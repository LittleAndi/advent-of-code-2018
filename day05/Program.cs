using System;
using System.Linq;

namespace day05
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Polymer
    {
        public string PolymerSequence { get; set; }

        public string React()
        {
            var result = PolymerSequence;
            string previousResult ="";
            while (result != previousResult)
            {
                previousResult = result;
                result = runReactionSequence(result);
            }
            return result;
        }


        private string runReactionSequence(string input)
        {
            for (int i = 65; i <= 90; i++)
            {
                var c = (char)i;
                var replacement1 = c.ToString().ToLower() + c.ToString().ToUpper();
                var replacement2 = c.ToString().ToUpper() + c.ToString().ToLower();
                input = input.Replace(replacement1, "").Replace(replacement2, "");
            }
            return input;
        }
    }
}
