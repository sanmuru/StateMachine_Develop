using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.ObjectModel
{
    public abstract class InputSymbols<T> : IInputSymbols<T?>
    {
        protected LinkedList<T?> edges = new();

        public virtual int Count => this.Count();
        public virtual bool IsSynchronized => false;
        public virtual object SyncRoot => this;
        public virtual bool IsReadOnly => false;

        public abstract bool HasPrevious(T? value);
        public virtual bool HasPrevious(object? value) => this.HasPrevious((T?)value);

        public abstract bool HasNext(T? value);
        public virtual bool HasNext(object? value) => this.HasNext((T?)value);

        public abstract T? Previous(T? value);
        public virtual object? Previous(object? value) => this.Previous((T?)value);

        public abstract T? Next(T? value);
        public virtual object? Next(object? value) => this.Next((T?)value);

        public abstract bool NextTo(T? x, T? y);
        public virtual bool NextTo(object? x, object? y) => this.NextTo((T?)x, (T?)y);

        public virtual void ExceptWith(IEnumerable<T?> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            foreach (var item in other)
                this.Remove(item);
        }
        public virtual void ExceptWith(IEnumerable other)
        {
            switch (other)
            {
                case null: throw new ArgumentNullException(nameof(other));
                case IEnumerable<T?>:
                    this.ExceptWith((IEnumerable<T?>)other);
                    break;
                default:
                    this.ExceptWith(other.Cast<T?>());
                    break;
            }
        }

        public virtual void IntersectWith(IEnumerable<T?> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            var edges = new T[this.edges.Count];
            this.edges.CopyTo(edges, 0);
            foreach (var item in this.EnumerateInternal(edges))
                this.Remove(item);
        }
        public virtual void IntersectWith(IEnumerable other)
        {
            switch (other)
            {
                case null: throw new ArgumentNullException(nameof(other));
                case IEnumerable<T?>:
                    this.IntersectWith((IEnumerable<T?>)other);
                    break;
                default:
                    this.IntersectWith(other.Cast<T?>());
                    break;
            }
        }

        public virtual void SymmetricExceptWith(IEnumerable<T?> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            this.UnionWith(other);

            var edges = new T[this.edges.Count];
            this.edges.CopyTo(edges, 0);
            this.ExceptWith(this.EnumerateInternal(edges).Intersect(other, this));
        }
        public virtual void SymmetricExceptWith(IEnumerable other)
        {
            switch (other)
            {
                case null: throw new ArgumentNullException(nameof(other));
                case IEnumerable<T?>:
                    this.SymmetricExceptWith((IEnumerable<T?>)other);
                    break;
                default:
                    this.SymmetricExceptWith(other.Cast<T?>());
                    break;
            }
        }

        public virtual void UnionWith(IEnumerable<T?> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            foreach (var item in other)
                this.Add(item);
        }
        public virtual void UnionWith(IEnumerable other)
        {
            switch (other)
            {
                case null: throw new ArgumentNullException(nameof(other));
                case IEnumerable<T?>:
                    this.UnionWith((IEnumerable<T?>)other);
                    break;
                default:
                    this.UnionWith(other.Cast<T?>());
                    break;
            }
        }

        public virtual bool IsProperSubsetOf(IEnumerable<T?> other) => this.Count < other.Count() && this.IsSubsetOf(other);
        public virtual bool IsProperSubsetOf(IEnumerable other) =>
            other switch
            {
                null => throw new ArgumentNullException(nameof(other)),
                IEnumerable<T?> => this.IsProperSubsetOf((IEnumerable<T?>)other),
                _ => this.IsProperSubsetOf(other.Cast<T?>())
            };

        public virtual bool IsProperSupersetOf(IEnumerable<T?> other) => this.Count > other.Count() && this.IsSupersetOf(other);
        public virtual bool IsProperSupersetOf(IEnumerable other) =>
            other switch
            {
                null => throw new ArgumentNullException(nameof(other)),
                IEnumerable<T?> => this.IsProperSupersetOf((IEnumerable<T?>)other),
                _ => this.IsProperSupersetOf(other.Cast<T?>())
            };

        public virtual bool IsSubsetOf(IEnumerable<T?> other) => this.All(item => (other ?? throw new ArgumentNullException(nameof(other))).Contains(item));
        public virtual bool IsSubsetOf(IEnumerable other) =>
            other switch
            {
                null => throw new ArgumentNullException(nameof(other)),
                IEnumerable<T?> => this.IsSubsetOf((IEnumerable<T?>)other),
                _ => this.IsSubsetOf(other.Cast<T?>())
            };

        public virtual bool IsSupersetOf(IEnumerable<T?> other) => (other ?? throw new ArgumentNullException(nameof(other))).All(item => this.Contains(item));
        public virtual bool IsSupersetOf(IEnumerable other) =>
            other switch
            {
                null => throw new ArgumentNullException(nameof(other)),
                IEnumerable<T?> => this.IsSupersetOf((IEnumerable<T?>)other),
                _ => this.IsSupersetOf(other.Cast<T?>())
            };

        public virtual bool Overlaps(IEnumerable<T?> other) => this.Intersect(other ?? throw new ArgumentNullException(nameof(other)), this).Any();
        public virtual bool Overlaps(IEnumerable other) =>
            other switch
            {
                null => throw new ArgumentNullException(nameof(other)),
                IEnumerable<T?> => this.Overlaps((IEnumerable<T?>)other),
                _ => this.Overlaps(other.Cast<T?>())
            };

        public virtual bool SetEquals(IEnumerable<T?> other) => this.SequenceEqual(other ?? throw new ArgumentNullException(nameof(other)), this);

        public virtual bool SetEquals(IEnumerable other) =>
            other switch
            {
                IInputSymbols => this.SetEquals((IInputSymbols)other),
                IEnumerable<T?> => this.SetEquals((IEnumerable<T?>)other),
                _ => this.SetEquals(other.Cast<T?>())
            };


        public virtual int Compare(T? x, T? y) => Comparer<T?>.Default.Compare(x, y);
        public virtual int Compare(object? x, object? y) => this.Compare((T?)x, (T?)y);

        public virtual bool Equals(T? x, T? y) => EqualityComparer<T?>.Default.Equals(x, y);
        public virtual new bool Equals(object? x, object? y) => this.Equals((T?)x, (T?)y);

        public virtual int GetHashCode(T? obj) => obj is null ? 0 : EqualityComparer<T?>.Default.GetHashCode(obj);
        public virtual int GetHashCode([AllowNull] object obj) => this.GetHashCode((T?)obj);

        protected virtual IEnumerable<T?> EnumerateInternal(IEnumerable<T?> edges)
        {
            var ranges = edges.Chunk(2);
            foreach (var range in ranges)
            {
                for (var item = range[0]; this.Compare(item, range[^0]) <= 0; item = this.Next(item))
                    yield return item;
            }
        }
        IEnumerator<T?> IEnumerable<T?>.GetEnumerator() => this.EnumerateInternal(this.edges).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.EnumerateInternal(this.edges).GetEnumerator();

        public virtual bool Add(T? item)
        {
            var (exists, from, to) = this.Find(item);
            if (exists) return false; // 已存在，则不添加。

#pragma warning disable CS8601, CS8602, CS8604 // 引用类型赋值可能为 null。
            if (from is not null && to is null)
            {
                if (this.NextTo(item, from))
                    from.Value = item;
                else
                {
                    this.edges.AddAfter(from, item);
                    this.edges.AddAfter(from, item);
                }
            }
            else if (from is null && to is not null)
            {
                if (this.NextTo(item, to))
                    to.Value = item;
                else
                {
                    this.edges.AddBefore(to, item);
                    this.edges.AddBefore(to, item);
                }
            }
            else if (from is null && to is null)
            {
                this.edges.AddFirst(item);
                this.edges.AddLast(item);
            }
            else
            {
                if (this.NextTo(item, from))
                    from.Value = item;
                else if (this.NextTo(item, to))
                    to.Value = item;
                else
                {
                    this.edges.AddAfter(from, item);
                    this.edges.AddBefore(to, item);
                }
            }
#pragma warning restore CS8601, CS8602, CS8604 // 引用类型赋值可能为 null。
            return true;
        }
        public virtual bool Add([AllowNull] object item) => this.Add((T?)item);
        void ICollection<T?>.Add(T? item) => this.Add(item);
        public virtual bool Add(T? from, T? to)
        {
            if (this.Compare(from, to) > 0) (from, to) = (to, from); // 调整为 (最小值, 最大值) 形式。

            var (fromExists, fromFrom, fromTo) = Find(from);
            if (!fromExists && fromTo is null) // 若查询到的下界的位置不存在且在末尾。
            {
                this.edges.AddLast(from);
                this.edges.AddLast(to);
                return true;
            }
            var (toExists, toFrom, toTo) = Find(to, startNode: fromFrom);
            if (!toExists && toFrom is null) // 若查询到的上界的位置不存在且在开头。
            {
                this.edges.AddFirst(to);
                this.edges.AddLast(from);
                return true;
            }

            // 此时， [from, to] 范围必定不会——在末尾或开头且不与输入符号集有共同元素。
            LinkedListNode<T?> deleteFrom, deleteTo;
            bool changed = false;
            if (fromExists) // 下界位置位于已有范围内，则不需要插入边界，从位置所在范围的上界开始删除。
            {
                deleteFrom = fromTo!;
                changed |= false;
            }
            else if (fromFrom is null) // 下届位置位于开头，则需要插入边界，并从第一个范围的下界开始删除。
            {
                deleteFrom = this.edges.First!;
                this.edges.AddFirst(from);
                changed |= true;
            }
            else if (this.NextTo(from, fromFrom.Value)) // 下界位置紧邻前方范围的上界，则不需要插入边界，从前方范围的上界开始删除。
            {
                deleteFrom = fromFrom;
                changed |= false;
            }
            else if (this.NextTo(from, fromTo!.Value)) // 下界位置紧邻后方范围的下界，则需要修改后方范围的下界，从后方范围的上界开始删除。
            {
                deleteFrom = fromTo!.Next!;
                fromTo!.Value = from;
                changed |= true;
            }
            else // 下界位置不紧邻前、后方范围的边界，则需要插入边界，并从后方范围的下界开始删除。
            {
                deleteFrom = fromTo;
                this.edges.AddAfter(fromTo, from);
                changed |= true;
            }
            if (toExists) // 上界位置位于已有范围内，则不需要插入边界，从位置所在范围的下界开始删除。
            {
                deleteTo = toFrom!;
                changed |= false;
            }
            else if (toTo is null) // 上界位置位于结尾，则需要插入边界，并从最后一个范围的上界开始删除。
            {
                deleteTo = this.edges.Last!;
                this.edges.AddLast(to);
                changed |= true;
            }
            else if (this.NextTo(to, toTo.Value)) // 上界位置紧邻后方范围的下界，则不需要插入边界，从后方范围的下界开始删除。
            {
                deleteTo = toTo;
                changed |= false;
            }
            else if (this.NextTo(to, toFrom!.Value)) // 上界位置紧邻前方范围的上界，则需要修改前方范围的上界，从前方范围的下界开始删除。
            {
                deleteTo = toFrom!.Next!;
                toFrom!.Value = to;
                changed |= true;
            }
            else // 上界位置不紧邻前、后方范围的边界，则需要插入边界，并从前方范围的上界开始删除。
            {
                deleteTo = toFrom;
                this.edges.AddBefore(toFrom, to);
                changed |= true;
            }

            // 删除失效的边界。
            if (this.Compare(deleteFrom.Value, deleteTo.Value) > 0) return changed; // 没有失效的边界。
            for (var node = deleteFrom; node != deleteTo; node = node.Next!)
                this.edges.Remove(node);
            this.edges.Remove(deleteTo);
            return true;
        }
        public virtual bool Add(object? from, object? to) => this.Add((T?)from, (T?)to);

        public virtual void Clear() => this.edges.Clear();

        public virtual bool Contains(T? item)
        {
            var (exists, _, _) = this.Find(item);
            return exists;
        }

        public virtual void CopyTo(T?[] array, int arrayIndex) => this.CopyTo((Array)array, arrayIndex);
        public virtual void CopyTo(Array array, int index)
        {
            ArgumentNullException.ThrowIfNull(array);
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            int i = index;
            foreach (var item in this)
            {
                if (i >= array.Length) break;
                ((object?[])array)[i++] = item;
            }
        }

        public virtual bool Remove(T? item)
        {
            var (exists, from, to) = this.Find(item);
            if (!exists) return false; // 已存在，则不移除。

#pragma warning disable CS8601, CS8602, CS8604 // 引用类型赋值可能为 null。
            if (this.Equals(item, from.Value))
                from.Value = this.Next(item);
            else if (this.Equals(item, to.Value))
                to.Value = this.Previous(item);
            else
            {
                this.edges.AddAfter(from, this.Previous(item));
                this.edges.AddBefore(to, this.Next(item));
            }
#pragma warning restore CS8604, CS8602, CS8604 // 引用类型参数可能为 null。
            return true;
        }
        public virtual bool Remove([AllowNull] object item) => this.Remove((T?)item);
        public virtual bool Remove(T? from, T? to)
        {
            if (this.Compare(from, to) > 0) (from, to) = (to, from); // 调整为 (最小值, 最大值) 形式。

            if (this.edges.Count == 0) return false;

            var (fromExists, fromFrom, fromTo) = Find(from);
            if (!fromExists && fromTo is null) // 若查询到的下界的位置不存在且在末尾。
                return false;
            var (toExists, toFrom, toTo) = Find(to, startNode: fromFrom);
            if (!toExists && toFrom is null) // 若查询到的上界的位置不存在且在开头。
                return false;

            // 此时， [from, to] 范围必定不会——在末尾或开头且不与输入符号集有共同元素。
            LinkedListNode<T?> deleteFrom, deleteTo;
            bool changed = false;
            if (fromExists) // 下界位置位于已有范围内，则需要插入边界，从位置所在范围的上界开始删除。
            {
                deleteFrom = fromTo!;
                this.edges.AddAfter(fromTo!, this.Previous(from));
                changed |= true;
            }
            else // 下届位置位于后方范围之前，则不需要插入边界，并从后方范围的下界开始删除。
            {
                deleteFrom = fromTo!;
                changed |= false;
            }
            if (toExists) // 上界位置位于已有范围内，则需要插入边界，到位置所在范围的下界结束删除。
            {
                deleteTo = toFrom!;
                this.edges.AddBefore(toFrom!, this.Next(to));
                changed |= true;
            }
            else // 上界位置位于前方范围之后，则不需要插入边界，并到前方范围的上界结束删除。
            {
                deleteTo = toFrom!;
                changed |= false;
            }

            // 删除失效的边界。
            if (this.Compare(deleteFrom.Value, deleteTo.Value) > 0) return changed; // 没有失效的边界。
            for (var node = deleteFrom; node != deleteTo; node = node.Next!)
                this.edges.Remove(node);
            this.edges.Remove(deleteTo);
            return true;
        }
        public virtual bool Remove(object? from, object? to) => this.Remove((T?)from, (T?)to);

        protected (bool exists, LinkedListNode<T?>? from, LinkedListNode<T?>? to) Find(T? item, LinkedListNode<T?>? startNode = null, LinkedListNode<T?>? endNode = null)
        {
            startNode ??= this.edges.First;
            endNode ??= this.edges.Last;

            if (this.edges.Count == 0) return (false, null, null);
            else if (this.Compare(item, startNode) < 0) return (false, null, startNode);
            else if (this.Compare(item, endNode) > 0) return (false, endNode, null);

            LinkedListNode<T?>? pos = null; // 一个定位，指向上一次检测的范围的上界。
            do
            {
                // 成对获取范围的上下界。
                var (from, to) = pos switch
                {
                    null => (startNode!, startNode!.Next!),
                    _ => (pos.Next!, pos.Next!.Next!)
                };
                if (this.Compare(item, from.Value) < 0) // 超出当前范围的下界。
                    return (false, pos, from);
                else if (this.Compare(item, to.Value) <= 0) // 在当前范围内。
                    return (true, from, to);
                else // 移动定位到当前范围上界，以进行下一个范围的检查。
                    pos = to;
            } while (pos is not null && endNode != pos);

            // ----- 不可能达到的代码位置。 -----
            return (false, null, null);
        }
    }
}
