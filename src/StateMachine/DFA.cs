using SamLu.StateMachine.ObjectModel;

namespace SamLu.StateMachine;

public abstract class DFA : FSM, IDFA
{ }

public abstract class DFA<TInput, TState, TTransition> : FSM<TInput, TState, TTransition>, IDFA<TInput, TState, TTransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
{ }
