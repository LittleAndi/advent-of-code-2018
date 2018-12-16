using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            Ops.testsample(new int[] { 3, 2, 1, 1 }, 2, 1, 2, new int[] { 3, 2, 2, 1 });
        }
    }

    public static class Ops
    {
        public static int[] execute(int operation, int[] register, int A, int B, int C)
        {
            switch (operation)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
            }
            return register;
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
            reg[C] = (register[A] > B ? 1: 0);
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

        public static int testsample(int[] register, int A, int B, int C, int[] compare)
        {
            int equals = 0;

            List<int[]> results = new List<int[]>();
            results.Add(addr(register, A, B, C));
            results.Add(addi(register, A, B, C));
            results.Add(mulr(register, A, B, C));
            results.Add(muli(register, A, B, C));
            results.Add(banr(register, A, B, C));
            results.Add(bani(register, A, B, C));
            results.Add(borr(register, A, B, C));
            results.Add(bori(register, A, B, C));
            results.Add(setr(register, A, B, C));
            results.Add(seti(register, A, B, C));
            results.Add(gtri(register, A, B, C));
            results.Add(gtir(register, A, B, C));
            results.Add(gtrr(register, A, B, C));
            results.Add(eqir(register, A, B, C));
            results.Add(eqri(register, A, B, C));
            results.Add(eqrr(register, A, B, C));


            var res = results.Where(r => string.Concat(r).Equals(string.Concat(compare))).Count();
            return res;
        }
    }
}
