using SamLu.StateMachine.ObjectModel;

namespace SamLu.StateMachine;

/// <summary>
/// 表示有限状态机。
/// </summary>
public abstract class FSM : IFSM
{
    /// <inheritdoc/>
    /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
    public virtual IState CurrentState
    {
        get => field ?? throw new UninitializedException("有限状态机未初始化。");
        protected set => field = value;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="value"/>的值为<see langword="null"/>。</exception>
    public virtual IState StartState
    {
        get => field ?? throw new UninitializedException("有限状态机未初始化。");
        set
        {
            ArgumentNullExceptionExtension.ThrowIfNull(value);

            field = value;
            this.Reset();
        }
    }

    /// <inheritdoc/>
    /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
    public virtual void Reset()
    {
        if (this.CurrentState is null) throw new UninitializedException("有限状态机未初始化。");

        this.CurrentState = this.StartState;
    }

    /// <inheritdoc/>
    public abstract bool Transit(object? input, IInputSymbolProvider provider);
}

/// <summary>
/// 表示有限状态机。
/// </summary>
/// <inheritdoc cref="IFSM{TInput, TState, TTransition}"/>
public abstract class FSM<TInput, TState, TTransition> : IFSM<TInput, TState, TTransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
{
    /// <inheritdoc/>
    /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
    public virtual TState CurrentState
    {
        get => field ?? throw new UninitializedException("有限状态机未初始化。");
        set => field = value;
    }
    IState IFSM.CurrentState => this.CurrentState;

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> 的值为<see langword="null"/>。</exception>
    public virtual TState StartState
    {
        get => field ?? throw new UninitializedException("有限状态机未初始化。");
        set
        {
            ArgumentNullExceptionExtension.ThrowIfNull(value);

            field = value;
            this.Reset();
        }
    }
    IState IFSM.StartState
    {
        get => this.StartState;
        set => this.StartState = (TState)value;
    }

    /// <inheritdoc/>
    /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
    public virtual void Reset() => this.CurrentState = this.StartState;

    /// <inheritdoc/>
    public abstract bool Transit(TInput? input, IInputSymbolProvider<TInput> provider);
    bool IFSM.Transit(object? input, IInputSymbolProvider provider) => this.Transit((TInput?)input, (IInputSymbolProvider<TInput>)provider);
}
