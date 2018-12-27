using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();


            var program = ParseInput(lines);

            // Part 1 = 8797248
            var result = TestRun(program, 2105975, false);


            Console.SetCursorPosition(0, 32);
            Console.WriteLine(result.runs);
        }

        private static (long[] register, int runs) TestRun((int ip, List<PgmLine> pgmLines) program, long reg0, bool print = false)
        {
            var register = new long[6] { reg0, 0, 0, 0, 0, 0 };
            bool running = true;
            List<int> ipList = new List<int>();
            List<long> regHistory = new List<long>();
            int ip = 0;
            while (running)
            {
                var pgmLine = program.pgmLines[ip];
                register[program.ip] = ip;

                ipList.Add(ip);

                if (ip == 28 && !regHistory.Contains(register[2]))
                {
                    //Console.WriteLine($"{Convert.ToString(register[2],2).PadLeft(26, '0')}");
                    regHistory.Add(register[2]);
                }
                if (print)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"ip={ip,2} ");
                    sb.Append($"[{string.Join(", ", register)}]\t");
                    sb.Append($"{pgmLine.Operation} {pgmLine.A} {pgmLine.B} {pgmLine.C}\t");
                    register = Ops.execute(pgmLine.Operation, register, pgmLine.A, pgmLine.B, pgmLine.C);
                    sb.Append($"[{string.Join(", ", register)}]");
                    Console.SetCursorPosition(0, ip);
                    Console.Write(sb.ToString());
                }
                else
                {
                    register = Ops.execute(pgmLine.Operation, register, pgmLine.A, pgmLine.B, pgmLine.C);
                }

                ip = (int)register[program.ip] + 1;

                if (ip >= program.pgmLines.Count) running = false;

            }

            return (register, ipList.Count);
        }

        private static (int ip, List<PgmLine> pgmLines) ParseInput(List<string> lines)
        {
            var pgmLines = new List<PgmLine>();

            var ip = int.Parse(lines[0].Replace("#ip ", ""));
            foreach (var line in lines.Skip(1))
            {
                var s = line.Split(' ');
                var pgmLine = new PgmLine { Operation = s[0], A = int.Parse(s[1]), B = int.Parse(s[2]), C = int.Parse(s[3]) };
                pgmLines.Add(pgmLine);
            }

            return (ip, pgmLines);
        }
    }

    public class PgmLine
    {
        public string Operation { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }

    public static class Ops
    {
        static Dictionary<string, int> opcodes = new Dictionary<string, int>
        {
            { "bori",  0 },
            { "borr",  1 },
            { "addi",  2 },
            { "muli",  3 },
            { "addr",  4 },
            { "bani",  5 },
            { "gtri",  6 },
            { "setr",  7 },
            { "gtrr",  8 },
            { "seti",  9 },
            { "eqir", 10 },
            { "eqrr", 11 },
            { "mulr", 12 },
            { "eqri", 13 },
            { "gtir", 14 },
            { "banr", 15 },
        };

        public static long[] execute(string operation, long[] register, int A, int B, int C)
        {
            return execute(opcodes[operation], register, A, B, C);
        }

        public static long[] execute(int operation, long[] register, int A, int B, int C)
        {
            switch (operation)
            {
                case 0:
                    return bori(register, A, B, C);
                case 1:
                    return borr(register, A, B, C);
                case 2:
                    return addi(register, A, B, C);
                case 3:
                    return muli(register, A, B, C);
                case 4:
                    return addr(register, A, B, C);
                case 5:
                    return bani(register, A, B, C);
                case 6:
                    return gtri(register, A, B, C);
                case 7:
                    return setr(register, A, B, C);
                case 8:
                    return gtrr(register, A, B, C);
                case 9:
                    return seti(register, A, B, C);
                case 10:
                    return eqir(register, A, B, C);
                case 11:
                    return eqrr(register, A, B, C);
                case 12:
                    return mulr(register, A, B, C);
                case 13:
                    return eqri(register, A, B, C);
                case 14:
                    return gtir(register, A, B, C);
                case 15:
                    return banr(register, A, B, C);
            }
            return register.ToArray();
        }

        /// <summary>
        /// addr (add register) stores into register C the result of adding register A and register B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] addr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] + register[B];
            return reg;
        }

        /// <summary>
        /// addi (add immediate) stores into register C the result of adding register A and value B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] addi(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] + B;
            return reg;
        }

        /// <summary>
        /// mulr (multiply register) stores into register C the result of multiplying register A and register B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] mulr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] * register[B];
            return reg;
        }

        /// <summary>
        /// muli (multiply immediate) stores into register C the result of multiplying register A and value B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] muli(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] * B;
            return reg;
        }

        /// <summary>
        /// banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] banr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] & register[B];
            return reg;
        }

        /// <summary>
        /// bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] bani(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] & B;
            return reg;
        }

        /// <summary>
        /// borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] borr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] | register[B];
            return reg;
        }

        /// <summary>
        /// bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] bori(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (uint)register[A] | (uint)B;
            return reg;
        }

        /// <summary>
        /// setr(set register) copies the contents of register A into register C. (Input B is ignored.)
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] setr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A];
            return reg;
        }

        /// <summary>
        /// seti(set immediate) stores value A into register C. (Input B is ignored.)
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] seti(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = A;
            return reg;
        }

        /// <summary>
        /// gtir(greater-than immediate/register) sets register C to 1 if value A is greater than register B.Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] gtir(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (A > register[B] ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B.Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] gtri(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (register[A] > B ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B.Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] gtrr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (register[A] > register[B] ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// eqir (equal immediate/register) sets register C to 1 if value A is equal to register B.Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] eqir(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (A == register[B] ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] eqri(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (register[A] == B ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static long[] eqrr(long[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (register[A] == register[B] ? 1 : 0);
            return reg;
        }

        public static Dictionary<string, int> testsample(long[] register, int A, int B, int C, long[] compare)
        {
            Dictionary<string, long[]> results = new Dictionary<string, long[]>();
            results.Add("addr", addr(register, A, B, C));
            results.Add("addi", addi(register, A, B, C));
            results.Add("mulr", mulr(register, A, B, C));
            results.Add("muli", muli(register, A, B, C));
            results.Add("banr", banr(register, A, B, C));
            results.Add("bani", bani(register, A, B, C));
            results.Add("borr", borr(register, A, B, C));
            results.Add("bori", bori(register, A, B, C));
            results.Add("setr", setr(register, A, B, C));
            results.Add("seti", seti(register, A, B, C));
            results.Add("gtri", gtri(register, A, B, C));
            results.Add("gtir", gtir(register, A, B, C));
            results.Add("gtrr", gtrr(register, A, B, C));
            results.Add("eqir", eqir(register, A, B, C));
            results.Add("eqri", eqri(register, A, B, C));
            results.Add("eqrr", eqrr(register, A, B, C));

            var res = results.Where(r => string.Concat(r.Value).Equals(string.Concat(compare)));
            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach (var item in res)
            {
                ret.Add(item.Key, 1);
            }
            return ret;
        }
    }
}
