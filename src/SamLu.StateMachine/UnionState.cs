using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示一个或多个有限状态机的状态的组合。
    /// </summary>
    public class UnionState : State, IUnionState
    {
        /// <summary>
        /// 储存组合中的一个或多个有限状态机的状态。
        /// </summary>
        protected IEnumerable<IState> states;

        /// <summary>
        /// 使用状态可变序列初始化实例。
        /// </summary>
        /// <param name="states">状态集。</param>
        public UnionState(params IState[] states) : this((IEnumerable<IState>)states) { }

        /// <summary>
        /// 使用指定的状态集初始化实例。
        /// </summary>
        /// <param name="states">指定的状态集。</param>
        public UnionState(IEnumerable<IState> states) => this.states = states ?? throw new ArgumentNullException(nameof(states));

        IEnumerator<IState> IEnumerable<IState>.GetEnumerator() => this.states.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.states.GetEnumerator();
    }

    /// <inheritdoc cref="UnionState"/>
    public class UnionState<TTransition> : State<TTransition>, IUnionState<TTransition> where TTransition : ITransition
    {
        /// <inheritdoc cref="UnionState.states"/>
        protected IEnumerable<IState<TTransition>> states;

        /// <inheritdoc cref="UnionState.UnionState(IState[])"/>
        public UnionState(params IState<TTransition>[] states) : this((IEnumerable<IState<TTransition>>)states) { }

        /// <inheritdoc cref="UnionState.UnionState(IEnumerable{IState})"/>
        public UnionState(IEnumerable<IState<TTransition>> states) => this.states = states ?? throw new ArgumentNullException(nameof(states));

        IEnumerator<IState<TTransition>> IEnumerable<IState<TTransition>>.GetEnumerator() => this.states.GetEnumerator();

        IEnumerator<IState> IEnumerable<IState>.GetEnumerator() => this.states.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.states.GetEnumerator();
    }
}
