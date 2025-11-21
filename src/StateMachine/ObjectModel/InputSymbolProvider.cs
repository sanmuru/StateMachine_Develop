using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SamLu.StateMachine.ObjectModel;

/// <summary>
/// 表示输入符号提供者的基类，负责比较、相等判定、相邻关系以及生成/分割输入符号区间等操作。
/// </summary>
public abstract class InputSymbolProvider : IInputSymbolProvider
{
    /// <inheritdoc/>
    public abstract bool HasPrevious(object? value);

    /// <inheritdoc/>
    public abstract bool HasNext(object? value);

    /// <inheritdoc/>
    public abstract object? Previous(object? value);

    /// <inheritdoc/>
    public abstract object? Next(object? value);

    /// <inheritdoc/>
    public abstract bool NextTo(object? x, object? y);

    /// <inheritdoc/>
    public virtual bool Contains(IEnumerable<InputEntry> inputEntries, object? value)
    {
        foreach (var entry in inputEntries)
        {
            var sign1 = Math.Sign(this.Compare(entry.From, value));
            var sign2 = Math.Sign(this.Compare(value, entry.To));
            if (sign1 == 0 || sign2 == 0)
                return true;
            else if (sign1 == sign2)
                return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public bool Contains(IEnumerable<InputEntry> inputEntries, InputEntry value)
    {
        // 计算 value 的区间下界和上界（不修改原始 value）
        (object? v1, object? v2) = value;
        var valueLower = this.Compare(v1, v2) <= 0 ? v1 : v2;
        var valueUpper = this.Compare(v1, v2) <= 0 ? v2 : v1;

        foreach (var entry in inputEntries)
        {
            (object? e1, object? e2) = entry;
            var entryLower = this.Compare(e1, e2) <= 0 ? e1 : e2;
            var entryUpper = this.Compare(e1, e2) <= 0 ? e2 : e1;

            // entry 包含 value 当且仅当 entryLower <= valueLower && valueUpper <= entryUpper
            if (this.Compare(entryLower, valueLower) <= 0 && this.Compare(valueUpper, entryUpper) <= 0)
                return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public IEnumerable<InputEntry> SplitEntries(IEnumerable<InputEntry> entries)
    {
        if (entries is null) return Enumerable.Empty<InputEntry>();

        // 将每个输入条目映射为规范化的 [lower, upper] 对（不修改原始）
        var normalized = entries
            .Select(e =>
            {
                (object? a, object? b) = e;
                var lower = this.Compare(a, b) <= 0 ? a : b;
                var upper = this.Compare(a, b) <= 0 ? b : a;
                return new InputEntry(lower, upper);
            })
            .Where(e => e.From is not null || e.To is not null) // 保留合理条目（可根据需要调整）
            .ToList();

        if (normalized.Count == 0)
            return Enumerable.Empty<InputEntry>();

        // 按 lower 排序
        normalized.Sort((x, y) => this.Compare(x.From, y.From));

        var result = new List<InputEntry>();
        var current = normalized[0];

        for (int i = 1; i < normalized.Count; i++)
        {
            var next = normalized[i];

            // 如果 current 与 next 之间存在严格间隙且不相邻，则将 current 输出，current = next
            if (this.Compare(current.To, next.From) < 0 && !this.NextTo(current.To, next.From))
            {
                result.Add(current);
                current = next;
            }
            else
            {
                // 有重叠或相邻，合并区间：newUpper = max(current.To, next.To)
                var newUpper = this.Compare(current.To, next.To) >= 0 ? current.To : next.To;
                current = new InputEntry(current.From, newUpper);
            }
        }

        result.Add(current);
        return result;
    }

    /// <inheritdoc/>
    public virtual int Compare(object? x, object? y) => Comparer.Default.Compare(x, y);

    /// <inheritdoc/>
    public virtual new bool Equals(object? x, object? y) => object.Equals(x, y);

    /// <inheritdoc/>
    public virtual int GetHashCode(object obj) => obj.GetHashCode();

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry> CreateSymbolsInclude(object? value);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry> CreateSymbolsInclude(object? from, object? to);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry> CreateSymbolsExclude(object? value);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry> CreateSymbolsExclude(object? from, object? to);
}

/// <summary>
/// 泛型版本的输入符号提供者基类，提供对泛型类型的比较、相等、邻接和符号生成支持。
/// </summary>
/// <typeparam name="T">输入符号的泛型类型。</typeparam>
public abstract class InputSymbolProvider<T> : IInputSymbolProvider<T>
{
    /// <inheritdoc/>
    public abstract bool HasPrevious(T? value);
    bool IInputSymbolProvider.HasPrevious(object? value) => this.HasPrevious((T?)value);

    /// <inheritdoc/>
    public abstract bool HasNext(T? value);
    bool IInputSymbolProvider.HasNext(object? value) => this.HasNext((T?)value);

    /// <inheritdoc/>
    public abstract T? Previous(T? value);
    object? IInputSymbolProvider.Previous(object? value) => this.Previous((T?)value);

    /// <inheritdoc/>
    public abstract T? Next(T? value);
    object? IInputSymbolProvider.Next(object? value) => this.Next((T?)value);

    /// <inheritdoc/>
    public abstract bool NextTo(T? x, T? y);
    bool IInputSymbolProvider.NextTo(object? x, object? y) => this.NextTo((T?)x, (T?)y);

    /// <inheritdoc/>
    public virtual bool Contains(IEnumerable<InputEntry<T>> inputEntries, T? value)
    {
        foreach (var entry in inputEntries)
        {
            var sign1 = Math.Sign(this.Compare(entry.From, value));
            var sign2 = Math.Sign(this.Compare(value, entry.To));
            if (sign1 == 0 || sign2 == 0)
                return true;
            else if (sign1 == sign2)
                return true;
        }
        return false;
    }
    bool IInputSymbolProvider.Contains(IEnumerable<InputEntry> inputEntries, object? value) => this.Contains(inputEntries.Select(static e => (InputEntry<T>)e), (T?)value);

    /// <inheritdoc/>
    public bool Contains(IEnumerable<InputEntry<T>> inputEntries, InputEntry<T> value)
    {
        // 计算 value 的区间下界和上界（不修改原始 value）
        (T? v1, T? v2) = value;
        var valueLower = this.Compare(v1, v2) <= 0 ? v1 : v2;
        var valueUpper = this.Compare(v1, v2) <= 0 ? v2 : v1;

        foreach (var entry in inputEntries)
        {
            (T? e1, T? e2) = entry;
            var entryLower = this.Compare(e1, e2) <= 0 ? e1 : e2;
            var entryUpper = this.Compare(e1, e2) <= 0 ? e2 : e1;

            if (this.Compare(entryLower, valueLower) <= 0 && this.Compare(valueUpper, entryUpper) <= 0)
                return true;
        }

        return false;
    }
    bool IInputSymbolProvider.Contains(IEnumerable<InputEntry> inputEntries, InputEntry value) => this.Contains(inputEntries.Select(static e => (InputEntry<T>)e), (InputEntry<T>)value);

    /// <inheritdoc/>
    public IEnumerable<InputEntry<T>> SplitEntries(IEnumerable<InputEntry<T>> entries)
    {
        if (entries is null) return Enumerable.Empty<InputEntry<T>>();

        var normalized = entries
            .Select(e =>
            {
                (T? a, T? b) = e;
                var lower = this.Compare(a, b) <= 0 ? a : b;
                var upper = this.Compare(a, b) <= 0 ? b : a;
                return new InputEntry<T>(lower, upper);
            })
            .ToList();

        if (normalized.Count == 0)
            return Enumerable.Empty<InputEntry<T>>();

        normalized.Sort((x, y) => this.Compare(x.From, y.From));

        var result = new List<InputEntry<T>>();
        var current = normalized[0];

        for (int i = 1; i < normalized.Count; i++)
        {
            var next = normalized[i];

            if (this.Compare(current.To, next.From) < 0 && !this.NextTo(current.To, next.From))
            {
                result.Add(current);
                current = next;
            }
            else
            {
                var newUpper = this.Compare(current.To, next.To) >= 0 ? current.To : next.To;
                current = new InputEntry<T>(current.From, newUpper);
            }
        }

        result.Add(current);
        return result;
    }
    IEnumerable<InputEntry> IInputSymbolProvider.SplitEntries(IEnumerable<InputEntry> entries) => this.SplitEntries(entries.Select(static e => (InputEntry<T>)e)).Select(static e => (InputEntry)e);

    /// <inheritdoc/>
    public virtual int Compare(T? x, T? y) => Comparer<T?>.Default.Compare(x, y);
    int IComparer.Compare(object? x, object? y) => this.Compare((T?)x, (T?)y);

    /// <inheritdoc/>
    public virtual bool Equals(T? x, T? y) => EqualityComparer<T?>.Default.Equals(x, y);
    bool IEqualityComparer.Equals(object? x, object? y) => this.Equals((T?)x, (T?)y);

    /// <inheritdoc/>
    public virtual int GetHashCode(T? obj) => obj is null ? 0 : EqualityComparer<T>.Default.GetHashCode(obj);
    int IEqualityComparer.GetHashCode(object obj) => this.GetHashCode((T)obj);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? value);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsInclude(object? value) => this.CreateSymbolsInclude((T?)value).Select(static e => (InputEntry)e);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? from, T? to);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsInclude(object? from, object? to) => this.CreateSymbolsInclude(((T?)from), ((T?)to)).Select(static e => (InputEntry)e);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? value);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsExclude(object? value) => this.CreateSymbolsExclude((T?)value).Select(static e => (InputEntry)e);

    /// <inheritdoc/>
    public abstract IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? from, T? to);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsExclude(object? from, object? to) => this.CreateSymbolsExclude((T?)from, (T?)to).Select(static e => (InputEntry)e);
}
