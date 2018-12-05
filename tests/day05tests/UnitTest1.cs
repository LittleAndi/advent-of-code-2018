using System;
using Xunit;
using day05;

namespace day05tests
{
    public class UnitTest1
    {
        Polymer polymer = new Polymer { PolymerSequence = "dabAcCaCBAcCcaDA" };

        [Fact]
        public void Test1()
        {
            Assert.Equal("dabCBAcaDA", polymer.React());
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("daDA", polymer.ReactWithoutTroubelingType());
        }
    }
}
