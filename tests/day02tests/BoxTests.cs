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
        public void HasThreeNonDifferingCharacters()
        {
            var box = new Box { ID = "abcde" };
            Assert.Equal("ace", box.NonDifferingCharacters("axcye"));
        }

        [Fact]
        public void HasFourNonDifferingCharacters()
        {
            var box = new Box { ID = "fghij" };
            Assert.Equal("fgij", box.NonDifferingCharacters("fguij"));
        }

    }
}
