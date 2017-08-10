using SamLu.StateMachine.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了有限状态机的 ε 转换应遵循的基本约束。
    /// </summary>
    [DebugInfoProxy(typeof(EpsilonTransitionDebugInfo))]
    public interface IEpsilonTransition : ITransition { }

    /// <summary>
    /// 定义了有限状态机的 ε 转换应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
    public interface IEpsilonTransition<TState> : IEpsilonTransition, ITransition<TState> where TState : IState { }
}
