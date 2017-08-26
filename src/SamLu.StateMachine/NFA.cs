using SamLu.StateMachine.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示非确定的有限自动机。
    /// </summary>
    public class NFA : FSM, INFA
    {
        /// <summary>
        /// 向 <see cref="NFA"/> 的一个指定状态添加指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool AttachTransition(IState state, IEpsilonTransition epsilonTransition) => this.AttachTransition(state, (ITransition)epsilonTransition);

        /// <summary>
        /// 从 <see cref="NFA"/> 的一个指定状态移除指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool RemoveTransition(IState state, IEpsilonTransition epsilonTransition) => this.RemoveTransition(state, (ITransition)epsilonTransition);

        /// <summary>
        /// 最小化 <see cref="NFA"/> 。
        /// </summary>
        public virtual void Optimize()
        {
            this.EpsilonClosure();
        }
    }

    /// <summary>
    /// 表示非确定的有限自动机。
    /// </summary>
    public class NFA<TState, TTransition, TEpsilonTransition> : FSM<TState, TTransition>, INFA<TState, TTransition, TEpsilonTransition>
        where TState : IState<TTransition>
        where TTransition : class, ITransition<TState>
        where TEpsilonTransition : TTransition, IEpsilonTransition<TState>
    {
        /// <summary>
        /// 向 <see cref="NFA{TState, TTransition, TEpsilonTransition}"/> 的一个指定状态添加指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool AttachTransition(TState state, TEpsilonTransition epsilonTransition) => base.AttachTransition(state, epsilonTransition);

        /// <summary>
        /// 从 <see cref="NFA{TState, TTransition, TEpsilonTransition}"/> 的一个指定状态移除指定 ε 转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool RemoveTransition(TState state, TEpsilonTransition epsilonTransition) => base.RemoveTransition(state, epsilonTransition);

        /// <summary>
        /// 最小化 <see cref="NFA{TState, TTransition, TEpsilonTransition}"/> 。
        /// </summary>
        public virtual void Optimize()
        {
            this.EpsilonClosure();
        }

        #region INFA Implementation
        bool INFA.AttachTransition(IState state, IEpsilonTransition epsilonTransition) => this.AttachTransition((TState)state, (TTransition)epsilonTransition);

        bool INFA.RemoveTransition(IState state, IEpsilonTransition epsilonTransition) => this.RemoveTransition((TState)state, (TTransition)epsilonTransition);
        #endregion
    }
}
