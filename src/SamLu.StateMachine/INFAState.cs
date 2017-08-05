using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了非确定的有限自动机的状态应遵循的基本约束。
    /// </summary>
    public interface INFAState : IState
    {
        /// <summary>
        /// 添加指定的 ε 转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="epsilonTransition"/> 的值为 null 。</exception>
        bool AttachTransition(IEpsilonTransition epsilonTransition);

        /// <summary>
        /// 移除指定的 ε 转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="epsilonTransition"/> 的值为 null 。</exception>
        bool RemoveTransition(IEpsilonTransition epsilonTransition);
    }

    /// <summary>
    /// 定义了非确定的有限自动机的状态应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TTransition">非确定的有限自动机的转换的类型。</typeparam>
    /// <typeparam name="TEpsilonTransition">非确定的有限自动机的 ε 转换的类型。</typeparam>
    public interface INFAState<TTransition, TEpsilonTransition> : IState<TTransition>
        where TTransition : class, ITransition
        where TEpsilonTransition : TTransition, IEpsilonTransition
    {
        /// <summary>
        /// 添加指定的 ε 转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool AttachTransition(TEpsilonTransition epsilonTransition);

        /// <summary>
        /// 移除指定的 ε 转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool RemoveTransition(TEpsilonTransition epsilonTransition);
    }
}
