namespace SamLu.StateMachine;

/// <summary>
/// 表示确定的有限状态机。
/// </summary>
public abstract class DFA : FSM, IDFA
{ }

/// <summary>
/// 表示确定的有限状态机。
/// </summary>
/// <inheritdoc cref="IDFA{TInput, TState, TTransition}"/>
public abstract class DFA<TInput, TState, TTransition> : FSM<TInput, TState, TTransition>, IDFA<TInput, TState, TTransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
{ }
