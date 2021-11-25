using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
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

        IDFA Determine(IInputSymbols inputSet);
    }

    /// <inheritdoc cref="INFA"/>
    /// <typeparam name="TInput">接收的输入的类型。</typeparam>
    /// <typeparam name="TState">状态的类型。</typeparam>
    /// <typeparam name="TTransition">转换的类型。</typeparam>
    public interface INFA<TInput, TState, TTransition> : INFA, IFSM<TInput, TState, TTransition>
        where TState : IState<TTransition>
        where TTransition : ITransition<TInput, TState>
    {
        IDFA<TInput, TState, TTransition> Determine(IInputSymbols<TInput> inputSet);
    }
}
