using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.EventDriven
{
    /// <summary>
    /// 定义了有限状态机的转换状态的动作应遵循的基本约束。
    /// </summary>
    [Obsolete("Use ITransition.TransitAction instead.", true)]
    public interface ITransitAction : IAction
    {
        /// <summary>
        /// <see cref="ITransitAction"/> 的调用方法。
        /// </summary>
        /// <param name="sender">调用方法施加的对象。</param>
        /// <param name="transition">调用方法接受的指定转换参数。</param>
        /// <param name="args">调用方法的其余参数。</param>
        void Invoke(object sender, ITransition transition, params object[] args);
    }

    /// <summary>
    /// 定义了有限状态机的转换状态的动作应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
    [Obsolete("Use ITransition.TransitAction instead.", true)]
    public interface ITransitAction<in TTransition> : ITransitAction
        where TTransition : ITransition
    {
        /// <summary>
        /// <see cref="ITransitAction{TTransition}"/> 的调用方法。
        /// </summary>
        /// <param name="sender">调用方法施加的对象。</param>
        /// <param name="transition">调用方法接受的指定转换参数。</param>
        /// <param name="args">调用方法的其余参数。</param>
        void Invoke(object sender, TTransition transition, params object[] args);
    }
}
