using day18;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace day18tests
{
    public class UnitTest1
    {
        Map map;

        public UnitTest1()
        {
            var lines = File.ReadAllLines("input_mini.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            map = new Map(ProcessInput(lines));
        }

        private static char[,] ProcessInput(List<string> lines)
        {
            char[,] map = new char[lines[0].Length, lines.Count];

            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                foreach (var col in line)
                {
                    map[x, y] = col;
                    x++;
                }
                y++;
            }

            return map;
        }

        [Fact]
        public void Test1()
        {
            //map.CountLumberyards();
        }
    }
}
