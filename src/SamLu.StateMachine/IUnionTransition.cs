using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了一个或多个有限状态机的转换的组合应遵循的基本约束。
    /// </summary>
    public interface IUnionTransition : ITransition, IEnumerable<ITransition> { }

    /// <inheritdoc cref="IUnionTransition"/>
    /// <typeparam name="TInput">接收的输入的类型。</typeparam>
    /// <typeparam name="TState">状态的类型。</typeparam>
    public interface IUnionTransition<TInput, TState> : ITransition<TInput, TState>, IUnionTransition, IEnumerable<ITransition<TInput, TState>> where TState : IState { }
}
