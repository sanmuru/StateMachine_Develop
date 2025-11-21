using SamLu.StateMachine.ObjectModel;

namespace SamLu.StateMachine;

/// <summary>
/// 定义了不确定的有限状态机应遵循的基本约束。
/// </summary>
public interface INFA : IFSM
{
    /// <summary>
    /// 优化或最小化此不确定的有限状态机，以便转化为等价的确定的有限状态机。
    /// </summary>
    void Optimize();

    /// <summary>
    /// 基于若干不确定的有限状态机的状态创建一个确定的有限状态机的状态。
    /// </summary>
    /// <param name="states">参与合并的不确定的有限状态机的状态集合。</param>
    /// <returns>创建的确定的有限状态机的状态实例。</returns>
    IState CreateDFAState(params IEnumerable<IState> states);

    /// <summary>
    /// 基于若干不确定的有限状态机的转换创建一个确定的有限状态机的转换。
    /// </summary>
    /// <param name="inputEntries">触发该确定的有限状态机的转换的输入条目集合。</param>
    /// <param name="transitions">参与合并的不确定的有限状态机的转换集合。</param>
    /// <returns>创建的确定的有限状态机的转换实例。</returns>
    ITransition CreateDFATransition(IEnumerable<InputEntry> inputEntries, params IEnumerable<ITransition> transitions);

    /// <summary>
    /// 执行不确定的有限状态机的确定化处理，返回确定化后的初始确定的有限状态机的起始状态。
    /// </summary>
    /// <param name="provider">用于匹配输入符号的提供者。</param>
    /// <returns>确定化得到的确定的有限状态机的起始状态。</returns>
    IState Determine(IInputSymbolProvider provider);
}

/// <inheritdoc cref="INFA"/>
/// <typeparam name="TInput">接收的输入的类型。</typeparam>
/// <typeparam name="TState">状态的类型。</typeparam>
/// <typeparam name="TTransition">转换的类型。</typeparam>
/// <typeparam name="TDFAState">目标确定的有限状态机的状态的类型。</typeparam>
/// <typeparam name="TDFATransition">目标确定的有限状态机的转换的类型。</typeparam>
public interface INFA<TInput, TState, TTransition, TDFAState, TDFATransition> : INFA, IFSM<TInput, TState, TTransition>
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
    where TDFAState : IState<TDFATransition>
    where TDFATransition : ITransition<TInput, TDFAState>
{
    /// <inheritdoc cref="INFA.CreateDFAState(IEnumerable{IState})"/>
    TDFAState CreateDFAState(params IEnumerable<TState> states);

    /// <inheritdoc cref="INFA.CreateDFATransition(IEnumerable{InputEntry}, IEnumerable{ITransition})"/>
    TDFATransition CreateDFATransition(IEnumerable<InputEntry<TInput>> inputEntries, params IEnumerable<TTransition> transitions);

    /// <inheritdoc cref="INFA.Determine(IInputSymbolProvider)"/>
    TDFAState Determine(IInputSymbolProvider<TInput> provider);
}
