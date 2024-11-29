namespace SamLu.StateMachine;

/// <summary>
/// 定义了有限状态机的状态应遵循的基本约束。
/// </summary>
public interface IState
{
    /// <summary>
    /// 获取或设置一个值，指示该状态是否为结束状态。
    /// </summary>
    bool IsTerminal { get; set; }

    /// <summary>
    /// 获取状态的转换集。
    /// </summary>
    ICollection<ITransition> Transitions { get; }

    /// <summary>
    /// 添加指定的转换。
    /// </summary>
    /// <param name="transition">要添加的转换。</param>
    /// <returns>一个值，指示操作是否成功。</returns>
    bool AttachTransition(ITransition transition);

    /// <summary>
    /// 移除指定的转换。
    /// </summary>
    /// <param name="transition">要添加的转换。</param>
    /// <returns>一个值，指示操作是否成功。</returns>
    bool RemoveTransition(ITransition transition);
}

/// <inheritdoc cref="IState"/>
/// <typeparam name="TTransition">转换的类型。</typeparam>
public interface IState<TTransition> : IState where TTransition : ITransition
{
    /// <inheritdoc cref="IState.Transitions"/>
    new ICollection<TTransition> Transitions { get; }

    /// <inheritdoc cref="IState.AttachTransition(ITransition)"/>
    bool AttachTransition(TTransition transition);

    /// <inheritdoc cref="IState.RemoveTransition(ITransition)"/>
    bool RemoveTransition(TTransition transition);
}
