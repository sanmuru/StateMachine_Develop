using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.ObjectModel
{
    public interface IInputSymbols : IComparer, IEqualityComparer, ICollection
    {
        bool HasPrevious(object? value);

        bool HasNext(object? value);

        object? Previous(object? value);

        object? Next(object? value);

        bool NextTo(object? x, object? y);

        bool Add([AllowNull] object item);

        bool Add(object? from, object? to);

        bool Remove([AllowNull] object item);

        bool Remove(object? from, object? to);

        void ExceptWith(IEnumerable other);

        void IntersectWith(IEnumerable other);

        bool IsProperSubsetOf(IEnumerable other);

        bool IsProperSupersetOf(IEnumerable other);

        bool IsSubsetOf(IEnumerable other);

        bool IsSupersetOf(IEnumerable other);

        bool Overlaps(IEnumerable other);

        void SymmetricExceptWith(IEnumerable other);

        void UnionWith(IEnumerable other);

        bool SetEquals(IEnumerable other);
    }

    public interface IInputSymbols<T> : IInputSymbols, IComparer<T?>, IEqualityComparer<T?>, ISet<T?>
    {
        bool HasPrevious(T? value);

        bool HasNext(T? value);

        T? Previous(T? value);

        T? Next(T? value);

        bool NextTo(T? x, T? y);

        bool Add(T? from, T? to);

        bool Remove(T? from, T? to);
    }
}
