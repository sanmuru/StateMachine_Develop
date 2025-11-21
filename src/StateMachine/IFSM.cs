using SamLu.StateMachine.ObjectModel;

namespace SamLu.StateMachine;

/// <summary>
/// 定义了有限状态机应遵循的基本约束。
/// </summary>
public interface IFSM
{
    /// <summary>
    /// 获取当前状态。
    /// </summary>
    IState CurrentState { get; }

    /// <summary>
    /// 获取或设置起始状态。
    /// </summary>
    IState StartState { get; set; }

    /// <summary>
    /// 接受一个指定输入并进行转换。返回一个值，指示操作是否成功。
    /// </summary>
    /// <param name="input">接收的输入。</param>
    /// <param name="provider">用于匹配输入符号的提供者。</param>
    /// <returns>一个值，指示移动是否成功。</returns>
    bool Transit(object? input, IInputSymbolProvider provider);

    /// <summary>
    /// 重置当前状态为起始状态。
    /// </summary>
    void Reset();
}

/// <inheritdoc cref="IFSM"/>
/// <typeparam name="TInput">接收的输入的类型。</typeparam>
/// <typeparam name="TState">状态的类型。</typeparam>
/// <typeparam name="TTransition">转换的类型。</typeparam>
public interface IFSM<TInput, TState, TTransition> : IFSM
    where TState : IState<TTransition>
    where TTransition : ITransition<TInput, TState>
{
    /// <inheritdoc cref="IFSM.CurrentState"/>
    new TState CurrentState { get; }

    /// <inheritdoc cref="IFSM.StartState"/>
    new TState StartState { get; set; }

    /// <inheritdoc cref="IFSM.Transit(object,IInputSymbolProvider)"/>
    bool Transit(TInput? input, IInputSymbolProvider<TInput> provider);
}
