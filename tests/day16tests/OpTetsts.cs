using System;
using Xunit;
using day16;

namespace day16tests
{
    public class OpTetsts
    {
        int[] register;

        public OpTetsts()
        {
            register = new int[] { 3, 2, 1, 1 };
        }
        [Fact]
        public void Test_addr()
        {
            var result = Ops.addr(register, 2, 1, 2);
            Assert.Equal(new int[] { 3, 2, 3, 1 }, result);
        }

        [Fact]
        public void Test_addi()
        {
            var result = Ops.addi(new int[] { 3, 2, 1, 1 }, 2, 1, 2);
            Assert.Equal(new int[] { 3, 2, 2, 1 }, result);
        }


    }
}
