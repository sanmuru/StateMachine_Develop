using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示非确定的有限自动机的状态。
    /// </summary>
    /// <typeparam name="TTransition">非确定的有限自动机的转换的类型。</typeparam>
    /// <typeparam name="TEpsilonTransition">非确定的有限自动机的 ε 转换的类型。</typeparam>
    public class NFAState<TTransition, TEpsilonTransition> : FSMState<TTransition>, INFAState<TTransition, TEpsilonTransition>
        where TTransition : class, ITransition
        where TEpsilonTransition : TTransition, IEpsilonTransition
    {
        /// <summary>
        /// 初始化 <see cref="NFAState{TTransition, TEpsilonTransition}"/> 类的新实例。
        /// </summary>
        public NFAState() : base() { }

        /// <summary>
        /// 初始化 <see cref="NFAState{TTransition, TEpsilonTransition}"/> 类的新实例，该实例接受一个指定是否为结束状态的值。
        /// </summary>
        /// <param name="isTerminal">一个值，指示该实例是否为结束状态。</param>
        public NFAState(bool isTerminal) : base(isTerminal) { }

        /// <summary>
        /// 添加指定的转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool AttachTransition(TEpsilonTransition epsilonTransition) => base.AttachTransition(epsilonTransition);

        /// <summary>
        /// 移除指定的转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool RemoveTransition(TEpsilonTransition epsilonTransition) => base.RemoveTransition(epsilonTransition);
    }
}
