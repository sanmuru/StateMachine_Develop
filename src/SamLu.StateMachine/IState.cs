using SamLu.Diagnostics;
using SamLu.Runtime;
using SamLu.StateMachine.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了有限状态机的状态应遵循的基本约束。
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 获取或设置一个值，指示该有限自动机的状态是否为结束状态。
        /// </summary>
        bool IsTerminal { get; set; }

        /// <summary>
        /// 获取或设置表示 <see cref="IState"/> 的进入动作。在进入状态时进行。
        /// </summary>
        IAction EntryAction { get; set; }

        /// <summary>
        /// 获取或设置表示 <see cref="IState"/> 的退出动作。在退出状态时进行。
        /// </summary>
        IAction ExitAction { get; set; }

        /// <summary>
        /// 获取或设置表示 <see cref="IState"/> 的输入动作。依赖于当前状态和输入条件进行。
        /// </summary>
        IAction InputAction { get; set; }
        
        /// <summary>
        /// 获取 <see cref="IState"/> 的转换集。
        /// </summary>
        ICollection<ITransition> Transitions { get; }

        /// <summary>
        /// 添加指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        bool AttachTransition(ITransition transition);

        /// <summary>
        /// 移除指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        bool RemoveTransition(ITransition transition);
    }

    /// <summary>
    /// 定义了有限状态机的状态应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
    [DebugInfoProxy(typeof(FSMStateDebugInfo), TypeParameterFillin.ModifiedType)]
    public interface IState<TTransition> : IState where TTransition : ITransition
    {
        /// <summary>
        /// 获取 <see cref="IState{TTransition}"/> 的转换集。
        /// </summary>
        new ICollection<TTransition> Transitions { get; }

        /// <summary>
        /// 添加指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool AttachTransition(TTransition transition);

        /// <summary>
        /// 移除指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool RemoveTransition(TTransition transition);
    }
}
