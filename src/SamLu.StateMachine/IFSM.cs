using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了有限状态机应遵循的基本约束。
    /// </summary>
    public interface IFSM
    {
        /// <summary>
        /// 获取 <see cref="IFSM"/> 的当前状态。
        /// </summary>
        IState CurrentState { get; }

        /// <summary>
        /// 获取 <see cref="IFSM"/> 的起始状态。
        /// </summary>
        IState StartState { get; }

        /// <summary>
        /// 获取 <see cref="IFSM"/> 的状态集。
        /// </summary>
        ICollection<IState> States { get; }

        /// <summary>
        /// 将所有状态和转换从有限状态机模型中移除。
        /// </summary>
        void Clear();

        /// <summary>
        /// 向 <see cref="IFSM"/> 的一个指定状态添加指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        bool AttachTransition(IState state, ITransition transition);

        /// <summary>
        /// 从 <see cref="IFSM"/> 的一个指定状态移除指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        bool RemoveTransition(IState state, ITransition transition);

        /// <summary>
        /// 将 <see cref="IFSM"/> 的一个指定转换的目标设为指定状态。
        /// </summary>
        /// <param name="transition">指定的目标。</param>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        bool SetTarget(ITransition transition, IState state);

        /// <summary>
        /// 获取用户数据字典。
        /// </summary>
        IDictionary<object, object> UserData { get; }

        /// <summary>
        /// 重置 <see cref="IFSM"/> 的方法。
        /// </summary>
        /// <exception cref="InvalidOperationException">此有限状态机模型的 <see cref="StartState"/> 为 null ，即无起始状态。</exception>
        /// <exception cref="NotSupportedException">此有限状态机模型不支持重置操作。</exception>
        void Reset();
        
        /// <summary>
        /// 接受一个指定输入并进行转换。返回一个值，指示操作是否成功。
        /// </summary>
        /// <typeparam name="TInput">输入的类型。</typeparam>
        /// <param name="input">指定的输入。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentOutOfRangeException">接受的输入超出有限状态机模型能接受的范围。建议返回 false 代替抛出此异常。</exception>
        bool Transit<TInput>(TInput input);
    }

    /// <summary>
    /// 定义了有限状态机应遵循的基本约束。
    /// </summary>
    /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
    /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
    public interface IFSM<TState, TTransition> : IFSM
        where TState : IState<TTransition>
        where TTransition : ITransition<TState>
    {
        /// <summary>
        /// 获取 <see cref="IFSM{TState, TTransition}"/> 的当前状态。
        /// </summary>
        new TState CurrentState { get; }

        /// <summary>
        /// 获取 <see cref="IFSM{TState, TTransition}"/> 的起始状态。
        /// </summary>
        new TState StartState { get; }

        /// <summary>
        /// 获取 <see cref="IFSM{TState, TTransition}"/> 的状态集。
        /// </summary>
        new ICollection<TState> States { get; }

        /// <summary>
        /// 为 <see cref="IFSM{TState, TTransition}"/> 的一个指定状态添加指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool AttachTransition(TState state, TTransition transition);

        /// <summary>
        /// 从 <see cref="IFSM{TState, TTransition}"/> 的一个指定状态移除指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool RemoveTransition(TState state, TTransition transition);

        /// <summary>
        /// 将 <see cref="IFSM{TState, TTransition}"/> 的一个指定转换的目标设为指定状态。
        /// </summary>
        /// <param name="transition">指定的目标。</param>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool SetTarget(TTransition transition, TState state);
    }
}
