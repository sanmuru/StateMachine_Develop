using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了一个或多个有限状态机的状态的组合应遵循的基本约束。
    /// </summary>
    public interface IStateGroupState : IState, IEnumerable<IState> { }

    /// <inheritdoc cref="IStateGroupState"/>
    /// <typeparam name="TTransition">转换的类型。</typeparam>
    public interface IStateGroupState<TTransition> : IStateGroupState, IState<TTransition>, IEnumerable<IState<TTransition>> where TTransition : ITransition { }
}
