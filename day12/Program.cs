using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt")
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

            var initialState = lines[0].Replace("initial state: ", "");
            Console.WriteLine(initialState);

            LinkedList<Pot> pots = new LinkedList<Pot>();
            int potPos = 0;
            foreach (var initialPot in initialState)
            {
                pots.AddLast(new Pot { Plant = initialPot, Position = potPos });
                potPos++;
            }

            FixBeginning(pots);
            FixEnd(pots);

            // Gens
            PrintList(pots);

            long gensToGo = 0;
            for (long gen = 1; gen <= 50000000000; gen++)
            {
                bool equal = true;
                var oldPots = pots;
                pots = CreateNewGeneration(pots, lines, gen);
                if (oldPots.Count == pots.Count)
                {
                    equal = true;
                    LinkedListNode<Pot> testPot = oldPots.First;
                    LinkedListNode<Pot> newPot = pots.First;
                    while (testPot.Next != null)
                    {
                        if (!testPot.Value.Plant.Equals(newPot.Value.Plant))
                        {
                            equal = false;
                            break;
                        }
                        testPot = testPot.Next;
                        newPot = newPot.Next;
                    }
                    if (equal)
                    {
                        gensToGo = 50000000000 - gen;
                        Console.ReadKey();
                        break;
                    }
                }

                PrintList(pots);
            }

            Console.SetCursorPosition(0, 15);
            Console.WriteLine(pots.Where(p => p.Plant.Equals('#')).Sum(p => p.Position + gensToGo));

            // 3100000000293

        }

        private static LinkedList<Pot> CreateNewGeneration(LinkedList<Pot> pots, List<string> lines, long gen)
        {
            Console.SetCursorPosition(0, 3);
            Console.Write($"{gen,12} {pots.Count,10}");
            LinkedList<Pot> nextGenPots = new LinkedList<Pot>();

            LinkedListNode<Pot> pot = pots.First.Next.Next;
            while (pot.Next.Next != null)
            {
                string matchPots = string.Concat(pot.Previous.Previous.Value.Plant, pot.Previous.Value.Plant, pot.Value.Plant, pot.Next.Value.Plant, pot.Next.Next.Value.Plant);
                char newPot = '.';
                for (int i = 1; i < lines.Count; i++)
                {
                    if (lines[i].Substring(0, 5).Equals(matchPots))
                    {
                        newPot = lines[i][9];
                        //break;
                    }
                }
                nextGenPots.AddLast(new Pot { Plant = newPot, Position = pot.Value.Position });
                pot = pot.Next;
            }

            FixBeginning(nextGenPots);

            FixEnd(nextGenPots);

            if (nextGenPots.Count % 1000 == 0) PrintList(nextGenPots);
            return nextGenPots;
        }


        private static void FixBeginning(LinkedList<Pot> nextGenPots)
        {
            for (int i = 1; i <= 5; i++)
            {
                string beginning = string.Concat(nextGenPots.First.Value.Plant, nextGenPots.First.Next.Value.Plant, nextGenPots.First.Next.Next.Value.Plant);
                if (!beginning.Equals("..."))
                {
                    nextGenPots.AddFirst(new Pot { Plant = '.', Position = nextGenPots.First.Value.Position - 1 });
                }
            }
        }

        private static void FixEnd(LinkedList<Pot> nextGenPots)
        {
            for (int i = 1; i <= 5; i++)
            {
                string end = string.Concat(nextGenPots.Last.Value.Plant, nextGenPots.Last.Previous.Value.Plant, nextGenPots.Last.Previous.Previous.Value.Plant);
                if (!end.Equals("..."))
                {
                    nextGenPots.AddLast(new Pot { Plant = '.', Position = nextGenPots.Last.Value.Position + 1 });
                }
            }
        }

        private static void PrintList(LinkedList<Pot> pots)
        {
            Console.SetCursorPosition(0, 10);
            foreach (var item in pots)
            {
                if (item.Position == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write(item.Plant);
                Console.ResetColor();
            }
            Console.Write(pots.Last.Value.Position);
            Console.WriteLine();
        }
    }

    public class Pot
    {
        public char Plant { get; set; }
        public long Position { get; set; }

        public override string ToString()
        {
            return $"{Plant} ({Position})";
        }
    }
}
