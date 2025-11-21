using SamLu.StateMachine.ObjectModel;
using System.Numerics;

namespace SamLu.RegularExpression.StateMachine.ObjectModel;

public class EnumInputSymbolProvider<T> : InputSymbolProvider<T> where T : struct, Enum
{
    /// <summary>
    /// 表示枚举类型 <typeparamref name="T"/> 的所有成员的只读列表。
    /// </summary>
    protected static readonly IList<T> members;

    static EnumInputSymbolProvider()
    {
        IEnumerable<T> members;
        Type enumType = typeof(T);
#if NET5_0_OR_GREATER
        members = Enum.GetValues<T>();
#else
        members = (T[])Enum.GetValues(enumType);
#endif
        if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0)
            members =
                from values in members.GetOptionalCombinations()
                let bitwiseOr = values.Select(static v => Convert.ToUInt64(v))
                               .Aggregate(0ul, (a, b) => a | b)
                let result = (T)Enum.ToObject(enumType, bitwiseOr)
                select result;
        EnumInputSymbolProvider<T>.members = members.ToList().AsReadOnly();
    }

    /// <inheritdoc/>
    public override bool HasNext(T value)
    {
        int index = members.IndexOf(value);
        return index >= 0 && index < members.Count - 1;
    }

    /// <inheritdoc/>
    public override bool HasPrevious(T value)
    {
        int index = members.IndexOf(value);
        return index >= 1 && index < members.Count;
    }

    /// <inheritdoc/>
    public override T Previous(T value)
    {
        int index = members.IndexOf(value);
        return members[index - 1];
    }

    /// <inheritdoc/>
    public override T Next(T value)
    {
        int index = members.IndexOf(value);
        return members[index + 1];
    }

    /// <inheritdoc/>
    public override bool NextTo(T x, T y)
    {
        int indexX = members.IndexOf(x);
        int indexY = members.IndexOf(y);

        return indexX >= 0 && indexY >= 0 &&
            Math.Abs(indexX - indexY) == 1;
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<T>> CreateSymbolsInclude(T value)
    {
        return [new(value, value)];
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<T>> CreateSymbolsInclude(T from, T to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        return [new(from, to)];
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<T>> CreateSymbolsExclude(T value)
    {
        if (!this.HasPrevious(value))
            return [new(this.Next(value), members[^1])];
        else if (!this.HasNext(value))
            return [new(members[0], this.Previous(value))];
        else
            return [new(members[0], this.Previous(value)), new(this.Next(value), members[^1])];
    }

    /// <inheritdoc/>
    public override IEnumerable<InputEntry<T>> CreateSymbolsExclude(T from, T to)
    {
        if (this.Compare(from, to) > 0)
            (to, from) = (from, to);

        InputEntry<T>[] before = this.HasPrevious(from) ? [new(members[0], this.Previous(from))] : [];
        InputEntry<T>[] after = this.HasNext(to) ? [new(this.Next(to), members[^1])] : [];
        return [..before, ..after];
    }
}
