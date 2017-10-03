using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了确定的有限自动机的状态应遵循的基本约束。
    /// </summary>
    public interface IDFAState : IState { }

    /// <summary>
    /// 定义了确定的有限自动机的状态应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TTransition">确定的有限自动机的转换的类型。</typeparam>
    public interface IDFAState<TTransition> : IDFAState, IState<TTransition>
        where TTransition : ITransition
    { }
}
