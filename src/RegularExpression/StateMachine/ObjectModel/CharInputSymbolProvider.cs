using SamLu.StateMachine.ObjectModel;

namespace SamLu.RegularExpression.StateMachine.ObjectModel;

public class CharInputSymbolProvider : InputSymbolProvider<char>
{
    public override bool HasNext(char value) => value != char.MaxValue;

    public override bool HasPrevious(char value) => value != char.MinValue;

    public override char Previous(char value) => (char)(value - 1);

    public override char Next(char value) => (char)(value + 1);

    public override bool NextTo(char x, char y) => Math.Abs(x - y) == 1;

    public override IEnumerable<InputEntry<char>> CreateSymbolsInclude(char value)
    {
        return [new(value, value)];
    }

    public override IEnumerable<InputEntry<char>> CreateSymbolsInclude(char from, char to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        return [new(from, to)];
    }

    public override IEnumerable<InputEntry<char>> CreateSymbolsExclude(char value)
    {
        if (!this.HasPrevious(value))
            return [new(this.Next(value), char.MaxValue)];
        else if (!this.HasNext(value))
            return [new(char.MinValue, this.Previous(value))];
        else
            return [new(char.MinValue, this.Previous(value)), new(this.Next(value), char.MaxValue)];
    }

    public override IEnumerable<InputEntry<char>> CreateSymbolsExclude(char from, char to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        InputEntry<char>[] fore = this.HasPrevious(from) ? [new(char.MinValue, this.Previous(from))] : [];
        InputEntry<char>[] back = this.HasNext(to) ? [new(this.Next(to), char.MaxValue)] : [];
        return fore.Concat(back);
    }
}
