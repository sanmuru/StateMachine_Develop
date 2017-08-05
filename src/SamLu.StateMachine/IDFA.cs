using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了确定的有限自动机应遵循的约束。
    /// </summary>
    public interface IDFA : IFSM { }

    /// <summary>
    /// 定义了确定的有限自动机应遵循的约束。
    /// </summary>
    public interface IDFA<TState, TTransition> : IDFA, IFSM<TState, TTransition>
        where TState : IState<TTransition>
        where TTransition : ITransition<TState>
    { }
}
