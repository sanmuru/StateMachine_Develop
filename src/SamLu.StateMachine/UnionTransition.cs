using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示一个或多个有限状态机的转换的组合。
    /// </summary>
    public class UnionTransition : Transition, IUnionTransition
    {
        /// <summary>
        /// 储存组合中的一个或多个有限状态机的转换。
        /// </summary>
        protected IEnumerable<ITransition> transitions;

        /// <inheritdoc/>
        public override bool IsEpsilon => this.transitions.All(transition => transition.IsEpsilon);

        /// <summary>
        /// 使用转换可变序列初始化实例。
        /// </summary>
        /// <param name="transitions">转换集。</param>
        public UnionTransition(params ITransition[] transitions) : this((IEnumerable<ITransition>)transitions) { }

        /// <summary>
        /// 使用指定的转换集初始化实例。
        /// </summary>
        /// <param name="transitions">指定的转换集。</param>
        /// <exception cref="ArgumentNullException"><paramref name="transitions"/> 的值为 <see langword="null"/> 。</exception>
        public UnionTransition(IEnumerable<ITransition> transitions) => this.transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));

        /// <inheritdoc/>
        public override bool Predicate(object? input) =>this.transitions.Any(transition => transition.Predicate(input));

        IEnumerator<ITransition> IEnumerable<ITransition>.GetEnumerator() => this.transitions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.transitions.GetEnumerator();
    }

    /// <inheritdoc cref="UnionTransition"/>
    public class UnionTransition<TInput, TState> : Transition<TInput,TState>, IUnionTransition<TInput, TState> where TState : IState
    {
        /// <inheritdoc cref="UnionTransition.transitions"/>
        protected IEnumerable<ITransition<TInput, TState>> transitions;

        /// <inheritdoc/>
        public override bool IsEpsilon => this.transitions.All(transition => transition.IsEpsilon);

        /// <inheritdoc cref="UnionTransition.UnionTransition(ITransition[])"/>
        public UnionTransition(params ITransition<TInput, TState>[] transitions) : this((IEnumerable<ITransition<TInput, TState>>)transitions) { }

        /// <inheritdoc cref="UnionTransition.UnionTransition(IEnumerable{ITransition})"/>
        public UnionTransition(IEnumerable<ITransition<TInput, TState>> transitions) => this.transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));

        /// <inheritdoc/>
        public override bool Predicate(TInput? input) => this.transitions.Any(transition => transition.Predicate(input));

        IEnumerator<ITransition<TInput, TState>> IEnumerable<ITransition<TInput, TState>>.GetEnumerator() => this.transitions.GetEnumerator();

        IEnumerator<ITransition> IEnumerable<ITransition>.GetEnumerator() => this.transitions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.transitions.GetEnumerator();
    }
}
