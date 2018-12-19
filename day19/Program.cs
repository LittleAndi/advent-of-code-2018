using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day19
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            LinkedList<string> testInputs = new LinkedList<string>(lines);

            List<Test> pgm = new List<Test>();
            List<Test> tests = ParseInput(testInputs, out pgm);
            Dictionary<string, int[]> mapOps = new Dictionary<string, int[]>();

            int moreThanOrEqualToThree = 0;
            Dictionary<int, string> opCodes = new Dictionary<int, string>();
            foreach (var test in tests)
            {
                var result = Ops.testsample(test.Before, test.A, test.B, test.C, test.After);
                if (result.Count >= 3) moreThanOrEqualToThree++;
                if (result.Count == 1)
                {
                    if (!opCodes.ContainsKey(test.Operation))
                    {
                        opCodes.Add(test.Operation, result.Keys.First());
                        Console.WriteLine($"Opcode {test.Operation} is {result.Keys.First()}");
                    }
                }
                foreach (var item in result)
                {
                    if (!mapOps.ContainsKey(item.Key))
                    {
                        mapOps.Add(item.Key, new int[16]);
                    }
                    mapOps[item.Key][test.Operation] = 1;
                }
            }

            Console.WriteLine(moreThanOrEqualToThree);

            PrintOps(mapOps);

            while (mapOps.Count > 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    var s = mapOps.Sum(m => m.Value[i]);
                    if (s == 1)
                    {
                        var o = mapOps.Where(m => m.Value[i] == 1).First();
                        Console.WriteLine($"{o.Key}, {i}");
                        mapOps.Remove(o.Key);
                    }
                }
            }

            int[] register = new int[] { 0, 0, 0, 0 };
            foreach (var item in pgm)
            {
                register = Ops.execute(item.Operation, register, item.A, item.B, item.C);
                Console.WriteLine($"{item.Operation,2} {item.A} {item.B} {item.C} => {register[0],4}{register[1],4}{register[2],4}{register[3],4}");
            }

            Console.WriteLine(register[0]);

            //Ops.testsample(new int[] { 3, 2, 1, 1 }, 2, 1, 2, new int[] { 3, 2, 2, 1 });
        }

        private static void PrintOps(Dictionary<string, int[]> mapOps)
        {
            foreach (var item in mapOps.OrderBy(m => m.Key))
            {
                StringBuilder output = new StringBuilder();
                for (int i = 0; i < 16; i++)
                {
                    output.Append($"{item.Value[i],3}");
                }
                Console.WriteLine($"{item.Key}\t{output.ToString()}");
            }
        }

        private static List<Test> ParseInput(LinkedList<string> testInputs, out List<Test> pgm)
        {
            pgm = new List<Test>();
            List<Test> tests = new List<Test>();
            var testInput = testInputs.First;

            Test test;
            while (testInput != null)
            {
                if (testInput.Value.StartsWith("Before: "))
                {
                    test = new Test();
                    test.Before = new int[] { int.Parse(testInput.Value.Substring(9, 1)), int.Parse(testInput.Value.Substring(12, 1)), int.Parse(testInput.Value.Substring(15, 1)), int.Parse(testInput.Value.Substring(18, 1)) };

                    testInput = testInput.Next;
                    var command = testInput.Value.Split(' ');
                    test.Operation = int.Parse(command[0]);
                    test.A = int.Parse(command[1]);
                    test.B = int.Parse(command[2]);
                    test.C = int.Parse(command[3]);

                    testInput = testInput.Next;
                    test.After = new int[] { int.Parse(testInput.Value.Substring(9, 1)), int.Parse(testInput.Value.Substring(12, 1)), int.Parse(testInput.Value.Substring(15, 1)), int.Parse(testInput.Value.Substring(18, 1)) };
                    tests.Add(test);

                    testInput = testInput.Next;
                }
                else
                {
                    var command = testInput.Value.Split(' ');
                    var pgmOp = new Test();
                    pgmOp.Operation = int.Parse(command[0]);
                    pgmOp.A = int.Parse(command[1]);
                    pgmOp.B = int.Parse(command[2]);
                    pgmOp.C = int.Parse(command[3]);
                    pgm.Add(pgmOp);
                    testInput = testInput.Next;
                }
            }

            //Console.WriteLine(tests.Count);
            return tests;
        }
    }

    public static class Ops
    {
        public static int[] execute(int operation, int[] register, int A, int B, int C)
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
        public static int[] addr(int[] register, int A, int B, int C)
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
        public static int[] addi(int[] register, int A, int B, int C)
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
        public static int[] mulr(int[] register, int A, int B, int C)
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
        public static int[] muli(int[] register, int A, int B, int C)
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
        public static int[] banr(int[] register, int A, int B, int C)
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
        public static int[] bani(int[] register, int A, int B, int C)
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
        public static int[] borr(int[] register, int A, int B, int C)
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
        public static int[] bori(int[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = register[A] | B;
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
        public static int[] setr(int[] register, int A, int B, int C)
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
        public static int[] seti(int[] register, int A, int B, int C)
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
        public static int[] gtir(int[] register, int A, int B, int C)
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
        public static int[] gtri(int[] register, int A, int B, int C)
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
        public static int[] gtrr(int[] register, int A, int B, int C)
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
        public static int[] eqir(int[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (A == register[B] ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// eqri (equal register/immediate) sets register C to 1 if register A is equal to value B.Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static int[] eqri(int[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (register[A] == B ? 1 : 0);
            return reg;
        }

        /// <summary>
        /// eqrr (equal register/register) sets register C to 1 if register A is equal to register B.Otherwise, register C is set to 0.
        /// </summary>
        /// <param name="register"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static int[] eqrr(int[] register, int A, int B, int C)
        {
            var reg = register.ToArray();
            reg[C] = (register[A] == register[B] ? 1 : 0);
            return reg;
        }

        public static Dictionary<string, int> testsample(int[] register, int A, int B, int C, int[] compare)
        {
            Dictionary<string, int[]> results = new Dictionary<string, int[]>();
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

    public class Test
    {
        public int[] Before { get; set; }
        public int[] After { get; set; }
        public int Operation { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }
}
