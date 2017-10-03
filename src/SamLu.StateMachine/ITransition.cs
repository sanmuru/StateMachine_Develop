using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了有限状态机的转换应遵循的基本约束。
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// 获取或设置 <see cref="ITransition"/> 指向的状态。
        /// </summary>
        IState Target { get; }

        /// <summary>
        /// 获取或设置表示 <see cref="ITransition"/> 的转换动作。在转换转换时进行。
        /// </summary>
        IAction TransitAction { get; set; }

        /// <summary>
        /// 将转换的目标设为指定状态。
        /// </summary>
        /// <param name="state">要设为目标的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        bool SetTarget(IState state);
    }

    /// <summary>
    /// 定义了有限状态机的转换应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
    public interface ITransition<TState> : ITransition where TState : IState
    {
        /// <summary>
        /// 获取或设置 <see cref="ITransition{TState}"/> 指向的状态。
        /// </summary>
        new TState Target { get; }

        /// <summary>
        /// 将转换的目标设为指定状态。
        /// </summary>
        /// <param name="state">要设为目标的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool SetTarget(TState state);
    }
}
