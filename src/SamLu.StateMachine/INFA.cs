using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了非确定的有限自动机应遵循的约束。
    /// </summary>
    public interface INFA : IFSM
    {
        /// <summary>
        /// 向 <see cref="INFA"/> 的一个指定状态添加指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="epsilonTransition"/> 的值为 null 。</exception>
        bool AttachTransition(IState state, IEpsilonTransition epsilonTransition);

        /// <summary>
        /// 从 <see cref="INFA"/> 的一个指定状态移除指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="epsilonTransition"/> 的值为 null 。</exception>
        bool RemoveTransition(IState state, IEpsilonTransition epsilonTransition);
    }

    /// <summary>
    /// 定义了非确定的有限自动机应遵循的约束。
    /// </summary>
    /// <typeparam name="TState">非确定的有限自动机的状态的类型。</typeparam>
    /// <typeparam name="TTransition">非确定的有限自动机的转换的类型。</typeparam>
    /// <typeparam name="TEpsilonTransition">非确定的有限自动机的 ε 转换的类型。</typeparam>
    public interface INFA<TState, TTransition, TEpsilonTransition> : INFA, IFSM<TState, TTransition>
        where TState : IState<TTransition>
        where TTransition : class, ITransition<TState>
        where TEpsilonTransition : TTransition, IEpsilonTransition<TState>
    {
        /// <summary>
        /// 向 <see cref="INFA{TState, TTransition, TEpsilonTransition}"/> 的一个指定状态添加指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool AttachTransition(TState state, TEpsilonTransition epsilonTransition);

        /// <summary>
        /// 从 <see cref="INFA{TState, TTransition, TEpsilonTransition}"/> 的一个指定状态移除指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool RemoveTransition(TState state, TEpsilonTransition epsilonTransition);
    }
}
