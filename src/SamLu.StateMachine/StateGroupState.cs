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
    public class StateGroupState : State, IStateGroupState
    {
        /// <summary>
        /// 储存组合中的一个或多个有限状态机的状态。
        /// </summary>
        protected IEnumerable<IState> states;

        /// <summary>
        /// 使用状态可变序列初始化实例。
        /// </summary>
        /// <param name="states">状态集。</param>
        public StateGroupState(params IState[] states) : this((IEnumerable<IState>)states) { }

        /// <summary>
        /// 使用指定的状态集初始化实例。
        /// </summary>
        /// <param name="states">指定的状态集。</param>
        public StateGroupState(IEnumerable<IState> states) => this.states = states ?? throw new ArgumentNullException(nameof(states));

        IEnumerator<IState> IEnumerable<IState>.GetEnumerator() => this.states.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.states.GetEnumerator();
    }

    /// <inheritdoc cref="StateGroupState"/>
    public class StateGroupState<TTransition> : State<TTransition>, IStateGroupState<TTransition> where TTransition : ITransition
    {
        /// <inheritdoc cref="StateGroupState.states"/>
        protected IEnumerable<IState<TTransition>> states;

        /// <inheritdoc cref="StateGroupState.StateGroupState(IState[])"/>
        public StateGroupState(params IState<TTransition>[] states) : this((IEnumerable<IState<TTransition>>)states) { }

        /// <inheritdoc cref="StateGroupState.StateGroupState(IEnumerable{IState})"/>
        public StateGroupState(IEnumerable<IState<TTransition>> states) => this.states = states ?? throw new ArgumentNullException(nameof(states));

        IEnumerator<IState<TTransition>> IEnumerable<IState<TTransition>>.GetEnumerator() => this.states.GetEnumerator();

        IEnumerator<IState> IEnumerable<IState>.GetEnumerator() => this.states.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.states.GetEnumerator();
    }
}
