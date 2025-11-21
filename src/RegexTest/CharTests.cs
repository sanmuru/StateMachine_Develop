namespace SamLu.RegularExpression.UnitTests;

public class CharTests
{
    private RegexProvider<char> provider = new CharRegexProvider();

    private RegexConst<char> Const(char c) => new(this.provider, c);

    private RegexRange<char> Range(char start, char end) => new(this.provider, start, end);

    [Fact]
    public void ConstTest_ASCII()
    {
        for (int i = char.MinValue; i <= 0x7F; i++)
        {
            var c = (char)i;
            Assert.True(Const(c).IsMatch(c));
        }
    }

    [Fact]
    public void ConstTest_UNICODE()
    {
        for (int i = char.MinValue; i <= char.MaxValue; i++)
        {
            var c = (char)i;
            Assert.True(Const(c).IsMatch(c));
        }
    }

    [Fact]
    public void RangeTest_ASCII()
    {
        var range = Range((char)0x00, (char)0x7F);
        for (int i = char.MinValue; i <= char.MaxValue; i++)
        {
            char c = (char)i;
            if (i is >= 0x00 and <= 0x7F)
                Assert.True(range.IsMatch(c));
            else
                Assert.False(range.IsMatch(c));
        }
    }

    [Fact]
    public void RangeTest_UNICODE()
    {
        var range = Range(char.MinValue, char.MaxValue);
        for (int i = char.MinValue; i <= char.MaxValue; i++)
        {
            char c = (char)i;
            Assert.True(range.IsMatch(c));
        }
    }
}
