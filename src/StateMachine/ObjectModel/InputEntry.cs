namespace SamLu.StateMachine.ObjectModel;

/// <summary>
/// 表示输入符号的一个条目，由起点与终点组成，可表示单个符号或区间。
/// </summary>
/// <param name="from">区间或符号的起点（可为 <see langword="null"/>）。</param>
/// <param name="to">区间或符号的终点（可为 <see langword="null"/>）。</param>
public readonly struct InputEntry(object? from, object? to)
{
    /// <summary>
    /// 起点值。
    /// </summary>
    public readonly object? From = from;

    /// <summary>
    /// 终点值。
    /// </summary>
    public readonly object? To = to;

    /// <summary>
    /// 将条目拆解为起点与终点对。
    /// </summary>
    /// <param name="from">输出的起点。</param>
    /// <param name="to">输出的终点。</param>
    public void Deconstruct(out object? from, out object? to)
    {
        from = this.From;
        to = this.To;
    }
}

/// <inheritdoc cref="InputEntry"/>
/// <typeparam name="T">条目中保存的输入类型。</typeparam>
public readonly struct InputEntry<T>(T? from, T? to)
{
    /// <inheritdoc cref="InputEntry.From"/>
    public readonly T? From = from;

    /// <inheritdoc cref="InputEntry.To"/>
    public readonly T? To = to;

    /// <inheritdoc cref="InputEntry.Deconstruct(out object?, out object?)"/>
    public void Deconstruct(out T? from, out T? to)
    {
        from = this.From;
        to = this.To;
    }

    /// <summary>
    /// 将非泛型条目显式转换为泛型条目。
    /// </summary>
    /// <param name="other">非泛型条目。</param>
    public static explicit operator InputEntry<T>(InputEntry other) => new((T?)other.From, (T?)other.To);

    /// <summary>
    /// 将泛型条目隐式转换为非泛型条目。
    /// </summary>
    /// <param name="other">泛型条目。</param>
    public static implicit operator InputEntry(InputEntry<T> other) => new(other.From, other.To);
}

/// <summary>
/// 基于 <see cref="IInputSymbolProvider"/> 的输入符号条目的相等性比较器。
/// </summary>
/// <remarks>
/// 创建新的比较器实例。
/// </remarks>
/// <param name="provider">用于比较输入符号的提供者。</param>
public class InputEntryEqualityComparer(IInputSymbolProvider provider) : IEqualityComparer<InputEntry>
{
    /// <inheritdoc/>
    public bool Equals(InputEntry x, InputEntry y) =>
        (provider.Equals(x.From, y.From) && provider.Equals(x.To, y.To)) ||
        (provider.Equals(x.From, y.To) && provider.Equals(x.To, y.From));

    /// <inheritdoc/>
    public int GetHashCode(InputEntry obj) => HashCode.Combine(
        obj.From is null ? 0 : provider.GetHashCode(obj.From),
        obj.To is null ? 0 : provider.GetHashCode(obj.To));
}

/// <summary>
/// 基于 <see cref="IInputSymbolProvider{T}"/> 的泛型输入符号条目的相等性比较器。
/// </summary>
/// <typeparam name="T">条目中保存的输入类型。</typeparam>
/// <remarks>
/// 创建新的泛型比较器实例。
/// </remarks>
/// <param name="provider">用于比较输入符号的泛型提供者。</param>
public class InputEntryEqualityComparer<T>(IInputSymbolProvider<T> provider) : IEqualityComparer<InputEntry<T>>
{

    /// <inheritdoc/>
    public bool Equals(InputEntry<T> x, InputEntry<T> y) =>
        (provider.Equals(x.From, y.From) && provider.Equals(x.To, y.To)) ||
        (provider.Equals(x.From, y.To) && provider.Equals(x.To, y.From));

    /// <inheritdoc/>
    public int GetHashCode(InputEntry<T> obj) => HashCode.Combine(
        obj.From is null ? 0 : provider.GetHashCode(obj.From),
        obj.To is null ? 0 : provider.GetHashCode(obj.To));
}
