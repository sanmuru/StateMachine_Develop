using SamLu.StateMachine.ObjectModel;

namespace SamLu.RegularExpression.StateMachine.ObjectModel;

/// <summary>
/// 提供了一个针对Unicode字符的输入符号提供程序实现，支持对字符值进行导航和范围操作。
/// </summary>
/// <remarks>
/// 此类允许确定相邻字符、生成包含或排除字符范围，并支持检索下一个或上一个字符等操作。它通常用于需要基于字符的输入符号操作或验证的场景，如解析或输入过滤。
/// </remarks>
public class CharInputSymbolProvider : InputSymbolProvider<char>
{
    /// <inheritdoc/>
    public override bool HasNext(char value) => value != char.MaxValue;

    /// <inheritdoc/>
    public override bool HasPrevious(char value) => value != char.MinValue;

    /// <inheritdoc/>
    public override char Previous(char value) => (char)(value - 1);

    /// <inheritdoc/>
    public override char Next(char value) => (char)(value + 1);

    /// <inheritdoc/>
    public override bool NextTo(char x, char y) => Math.Abs(x - y) == 1;

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<char>> CreateSymbolsInclude(char value)
    {
        return [new(value, value)];
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<char>> CreateSymbolsInclude(char from, char to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        return [new(from, to)];
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<char>> CreateSymbolsExclude(char value)
    {
        if (!this.HasPrevious(value))
            return [new(this.Next(value), char.MaxValue)];
        else if (!this.HasNext(value))
            return [new(char.MinValue, this.Previous(value))];
        else
            return [new(char.MinValue, this.Previous(value)), new(this.Next(value), char.MaxValue)];
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<char>> CreateSymbolsExclude(char from, char to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        InputEntry<char>[] before = this.HasPrevious(from) ? [new(char.MinValue, this.Previous(from))] : [];
        InputEntry<char>[] after = this.HasNext(to) ? [new(this.Next(to), char.MaxValue)] : [];
        return [..before, ..after];
    }
}
