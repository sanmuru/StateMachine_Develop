using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SamLu.StateMachine.ObjectModel;

public abstract class InputSymbolProvider : IInputSymbolProvider
{
    public abstract bool HasPrevious(object? value);

    public abstract bool HasNext(object? value);

    public abstract object? Previous(object? value);

    public abstract object? Next(object? value);

    public abstract bool NextTo(object? x, object? y);

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

    public bool Contains(IEnumerable<InputEntry> inputEntries, InputEntry value)
    {
#warning 不正确的代码逻辑
        foreach (var entry in inputEntries)
        {
            object? from1, to1, from2, to2;
            if (this.Compare(entry.From, entry.To) <= 0)
                (from1, to1) = entry;
            else
                (to1, from1) = entry;
            if (this.Compare(value.From, value.To) <= 0)
                (from2, to2) = value;
            else
                (to2, from2) = value;

            if (Math.Sign(this.Compare(from1, from2)) <= 0 &&
                Math.Sign(this.Compare(to2, to1)) <= 0)
                return true;
        }
        return false;
    }

    public IEnumerable<InputEntry> SplitEntries(IEnumerable<InputEntry> entries)
    {
#warning 不正确的代码逻辑
        return entries;
    }

    public virtual int Compare(object? x, object? y) => Comparer.Default.Compare(x, y);

    public virtual new bool Equals(object? x, object? y) => object.Equals(x, y);

    public virtual int GetHashCode(object obj) => obj.GetHashCode();

    public abstract IEnumerable<InputEntry> CreateSymbolsInclude(object? value);

    public abstract IEnumerable<InputEntry> CreateSymbolsInclude(object? from, object? to);

    public abstract IEnumerable<InputEntry> CreateSymbolsExclude(object? value);

    public abstract IEnumerable<InputEntry> CreateSymbolsExclude(object? from, object? to);
}

public abstract class InputSymbolProvider<T> : IInputSymbolProvider<T>
{
    public abstract bool HasPrevious(T? value);
    bool IInputSymbolProvider.HasPrevious(object? value) => this.HasPrevious((T?)value);

    public abstract bool HasNext(T? value);
    bool IInputSymbolProvider.HasNext(object? value) => this.HasNext((T?)value);

    public abstract T? Previous(T? value);
    object? IInputSymbolProvider.Previous(object? value) => this.Previous((T?)value);

    public abstract T? Next(T? value);
    object? IInputSymbolProvider.Next(object? value) => this.Next((T?)value);

    public abstract bool NextTo(T? x, T? y);
    bool IInputSymbolProvider.NextTo(object? x, object? y) => this.NextTo((T?)x, (T?)y);

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

    public bool Contains(IEnumerable<InputEntry<T>> inputEntries, InputEntry<T> value)
    {
#warning 不正确的代码逻辑
        foreach (var entry in inputEntries)
        {
            T? from1, to1, from2, to2;
            if (this.Compare(entry.From, entry.To) <= 0)
                (from1, to1) = entry;
            else
                (to1, from1) = entry;
            if (this.Compare(value.From, value.To) <= 0)
                (from2, to2) = value;
            else
                (to2, from2) = value;

            if (Math.Sign(this.Compare(from1, from2)) <= 0 &&
                Math.Sign(this.Compare(to2, to1)) <= 0)
                return true;
        }
        return false;
    }
    bool IInputSymbolProvider.Contains(IEnumerable<InputEntry> inputEntries, InputEntry value) => this.Contains(inputEntries.Select(static e => (InputEntry<T>)e), (InputEntry<T>)value);

    public IEnumerable<InputEntry<T>> SplitEntries(IEnumerable<InputEntry<T>> entries)
    {
#warning 不正确的代码逻辑
        return entries;
    }
    IEnumerable<InputEntry> IInputSymbolProvider.SplitEntries(IEnumerable<InputEntry> entries) => this.SplitEntries(entries.Select(static e => (InputEntry<T>)e)).Select(static e => (InputEntry)e);

    public virtual int Compare(T? x, T? y) => Comparer<T>.Default.Compare(x, y);
    int IComparer.Compare(object? x, object? y) => this.Compare((T?)x, (T?)y);

    public virtual bool Equals(T? x, T? y) => EqualityComparer<T>.Default.Equals(x, y);
    bool IEqualityComparer.Equals(object? x, object? y) => this.Equals((T?)x, (T?)y);

    public virtual int GetHashCode(T obj) => EqualityComparer<T>.Default.GetHashCode(obj);
    int IEqualityComparer.GetHashCode(object obj) => this.GetHashCode((T)obj);

    public abstract IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? value);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsInclude(object? value) => this.CreateSymbolsInclude((T?)value).Select(static e => (InputEntry)e);

    public abstract IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? from, T? to);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsInclude(object? from, object? to) => this.CreateSymbolsInclude(((T?)from), ((T?)to)).Select(static e => (InputEntry)e);

    public abstract IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? value);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsExclude(object? value) => this.CreateSymbolsExclude((T?)value).Select(static e => (InputEntry)e);

    public abstract IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? from, T? to);
    IEnumerable<InputEntry> IInputSymbolProvider.CreateSymbolsExclude(object? from, object? to) => this.CreateSymbolsExclude((T?)from, (T?)to).Select(static e => (InputEntry)e);
}
