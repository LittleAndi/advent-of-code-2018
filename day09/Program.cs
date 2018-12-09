using System;
using System.Collections.Generic;

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
            int[] playerScore = new int[players];
            List<int> marbles = new List<int>();
            marbles.Add(0);
            Console.Write($"{"-",2} [{"--",2}] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{marbles[0],3}");
            Console.ResetColor();
            int lastInsertPos = 0;
            for (int i = 1; i <= totalMarbles; i++)
            {
                if (turn % 1000 == 0) Console.Write($"{turn, 2} [{currentPlayer, 2}] ");

                if (i % 23 == 0)
                {
                    playerScore[currentPlayer - 1] += i;
                    var pickPos = (lastInsertPos - 7);
                    if (pickPos < 0) pickPos += marbles.Count;
                    playerScore[currentPlayer - 1] += marbles[pickPos];
                    marbles.RemoveAt(pickPos);
                    lastInsertPos = pickPos;
                }
                else
                {
                    var pos1 = (lastInsertPos + 1) % marbles.Count + 1;
                    marbles.Insert(pos1, i);
                    lastInsertPos = pos1;
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
                if (turn % 1000 == 0) Console.WriteLine();
            }

            int highestScore = 0;
            int playerWithHighestScore = 0;
            for (int i = 0; i < playerScore.Length; i++)
            {
                if (playerScore[i] > highestScore)
                {
                    highestScore = playerScore[i];
                    playerWithHighestScore = i + 1;
                }
                Console.WriteLine($"[{i,2}] {playerScore[i],4}");
            }
            Console.WriteLine($"Highest score: Player {playerWithHighestScore} wins at {highestScore}");

            // Part 1: 384288

        }
    }
}
