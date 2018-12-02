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
            var box = new Box { ID = "abcdef" };
            Assert.False(box.HasExactlyTwoOfAnyLetter);
            Assert.False(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasBothRepeatingLetters()
        {
            var box = new Box { ID = "bababc" };
            Assert.True(box.HasExactlyTwoOfAnyLetter);
            Assert.True(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasTwoOfAnyLetterButNotThree()
        {
            var box = new Box { ID = "abbcde" };
            Assert.True(box.HasExactlyTwoOfAnyLetter);
            Assert.False(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasThreeOfAnyLetterButNotTwo()
        {
            var box = new Box { ID = "abcccd" };
            Assert.False(box.HasExactlyTwoOfAnyLetter);
            Assert.True(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasDoubleTwosButNoThrees()
        {
            var box = new Box { ID = "aabcdd" };
            Assert.True(box.HasExactlyTwoOfAnyLetter);
            Assert.False(box.HasExactlyThreeOfAnyLetter);
        }

        [Fact]
        public void HasDoubleThreesButNoTwos()
        {
            var box = new Box { ID = "ababab" };
            Assert.False(box.HasExactlyTwoOfAnyLetter);
            Assert.True(box.HasExactlyThreeOfAnyLetter);
        }
        
        [Fact]
        public void HasNotExactlyOneDifferingCharacter()
        {
            var box = new Box { ID = "abcde" };
            Assert.False(box.HasExactlyOneDifferingCharacter("axcye"));
        }

        [Fact]
        public void HasExactlyOneDifferingCharacter()
        {
            var box = new Box { ID = "fghij" };
            Assert.True(box.HasExactlyOneDifferingCharacter("fguij"));
        }

    }
}
