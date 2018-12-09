using System;
using System.Collections.Generic;
using System.Linq;

namespace day09
{
    class Program
    {
        static void Main(string[] args)
        {
            // Settings
            int players = 455;
            int totalMarbles = 71223 * 100;

            int turn = 0;
            int currentPlayer = 1;
            long[] playerScore = new long[players];
            LinkedList<int> marbles = new LinkedList<int>();
            var lastInsertPos = marbles.AddFirst(0);
            //Console.Write($"{"-",2} [{"--",2}] ");
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"{marbles[0],3}");
            //Console.ResetColor();
            for (int i = 1; i <= totalMarbles; i++)
            {
                //if (turn % 1000 == 0) Console.Write($"{turn, 2} [{currentPlayer, 2}] ");

                if (i % 23 == 0)
                {
                    playerScore[currentPlayer - 1] += i;
                    var pickPos = lastInsertPos;
                    for (int j = 0; j < 7; j++)
                    {
                        pickPos = pickPos.Previous ?? marbles.Last;
                    }
                    playerScore[currentPlayer - 1] += pickPos.Value;
                    lastInsertPos = pickPos.Next;
                    marbles.Remove(pickPos);
                }
                else
                {
                    lastInsertPos = lastInsertPos.Next ?? marbles.First;
                    lastInsertPos = marbles.AddAfter(lastInsertPos, i);
                }

                //// Print
                //for (int j = 0; j < marbles.Count; j++)
                //{
                //    if (j == (lastInsertPos))
                //    {
                //        Console.ForegroundColor = ConsoleColor.Red;
                //        Console.Write($"{marbles[j],3}");
                //    }
                //    else
                //    {
                //        Console.Write($"{marbles[j],3}");
                //    }
                //    Console.ResetColor();
                //}

                // Prep for next turn
                currentPlayer = (++turn % players) + 1;
                //if (turn % 1000 == 0) Console.Write('.');
            }
            Console.WriteLine();

            long highestScore = 0;
            int playerWithHighestScore = 0;
            for (int i = 0; i < playerScore.Length; i++)
            {
                if (playerScore[i] > highestScore)
                {
                    highestScore = playerScore[i];
                    playerWithHighestScore = i + 1;
                }
                Console.WriteLine($"[{i+1,2}] {playerScore[i],4}");
            }
            Console.WriteLine($"Highest score: Player {playerWithHighestScore} wins at {highestScore}");

            // Part 1: 384288
            // Part 2: 3189426841
        }
    }
}
