namespace SamLu.RegularExpression.UnitTests;

public class CharTests
{
    private RegexProvider<char> provider = new CharRegexProvider();

    private RegexConst<char> Const(char c) => new(this.provider, c);

    [Fact]
    public void ConstTest()
    {
        for (char c = char.MinValue; c <= char.MaxValue; c++)
        {
            Assert.True(Const(c).IsMatch(c));
        }
    }
}
