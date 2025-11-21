using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace SamLu.StateMachine;

/// <summary>
/// 表示有限状态机的状态。
/// </summary>
public abstract class State : IState
{
    /// <summary>
    /// 储存状态的转换集。
    /// </summary>
    protected HashSet<ITransition> transitions = [];

    /// <inheritdoc/>
    public virtual bool IsTerminal { get; set; }

    /// <inheritdoc/>
    public virtual IReadOnlySet<ITransition> Transitions => field ?? new ReadOnlyTransitionSet<ITransition>(this.transitions);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
    public virtual bool AttachTransition(ITransition transition)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        return this.transitions.Add(transition);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
    public bool RemoveTransition(ITransition transition)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        return this.transitions.Remove(transition);
    }
}

/// <inheritdoc cref="State"/>
/// <typeparam name="TTransition">转换的类型。</typeparam>
public abstract class State<TTransition> : IState<TTransition> where TTransition : ITransition
{
    /// <summary>
    /// 储存状态的转换集。
    /// </summary>
    protected HashSet<TTransition> transitions = [];

    /// <inheritdoc/>
    public virtual bool IsTerminal { get; set; }

    /// <inheritdoc/>
    public virtual IReadOnlySet<TTransition> Transitions => field ?? new ReadOnlyTransitionSet<TTransition>(this.transitions);
    IReadOnlySet<ITransition> IState.Transitions => field ?? new ReadOnlyTransitionInterfaceSet<TTransition>(this.transitions);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
    public virtual bool AttachTransition(TTransition transition)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        return this.transitions.Add(transition);
    }

    bool IState.AttachTransition(ITransition transition) => this.AttachTransition((TTransition)transition);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
    public virtual bool RemoveTransition(TTransition transition)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        return this.transitions.Remove(transition);
    }

    bool IState.RemoveTransition(ITransition transition) => this.RemoveTransition((TTransition)transition);
}

file class ReadOnlyTransitionSet<TTransition>(HashSet<TTransition> underlying) : IReadOnlySet<TTransition>
    where TTransition : ITransition
{
    public int Count => underlying.Count;
    public bool Contains(TTransition item) => underlying.Contains(item);
    public IEnumerator<TTransition> GetEnumerator() => underlying.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
    public bool IsProperSubsetOf(IEnumerable<TTransition> other) => underlying.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<TTransition> other) => underlying.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<TTransition> other) => underlying.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<TTransition> other) => underlying.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<TTransition> other) => underlying.Overlaps(other);
    public bool SetEquals(IEnumerable<TTransition> other) => underlying.SetEquals(other);
}

file class ReadOnlyTransitionInterfaceSet<TTransition>(HashSet<TTransition> underlying) : IReadOnlySet<ITransition>
    where TTransition : ITransition
{
    public int Count => underlying.Count;
    public bool Contains(ITransition item) => item is TTransition transition && underlying.Contains(transition);
    public IEnumerator<ITransition> GetEnumerator() => underlying.Cast<ITransition>().GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
    public bool IsProperSubsetOf(IEnumerable<ITransition> other) => other.AnyNotOfType<ITransition, TTransition>() ? underlying.IsSubsetOf(other.OfType<TTransition>()) : underlying.IsProperSubsetOf(other.Cast<TTransition>());
    public bool IsProperSupersetOf(IEnumerable<ITransition> other) => other.AllOfType<ITransition, TTransition>() && underlying.IsProperSupersetOf(other.Cast<TTransition>());
    public bool IsSubsetOf(IEnumerable<ITransition> other) => underlying.IsSubsetOf(other.OfType<TTransition>());
    public bool IsSupersetOf(IEnumerable<ITransition> other) => other.AllOfType<ITransition, TTransition>() && underlying.IsSupersetOf(other.Cast<TTransition>());
    public bool Overlaps(IEnumerable<ITransition> other) => underlying.Overlaps(other.OfType<TTransition>());
    public bool SetEquals(IEnumerable<ITransition> other) => other.AllOfType<ITransition, TTransition>() && underlying.SetEquals(other.Cast<TTransition>());
}