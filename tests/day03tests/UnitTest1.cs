using System;
using Xunit;
using day03;
using System.Collections.Generic;

namespace day03tests
{
    public class UnitTest1
    {
        List<Fabric> fabrics  = new List<Fabric>();

        public UnitTest1()
        {
            fabrics.Add(new Fabric{ Line = "#1 @ 1,3: 4x4" }); // 0
            fabrics.Add(new Fabric{ Line = "#2 @ 3,1: 4x4" }); // 1
            fabrics.Add(new Fabric{ Line = "#3 @ 5,5: 2x2" }); // 2
            fabrics.Add(new Fabric{ Line = "#806 @ 728,868: 16x18"});   // 3
            fabrics.Add(new Fabric{ Line = "#811 @ 731,816: 12x17"});   // 4
            fabrics.Add(new Fabric{ Line = "#1311 @ 407,579: 27x23"});  // 5
            fabrics.Add(new Fabric{ Line = "#1325 @ 383,562: 28x23"});  // 6
            // #1 @ 100,366: 24x27
            // #720 @ 103,368: 16x20

        }
        [Fact]
        public void Overlap1()
        {
            Assert.True(fabrics[0].Overlap(fabrics[1]));
        }

        [Fact]
        public void NoOverlap4()
        {
            Assert.False(fabrics[0].Overlap(fabrics[2]));
        }

        [Fact]
        public void Overlap1Inverted()
        {
            Assert.True(fabrics[1].Overlap(fabrics[0]));
        }

        [Fact]
        public void NoOverlap1()
        {
            Assert.False(fabrics[1].Overlap(fabrics[2]));
        }

        [Fact]
        public void NoOverlap2()
        {
            Assert.False(fabrics[3].Overlap(fabrics[4]));
        }

        [Fact]
        public void NoOverlap3()
        {
            Assert.True(fabrics[5].Overlap(fabrics[6]));
        }

        [Fact]
        public void OverlappingArea1()
        {
            Assert.Equal(4, fabrics[0].OverlappingArea(fabrics[1]));
        }

        [Fact]
        public void OverlappingArea2()
        {
            Assert.Equal(0, fabrics[0].OverlappingArea(fabrics[2]));
        }

        [Fact]
        public void OverlappingArea3()
        {
            Assert.Equal(0, fabrics[1].OverlappingArea(fabrics[2]));
        }

        [Fact]
        public void OverlappingArea4()
        {
            Assert.Equal(24, fabrics[5].OverlappingArea(fabrics[6]));
        }

        [Fact]
        public void OverlappingCoordiantes4()
        {
            Assert.Equal(24, fabrics[5].OverlappingCoordinates(fabrics[6]).Count);
        }

    }
}
