using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示确定的有限自动机。
    /// </summary>
    public class DFA : FSM, IDFA
    {
        /// <summary>
        /// 为 <see cref="DFA"/> 的一个指定状态添加指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="InvalidOperationException">在 <paramref name="transition"/> 为 <see cref="IEpsilonTransition"/> 接口的实例时抛出。试图向确定的有限自动机模型的状态中添加一个 ε 转换。</exception>
        public override bool AttachTransition(IState state, ITransition transition)
        {
            if (transition is IEpsilonTransition)
                throw new InvalidOperationException(
                    "试图向确定的有限自动机模型中添加一个 ε 转换。",
                    new ArgumentException("无法接受的 ε 转换。", nameof(transition))
                );

            return base.AttachTransition(state, transition);
        }

        /// <summary>
        /// 从 <see cref="DFA"/> 的一个指定状态移除指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="InvalidOperationException">在 <paramref name="transition"/> 为 <see cref="IEpsilonTransition"/> 接口的实例时抛出。试图从确定的有限自动机模型的状态中移除一个 ε 转换。</exception>
        public override bool RemoveTransition(IState state, ITransition transition)
        {
            if (transition is IEpsilonTransition)
                throw new InvalidOperationException(
                    "试图从确定的有限自动机模型中移除一个 ε 转换。",
                    new ArgumentException("无法接受的 ε 转换。", nameof(transition))
                );

            return base.RemoveTransition(state, transition);
        }
    }

    /// <summary>
    /// 表示确定的有限自动机。
    /// </summary>
    public class DFA<TState, TTransition> : FSM<TState, TTransition>, IDFA<TState, TTransition>
        where TState : IDFAState<TTransition>
        where TTransition : ITransition<TState>
    {
        /// <summary>
        /// 为 <see cref="DFA{TState, TTransition}"/> 的一个指定状态添加指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="InvalidOperationException">在 <paramref name="transition"/> 为 <see cref="IEpsilonTransition"/> 接口的实例时抛出。试图向确定的有限自动机模型的状态中添加一个 ε 转换。</exception>
        public override bool AttachTransition(TState state, TTransition transition)
        {
            if (transition is IEpsilonTransition)
                throw new InvalidOperationException(
                    "试图向确定的有限自动机模型中添加一个 ε 转换。",
                    new ArgumentException("无法接受的 ε 转换。", nameof(transition))
                );

            return base.AttachTransition(state, transition);
        }

        /// <summary>
        /// 从 <see cref="DFA{TState, TTransition}"/> 的一个指定状态移除指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="InvalidOperationException">在 <paramref name="transition"/> 为 <see cref="IEpsilonTransition"/> 接口的实例时抛出。试图从确定的有限自动机模型的状态中移除一个 ε 转换。</exception>
        public override bool RemoveTransition(TState state, TTransition transition)
        {
            if (transition is IEpsilonTransition)
                throw new InvalidOperationException(
                    "试图从确定的有限自动机模型中移除一个 ε 转换。",
                    new ArgumentException("无法接受的 ε 转换。", nameof(transition))
                );

            return base.RemoveTransition(state, transition);
        }
    }
}
