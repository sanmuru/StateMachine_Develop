namespace SamLu.StateMachine;

/// <summary>
/// 定义了确定的有限状态机应遵循的基本约束。
/// </summary>
public interface IDFA : IFSM { }

/// <inheritdoc cref="IDFA"/>
/// <typeparam name="TInput">接收的输入的类型。</typeparam>
/// <typeparam name="TState">状态的类型。</typeparam>
/// <typeparam name="TTransition">转换的类型。</typeparam>
public interface IDFA<TInput, TState, TTransition> : IDFA, IFSM<TInput, TState, TTransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
{ }
