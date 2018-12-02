using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day02
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Box> boxes = new List<Box>();
            using (TextReader tr = new StreamReader("input.txt"))
            {
                string s = "";
                while ((s = tr.ReadLine()) != null)
                {
                    boxes.Add(new Box { ID = s });
                }
            }

            var twos = boxes.Count(box => box.HasExactlyTwoOfAnyLetter);
            var threes = boxes.Count(box => box.HasExactlyThreeOfAnyLetter);

            System.Console.WriteLine($"Part 1: {twos * threes}");

            foreach (var box in boxes)
            {
                // All other boxes that's not itself
                var allOtherBoxes = boxes.Where(b => !b.Equals(box));

                foreach (var otherBox in allOtherBoxes)
                {
                    var remainder = box.NonDifferingCharacters(otherBox.ID);
                    if (remainder.Length.Equals(box.ID.Length-1))
                    {
                        System.Console.WriteLine($"Part 2: {box.ID} - {remainder}");
                    }
                }
            }
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

        public string NonDifferingCharacters(string input)
        {
            if (ID.Length != input.Length) return null;
            StringBuilder sb = new StringBuilder();
            var idChars = ID.ToCharArray();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Equals(idChars[i])) { sb.Append(input[i]); };
            }

            return sb.ToString();
        }

        [Obsolete]
        public char HasExactlyOneDifferingCharacter(string input)
        {
            if (ID.Length != input.Length) return ' ';
            var idChars = ID.ToCharArray();
            int differingLetters = 0;
            char lastDifferingLetter = ' ';
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != idChars[i]) { differingLetters++; lastDifferingLetter = idChars[i]; }
            }
            return differingLetters == 1 ? lastDifferingLetter : ' ';
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
