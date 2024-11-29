using SamLu.StateMachine.ObjectModel;

namespace SamLu.StateMachine;

/// <summary>
/// 定义了不确定的有限状态机应遵循的基本约束。
/// </summary>
public interface INFA : IFSM
{
    /// <summary>
    /// 最小化，以转化为确定的有限状态机。
    /// </summary>
    /// <returns>转化得到的确定的有限状态机。</returns>
    void Optimize();

    IState CreateDFAState(params IEnumerable<IState> states);
    ITransition CreateDFATransition(IEnumerable<InputEntry> inputEntries, params IEnumerable<ITransition> transitions);

    IState Determine(IInputSymbolProvider provider);
}

/// <inheritdoc cref="INFA"/>
/// <typeparam name="TInput">接收的输入的类型。</typeparam>
/// <typeparam name="TState">状态的类型。</typeparam>
/// <typeparam name="TTransition">转换的类型。</typeparam>
public interface INFA<TInput, TState, TTransition, TDFAState, TDFATransition> : INFA, IFSM<TInput, TState, TTransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
    where TDFAState : IState<TDFATransition>
    where TDFATransition : ITransition<TInput, TDFAState>
{
    TDFAState CreateDFAState(params IEnumerable<TState> states);
    TDFATransition CreateDFATransition(IEnumerable<InputEntry<TInput>> inputEntries, params IEnumerable<TTransition> transitions);

    TDFAState Determine(IInputSymbolProvider<TInput> provider);
}
