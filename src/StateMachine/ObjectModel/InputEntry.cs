using System.Runtime.InteropServices;

namespace SamLu.StateMachine.ObjectModel;

public readonly struct InputEntry(object? from, object? to)
{
    public readonly object? From = from;
    public readonly object? To = to;

    public void Deconstruct(out object? from, out object? to)
    {
        from = this.From;
        to = this.To;
    }
}

public readonly struct InputEntry<T>(T? from, T? to)
{
    public readonly T? From = from;
    public readonly T? To = to;

    public void Deconstruct(out T? from, out T? to)
    {
        from = this.From;
        to = this.To;
    }

    public static explicit operator InputEntry<T>(InputEntry other) => new((T?)other.From, (T?)other.To);
    public static implicit operator InputEntry(InputEntry<T> other) => new(other.From, other.To);
}

public class InputEntryEqualityComparer : IEqualityComparer<InputEntry>
{
    private readonly IInputSymbolProvider provider;

    public InputEntryEqualityComparer(IInputSymbolProvider provider) => this.provider = provider;

    public bool Equals(InputEntry x, InputEntry y) =>
        (this.provider.Equals(x.From, y.From) && this.provider.Equals(x.To, y.To)) ||
        (this.provider.Equals(x.From, y.To) && this.provider.Equals(x.To, y.From));

    public int GetHashCode(InputEntry obj) =>
        (obj.From is null ? 0 : this.provider.GetHashCode(obj.From)) ^
        (obj.To is null ? 0 : this.provider.GetHashCode(obj.To));
}

public class InputEntryEqualityComparer<T> : IEqualityComparer<InputEntry<T>>
{
    private readonly IInputSymbolProvider<T> provider;

    public InputEntryEqualityComparer(IInputSymbolProvider<T> provider) => this.provider = provider;

    public bool Equals(InputEntry<T> x, InputEntry<T> y) =>
        (this.provider.Equals(x.From, y.From) && this.provider.Equals(x.To, y.To)) ||
        (this.provider.Equals(x.From, y.To) && this.provider.Equals(x.To, y.From));

    public int GetHashCode(InputEntry<T> obj) =>
        (obj.From is null ? 0 : this.provider.GetHashCode(obj.From)) ^
        (obj.To is null ? 0 : this.provider.GetHashCode(obj.To));
}
