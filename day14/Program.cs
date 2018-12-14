using System;
using System.Collections.Generic;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<int> recipies = new LinkedList<int>();
            var elf1 = recipies.AddLast(3);
            var elf2 = recipies.AddLast(7);

            LinkedListNode<int> marker = null;
            int markThis = 909441;

            while (recipies.Count < markThis + 10)
            {
                var sum = elf1.Value + elf2.Value;
                if (sum >= 10)
                {
                    recipies.AddLast(1);
                    if (recipies.Count == markThis) marker = recipies.Last;
                }
                recipies.AddLast(sum % 10);
                if (recipies.Count == markThis) marker = recipies.Last;

                // Move elfs
                var elf1Move = elf1.Value;
                for (int i = 0; i <= elf1Move; i++)
                {
                    elf1 = elf1.Next ?? recipies.First;
                }
                var elf2Move = elf2.Value;
                for (int i = 0; i <=  elf2Move; i++)
                {
                    elf2 = elf2.Next ?? recipies.First;
                }

                //Print(recipies);
                PrintMarker(marker);
            }
        }

        private static void PrintMarker(LinkedListNode<int> marker)
        {
            if (marker == null) return;
            var current = marker.Next;
            for (int i = 0; i < 10; i++)
            {
                if (current == null) break;
                Console.Write(current.Value);
                current = current.Next;
            }
            Console.WriteLine();
        }

        private static void Print(LinkedList<int> recipies)
        {
            Console.WriteLine(string.Concat(recipies));
        }
    }
}
