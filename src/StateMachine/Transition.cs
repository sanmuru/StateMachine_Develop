﻿using SamLu.StateMachine.ObjectModel;
using System.Collections.Immutable;

namespace SamLu.StateMachine;

/// <summary>
/// 表示有限状态机的转换。
/// </summary>
public abstract class Transition : ITransition
{
    /// <summary>
    /// 储存转换指向的状态。
    /// </summary>
    protected IState? target;

    /// <inheritdoc/>
    public virtual IState? Target => this.target;

    /// <inheritdoc/>
    public abstract bool IsEpsilon { get; }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 <see langword="null"/> 。</exception>
    public virtual bool SetTarget(IState state)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(state);

        if (this.target is not null && object.Equals(this.target, state))
            return false;
        else
        {
            this.target = state;
            return true;
        }
    }

    /// <inheritdoc/>
    public abstract ImmutableArray<InputEntry> InputEntries { get; }
}

/// <inheritdoc cref="Transition"/>
/// <typeparam name="TInput">接收的输入的类型。</typeparam>
/// <typeparam name="TState">状态的类型。</typeparam>
public abstract class Transition<TInput, TState> : ITransition<TInput, TState> where TState : IState
{
    /// <inheritdoc cref="Transition.target"/>
    protected TState? target;

    /// <inheritdoc/>
    public virtual TState? Target => this.target;
    IState? ITransition.Target => this.Target;

    /// <inheritdoc/>
    public abstract bool IsEpsilon { get; }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 <see langword="null"/> 。</exception>
    public bool SetTarget(TState state)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(state);

        if (this.target is not null && object.Equals(this.target, state))
            return false;
        else
        {
            this.target = state;
            return true;
        }
    }
    bool ITransition.SetTarget(IState state) => this.SetTarget((TState)state);

    /// <inheritdoc/>
    public abstract ImmutableArray<InputEntry<TInput>> InputEntries { get; }
    ImmutableArray<InputEntry> ITransition.InputEntries => this.InputEntries.Select(static entry => (InputEntry)entry).ToImmutableArray();
}
