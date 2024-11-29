using System.Collections;

namespace SamLu.StateMachine.ObjectModel;

public interface IInputSymbolProvider : IComparer, IEqualityComparer
{
    bool HasPrevious(object? value);

    bool HasNext(object? value);

    object? Previous(object? value);

    object? Next(object? value);

    bool NextTo(object? x, object? y);

    bool Contains(IEnumerable<InputEntry> inputEntries, object? value);

    bool Contains(IEnumerable<InputEntry> inputEntries, InputEntry value);

    IEnumerable<InputEntry> CreateSymbolsInclude(object? value);

    IEnumerable<InputEntry> CreateSymbolsInclude(object? from, object? to);

    IEnumerable<InputEntry> CreateSymbolsExclude(object? value);

    IEnumerable<InputEntry> CreateSymbolsExclude(object? from, object? to);

    IEnumerable<InputEntry> SplitEntries(IEnumerable<InputEntry> entries);
}

public interface IInputSymbolProvider<T> : IInputSymbolProvider, IComparer<T?>, IEqualityComparer<T?>
{
    bool HasPrevious(T? value);

    bool HasNext(T? value);

    T? Previous(T? value);

    T? Next(T? value);

    bool NextTo(T? x, T? y);

    bool Contains(IEnumerable<InputEntry<T>> entries, T? value);

    bool Contains(IEnumerable<InputEntry<T>> entries, InputEntry<T> value);

    IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? value);

    IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? from, T? to);

    IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? value);

    IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? from, T? to);

    IEnumerable<InputEntry<T>> SplitEntries(IEnumerable<InputEntry<T>> entries);
}
