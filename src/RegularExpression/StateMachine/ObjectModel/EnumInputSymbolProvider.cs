using SamLu.StateMachine.ObjectModel;

namespace SamLu.RegularExpression.StateMachine.ObjectModel;

public class EnumInputSymbolProvider<T> : InputSymbolProvider<T> where T : struct, Enum
{
    protected static readonly IList<T> members;

    static EnumInputSymbolProvider()
    {
        IEnumerable<T> members;
        Type enumType = typeof(T);
        if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0)
            members =
                from values in ((T[])Enum.GetValues(enumType)).GetOptionalCombinations()
                let sum = values.Sum(t => (long)Convert.ChangeType(t, Enum.GetUnderlyingType(enumType)))
                let result = (T)Convert.ChangeType(sum, enumType)
                select result;
        else
            members = ((T[])Enum.GetValues(enumType));
        EnumInputSymbolProvider<T>.members = members.ToList().AsReadOnly();
    }

    public override bool HasNext(T value)
    {
        int index = members.IndexOf(value);
        return index >= 0 && index < members.Count - 1;
    }

    public override bool HasPrevious(T value)
    {
        int index = members.IndexOf(value);
        return index >= 1 && index < members.Count;
    }

    public override T Previous(T value)
    {
        int index = members.IndexOf(value);
        return members[index - 1];
    }

    public override T Next(T value)
    {
        int index = members.IndexOf(value);
        return members[index + 1];
    }

    public override bool NextTo(T x, T y)
    {
        int indexX = members.IndexOf(x);
        int indexY = members.IndexOf(y);

        return indexX >= 0 && indexY >= 0 &&
            Math.Abs(indexX - indexY) == 1;
    }
    public override IEnumerable<InputEntry<T>> CreateSymbolsInclude(T value)
    {
        return [new(value, value)];
    }

    public override IEnumerable<InputEntry<T>> CreateSymbolsInclude(T from, T to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        return [new(from, to)];
    }

    public override IEnumerable<InputEntry<T>> CreateSymbolsExclude(T value)
    {
        if (!this.HasPrevious(value))
            return [new(this.Next(value), members[^1])];
        else if (!this.HasNext(value))
            return [new(members[0], this.Previous(value))];
        else
            return [new(members[0], this.Previous(value)), new(this.Next(value), members[^1])];
    }

    public override IEnumerable<InputEntry<T>> CreateSymbolsExclude(T from, T to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        InputEntry<T>[] fore = this.HasPrevious(from) ? [new(members[0], this.Previous(from))] : [];
        InputEntry<T>[] back = this.HasNext(to) ? [new(this.Next(to), members[^1])] : [];
        return fore.Concat(back);
    }
}
