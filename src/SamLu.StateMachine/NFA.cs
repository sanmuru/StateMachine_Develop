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
    
        [Obsolete("Not Implemented.", true)]
        public virtual void Optimize()
        {
            throw new NotImplementedException();
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
        /// 最小化 NFA 。
        /// </summary>
        public virtual void Optimize()
        {
            this.EpsilonClosure();
        }

        /// <summary>
        /// 消除 NFA 中的所有 ε 闭包。
        /// </summary>
        protected virtual void EpsilonClosure()
        {
            if (this.StartState
                .RecurGetTransitions<TState, TTransition>()
                .Any(transition => transition is TEpsilonTransition)
            )
            {
                var states = this.States;
                // 计算有效状态
                var avaliableStates = states.Where(state =>
                    // 起始状态
                    object.Equals(state,this.StartState) ||
                    // 存在非 ε 转换的输入转换
                    states.SelectMany(_state => _state.Transitions)
                        .Where(transition => object.Equals(transition.Target , state))
                        .Any(transition => !(transition is TEpsilonTransition))
                ).ToList();

                foreach (var avaliableState in avaliableStates)
                {
                    // 计算状态 avaliableState 的 ε 闭包。
                    // 所谓一个状态的 ε 闭包就是从这个状态出发，仅通过 ε 转换就可以到达的所有状态的集合。
                    var epsilonClosure = avaliableState.RecurGetReachableStates<TState,TTransition>().ToList();
                    // 把状态 avaliableState 从其 ε 闭包中排除出去。
                    epsilonClosure.Remove(avaliableState);

                    // 复制所有有效转换到状态 avaliableState 。
                    var avaliableTransitions = epsilonClosure
                        .SelectMany(state => state.Transitions)
                        .Where(transition => !(transition is TEpsilonTransition))
                        .ToList();
                    foreach (var avaliableTransition in avaliableTransitions)
                        this.AttachTransition(avaliableState, avaliableTransition);

                    // 移除状态 avaliableState 的所有 ε 转换。
                    // 与此同时，由于此状态机框架的实现方式：移除某个转换且其所指向的状态不为状态机任意可达转换的目标时，此状态不可达，即被排除于状态机外。
                    var epsilonTransitions = avaliableState.Transitions
                        .Where(transition => transition is TEpsilonTransition)
                        .ToList();
                    foreach (var epsilonTransition in epsilonTransitions)
                        this.RemoveTransition(avaliableState, epsilonTransition);

                    // 如果存在一个有效状态可以仅通过 ε 转换到达结束状态的话，那么这个状态应该被标记为结束状态。
                    if (epsilonClosure.Any(state => state.IsTerminal))
                        avaliableState.IsTerminal = true;
                }
            }
        }

        #region INFA Implementation
        bool INFA.AttachTransition(IState state, IEpsilonTransition epsilonTransition) => this.AttachTransition((TState)state, (TTransition)epsilonTransition);

        bool INFA.RemoveTransition(IState state, IEpsilonTransition epsilonTransition) => this.RemoveTransition((TState)state, (TTransition)epsilonTransition);
        #endregion
    }
}
