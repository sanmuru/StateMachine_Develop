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
        /// 添加指定的转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public bool AttachTransition(TEpsilonTransition epsilonTransition) => base.AttachTransition(epsilonTransition);

        /// <summary>
        /// 移除指定的转换。
        /// </summary>
        /// <param name="epsilonTransition">要添加的 ε 转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public bool RemoveTransition(TEpsilonTransition epsilonTransition) => base.RemoveTransition(epsilonTransition);
    }
}
