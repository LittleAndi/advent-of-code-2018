using System;
using Xunit;

namespace day02tests
{
    using day02;

    public class BoxTests
    {
        [Fact]
        public void HasNoRepeatingLetters()
        {
            var box = new Box();
            box.ID = "abcdef";
            Assert.False(box.HasExactlyTwoOfAnyLetter);
            Assert.False(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasBothRepeatingLetters()
        {
            var box = new Box();
            box.ID = "bababc";
            Assert.True(box.HasExactlyTwoOfAnyLetter);
            Assert.True(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasTwoOfAnyLetterButNotThree()
        {
            var box = new Box();
            box.ID = "abbcde";
            Assert.True(box.HasExactlyTwoOfAnyLetter);
            Assert.False(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasThreeOfAnyLetterButNotTwo()
        {
            var box = new Box();
            box.ID = "abcccd";
            Assert.False(box.HasExactlyTwoOfAnyLetter);
            Assert.True(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasDoubleTwosButNoThrees()
        {
            var box = new Box();
            box.ID = "aabcdd";
            Assert.True(box.HasExactlyTwoOfAnyLetter);
            Assert.False(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasDoubleThreesButNoTwos()
        {
            var box = new Box();
            box.ID = "ababab";
            Assert.False(box.HasExactlyTwoOfAnyLetter);
            Assert.True(box.HasExactlyThreeOfAnyLetter);
        }
        
    }
}
