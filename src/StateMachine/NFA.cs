using SamLu.StateMachine.ObjectModel;

namespace SamLu.StateMachine;

/// <summary>
/// 表示不确定的有限状态机。
/// </summary>
public abstract class NFA : FSM, INFA
{
    /// <inheritdoc/>
    public virtual void Optimize() => this.EpsilonClosure();

    /// <inheritdoc/>
    public abstract IState CreateDFAState(params IEnumerable<IState> states);

    /// <inheritdoc/>
    public abstract ITransition CreateDFATransition(IEnumerable<InputEntry> inputEntries, params IEnumerable<ITransition> transitions);

    /// <inheritdoc/>
    public virtual IState Determine(IInputSymbolProvider provider) => FSMUtility.Determine(this, provider);
}

/// <summary>
/// 表示不确定的有限状态机。
/// </summary>
/// <inheritdoc cref="INFA{TInput, TState, TTransition, TDFAState, TDFATransition}"/>
public abstract class NFA<TInput, TState, TTransition, TDFAState, TDFATransition> : FSM<TInput, TState, TTransition>, INFA<TInput, TState, TTransition, TDFAState, TDFATransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
    where TDFAState : IState<TDFATransition>
    where TDFATransition : ITransition<TInput, TDFAState>
{
    /// <inheritdoc/>
    public virtual void Optimize() => this.EpsilonClosure();
    void INFA.Optimize() => this.Optimize();

    /// <inheritdoc/>
    public abstract TDFAState CreateDFAState(params IEnumerable<TState> states);
    IState INFA.CreateDFAState(params IEnumerable<IState> states) => this.CreateDFAState(states.Cast<TState>());

    /// <inheritdoc/>
    public abstract TDFATransition CreateDFATransition(IEnumerable<InputEntry<TInput>> inputEntries, params IEnumerable<TTransition> transitions);
    ITransition INFA.CreateDFATransition(IEnumerable<InputEntry> inputEntries, params IEnumerable<ITransition> transitions) => this.CreateDFATransition(inputEntries.Select(static e => (InputEntry<TInput>)e), transitions.Cast<TTransition>());

    /// <inheritdoc/>
    public TDFAState Determine(IInputSymbolProvider<TInput> provider) => (TDFAState)FSMUtility.Determine(this, provider);
    IState INFA.Determine(IInputSymbolProvider provider) => this.Determine((IInputSymbolProvider<TInput>)provider);
}
