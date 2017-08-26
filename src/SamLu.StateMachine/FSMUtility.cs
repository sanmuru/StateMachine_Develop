using SamLu.StateMachine.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 提供了一系列操作、获取有关有限状态机及其部件信息的静态方法。
    /// </summary>
    public static class FSMUtility
    {
        #region RecurGetStates
        /// <summary>
        /// 递归获取指定起始状态开始能达到的所有状态的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态。</param>
        /// <returns>指定起始状态开始能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<IState> RecurGetStates(this IState startState)
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<IState> states = new HashSet<IState>();
            FSMUtility.RecurGetStatesInternal(startState, states);

            return states;
        }

        /// <summary>
        /// 递归获取指定转换开始能达到的所有状态的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<IState> RecurGetStates(this ITransition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            return transition.Target.RecurGetStates();
        }

        private static void RecurGetStatesInternal(IState startState, HashSet<IState> states)
        {
            if (states.Add(startState))
            {
                foreach (var transition in startState.Transitions.Where(transition => transition.Target != null))
                    FSMUtility.RecurGetStatesInternal(transition.Target, states);
            }
        }

        /// <summary>
        /// 递归获取指定起始状态开始能达到的所有状态的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态。</param>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <returns>指定起始状态开始能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<TState> RecurGetStates<TState, TTransition>(this TState startState)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<TState> states = new HashSet<TState>();
            FSMUtility.RecurGetStatesInternal<TState, TTransition>(startState, states);

            return states;
        }

        /// <summary>
        /// 递归获取指定转换开始能达到的所有状态的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <returns>指定转换开始能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<TState> RecurGetStates<TState, TTransition>(this TTransition transition)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            return transition.Target.RecurGetStates<TState, TTransition>();
        }

        private static void RecurGetStatesInternal<TState, TTransition>(TState startState, HashSet<TState> states)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (states.Add(startState))
            {
                foreach (var transition in startState.Transitions.Where(transition => transition.Target != null))
                    FSMUtility.RecurGetStatesInternal<TState, TTransition>(transition.Target, states);
            }
        }
        #endregion

        #region RecurGetTransitions
        /// <summary>
        /// 递归获取指定起始状态开始能经到的所有转换的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态</param>
        /// <returns>指定起始状态开始能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<ITransition> RecurGetTransitions(this IState startState)
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<ITransition> transitions = new HashSet<ITransition>();
            foreach (var transition in startState.Transitions)
                transitions.UnionWith(transition.RecurGetTransitions());

            return transitions;
        }

        /// <summary>
        /// 递归获取指定转换开始能经到的所有转换的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<ITransition> RecurGetTransitions(this ITransition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            HashSet<ITransition> transitions = new HashSet<ITransition>();
            FSMUtility.RecurGetTransitionsInternal(transition, transitions);

            return transitions;
        }

        private static void RecurGetTransitionsInternal(ITransition transition, HashSet<ITransition> transitions)
        {
            if (transitions.Add(transition))
            {
                if (transition.Target != null)
                {
                    foreach (var _transition in transition.Target.Transitions.Where(t => t != null))
                        FSMUtility.RecurGetTransitionsInternal(_transition, transitions);
                }
            }
        }

        /// <summary>
        /// 递归获取指定起始状态开始能经到的所有转换的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态。</param>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <returns>指定起始状态开始能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<TTransition> RecurGetTransitions<TState, TTransition>(this TState startState)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<TTransition> transitions = new HashSet<TTransition>();
            foreach (var transition in startState.Transitions)
                transitions.UnionWith(transition.RecurGetTransitions<TState, TTransition>());

            return transitions;
        }

        /// <summary>
        /// 递归获取指定转换开始能经到的所有转换的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <returns>指定转换开始能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<TTransition> RecurGetTransitions<TState, TTransition>(this TTransition transition)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            HashSet<TTransition> transitions = new HashSet<TTransition>();
            FSMUtility.RecurGetTransitionsInternal<TState, TTransition>(transition, transitions);

            return transitions;
        }

        private static void RecurGetTransitionsInternal<TState, TTransition>(TTransition transition, HashSet<TTransition> transitions)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (transitions.Add(transition))
            {
                if (transition.Target != null)
                {
                    foreach (var _transition in transition.Target.Transitions.Where(t => t != null))
                        FSMUtility.RecurGetTransitionsInternal<TState, TTransition>(_transition, transitions);
                }
            }
        }
        #endregion

        #region RecurGetReachableStates
        /// <summary>
        /// 递归获取指定起始状态开始无须接受输入就能达到的所有状态的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态。</param>
        /// <returns>指定起始状态开始无须接受输入就能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<IState> RecurGetReachableStates(this IState startState)
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<IState> states = new HashSet<IState>();
            FSMUtility.RecurGetReachableStatesInternal(startState, states);

            return states;
        }

        /// <summary>
        /// 递归获取指定转换开始无须接受输入就能达到的所有状态的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始无须接受输入就能达到的所有状态的集合。</returns>
        public static IEnumerable<IState> RecurGetReachableStates(this ITransition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            HashSet<IState> states = new HashSet<IState> { transition.Target };
            FSMUtility.RecurGetReachableStatesInternal(transition.Target, states);

            return states;
        }

        private static void RecurGetReachableStatesInternal(IState startState, HashSet<IState> states)
        {
            foreach (var transition in startState.Transitions.Where(_transition => _transition != null && _transition.Target != null))
            {
                if (transition is IEpsilonTransition && states.Add(transition.Target))
                    FSMUtility.RecurGetReachableStatesInternal(transition.Target, states);
            }
        }

        /// <summary>
        /// 递归获取指定起始状态开始无须接受输入就能达到的所有状态的集合。
        /// </summary>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <param name="startState">指定的起始状态。</param>
        /// <returns>指定起始状态开始无须接受输入就能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<TState> RecurGetReachableStates<TState, TTransition>(this TState startState)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<TState> states = new HashSet<TState>();
            FSMUtility.RecurGetReachableStatesInternal<TState, TTransition>(startState, states);

            return states;
        }

        /// <summary>
        /// 递归获取指定转换开始无须接受输入就能达到的所有状态的集合。
        /// </summary>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始无须接受输入就能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<TState> RecurGetReachableStates<TState, TTransition>(this TTransition transition)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            HashSet<TState> states = new HashSet<TState> { transition.Target };
            FSMUtility.RecurGetReachableStatesInternal<TState, TTransition>(transition.Target, states);

            return states;
        }

        private static void RecurGetReachableStatesInternal<TState, TTransition>(TState startState, HashSet<TState> states)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            foreach (var transition in startState.Transitions.Where(_transition => _transition != null && _transition.Target != null))
            {
                if (transition is IEpsilonTransition && states.Add(transition.Target))
                    FSMUtility.RecurGetReachableStatesInternal<TState, TTransition>(transition.Target, states);
            }
        }
        #endregion

        #region RecurGetReachableTransitions
        /// <summary>
        /// 递归获取指定起始状态开始无须接受输入就能经到的所有转换的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态</param>
        /// <returns>指定起始状态开始无须接受输入就能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<ITransition> RecurGetReachableTransitions(this IState startState)
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<ITransition> transitions = new HashSet<ITransition>();
            foreach (var transition in startState.Transitions.Where(_transition => _transition != null))
            {
                if (transitions.Add(transition) && transition is IEpsilonTransition)
                    transitions.UnionWith(transition.RecurGetReachableTransitions());
            }

            return transitions;
        }

        /// <summary>
        /// 递归获取指定转换开始无须接受输入就能经到的所有转换的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始无须接受输入就能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<ITransition> RecurGetReachableTransitions(this ITransition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            HashSet<ITransition> transitions = new HashSet<ITransition>();
            FSMUtility.RecurGetReachableTransitionsInternal(transition, transitions);

            return transitions;
        }

        private static void RecurGetReachableTransitionsInternal(ITransition transition, HashSet<ITransition> transitions)
        {
            foreach (var t in transition.Target.Transitions)
            {
                if (transitions.Add(t) && t is IEpsilonTransition)
                    FSMUtility.RecurGetReachableTransitionsInternal(transition, transitions);
            }
        }

        /// <summary>
        /// 递归获取指定起始状态开始无须接受输入就能经到的所有转换的集合。
        /// </summary>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <param name="startState">指定的起始状态</param>
        /// <returns>指定起始状态开始无须接受输入就能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 null 。</exception>
        public static IEnumerable<TTransition> RecurGetReachableTransitions<TState, TTransition>(this TState startState)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (startState == null) throw new ArgumentNullException(nameof(startState));

            HashSet<TTransition> transitions = new HashSet<TTransition>();
            foreach (var transition in startState.Transitions.Where(_transition => _transition != null))
            {
                if (transitions.Add(transition) && transition is IEpsilonTransition)
                    transitions.UnionWith(transition.RecurGetReachableTransitions<TState, TTransition>());
            }

            return transitions;
        }

        /// <summary>
        /// 递归获取指定转换开始无须接受输入就能经到的所有转换的集合。
        /// </summary>
        /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
        /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始无须接受输入就能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public static IEnumerable<TTransition> RecurGetReachableTransitions<TState, TTransition>(this TTransition transition)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            HashSet<TTransition> transitions = new HashSet<TTransition>();
            FSMUtility.RecurGetReachableTransitionsInternal<TState, TTransition>(transition, transitions);

            return transitions;
        }

        private static void RecurGetReachableTransitionsInternal<TState, TTransition>(TTransition transition, HashSet<TTransition> transitions)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            foreach (var t in transition.Target.Transitions)
            {
                if (transitions.Add(t) && t is IEpsilonTransition)
                    FSMUtility.RecurGetReachableTransitionsInternal<TState, TTransition>(transition, transitions);
            }
        }
        #endregion

        #region EpsilonClosure
        /// <summary>
        /// 消除 <see cref="INFA"/> 中的所有 ε 闭包。
        /// </summary>
        public static void EpsilonClosure(this INFA nfa)
        {
            if (nfa == null) throw new ArgumentNullException(nameof(nfa));

            if (nfa.StartState
                .RecurGetTransitions()
                .Any(transition => transition is IEpsilonTransition)
            )
            {
                var states = nfa.States;
                // 计算有效状态
                var avaliableStates = states.Where(state =>
                    // 起始状态
                    object.Equals(state, nfa.StartState) ||
                    // 存在非 ε 转换的输入转换
                    states.SelectMany(_state => _state.Transitions)
                        .Where(transition => object.Equals(transition.Target, state))
                        .Any(transition => !(transition is IEpsilonTransition))
                ).ToList();

                foreach (var avaliableState in avaliableStates)
                {
                    /* 计算状态 avaliableState 的 ε 闭包。 */
                    // 所谓一个状态的 ε 闭包就是从这个状态出发，仅通过 ε 转换就可以到达的所有状态的集合。
                    var epsilonClosure = avaliableState.RecurGetReachableStates().ToList();
                    // 把状态 avaliableState 从其 ε 闭包中排除出去。
                    epsilonClosure.Remove(avaliableState);

                    /* 复制所有有效转换到状态 avaliableState 。 */
                    var avaliableTransitions = epsilonClosure
                        .SelectMany(state => state.Transitions)
                        .Where(transition => !(transition is IEpsilonTransition))
                        .ToList();
                    foreach (var avaliableTransition in avaliableTransitions)
                        nfa.AttachTransition(avaliableState, avaliableTransition);

                    /* 移除状态 avaliableState 的所有 ε 转换。 */
                    // 与此同时，由于此状态机框架的实现方式：移除某个转换且其所指向的状态不为状态机任意可达转换的目标时，此状态不可达，即被排除于状态机外。
                    var epsilonTransitions = avaliableState.Transitions
                        .Where(transition => transition is IEpsilonTransition)
                        .ToList();
                    foreach (var epsilonTransition in epsilonTransitions)
                        nfa.RemoveTransition(avaliableState, epsilonTransition);

                    // 如果存在一个有效状态可以仅通过 ε 转换到达结束状态的话，那么这个状态应该被标记为结束状态。
                    if (epsilonClosure.Any(state => state.IsTerminal))
                        avaliableState.IsTerminal = true;
                }
            }
        }

        /// <summary>
        /// 消除 <see cref="INFA{TState, TTransition, TEpsilonTransition}"/> 中的所有 ε 闭包。
        /// </summary>
        public static void EpsilonClosure<TState, TTransition, TEpsilonTransition>(this INFA<TState, TTransition, TEpsilonTransition> nfa)
            where TState : IState<TTransition>
            where TTransition : class, ITransition<TState>
            where TEpsilonTransition : TTransition, IEpsilonTransition<TState>
        {
            if (nfa == null) throw new ArgumentNullException(nameof(nfa));

            if (nfa.StartState
                .RecurGetTransitions<TState, TTransition>()
                .Any(transition => transition is TEpsilonTransition)
            )
            {
                var states = nfa.States;
                // 计算有效状态
                var avaliableStates = states.Where(state =>
                    // 起始状态
                    object.Equals(state, nfa.StartState) ||
                    // 存在非 ε 转换的输入转换
                    states.SelectMany(_state => _state.Transitions)
                        .Where(transition => object.Equals(transition.Target, state))
                        .Any(transition => !(transition is TEpsilonTransition))
                ).ToList();

                foreach (var avaliableState in avaliableStates)
                {
                    /* 计算状态 avaliableState 的 ε 闭包。 */
                    // 所谓一个状态的 ε 闭包就是从这个状态出发，仅通过 ε 转换就可以到达的所有状态的集合。
                    var epsilonClosure = avaliableState.RecurGetReachableStates<TState, TTransition>().ToList();
                    // 把状态 avaliableState 从其 ε 闭包中排除出去。
                    epsilonClosure.Remove(avaliableState);

                    /* 复制所有有效转换到状态 avaliableState 。 */
                    var avaliableTransitions = epsilonClosure
                        .SelectMany(state => state.Transitions)
                        .Where(transition => !(transition is TEpsilonTransition))
                        .ToList();
                    foreach (var avaliableTransition in avaliableTransitions)
                        nfa.AttachTransition(avaliableState, avaliableTransition);

                    /* 移除状态 avaliableState 的所有 ε 转换。 */
                    // 与此同时，由于此状态机框架的实现方式：移除某个转换且其所指向的状态不为状态机任意可达转换的目标时，此状态不可达，即被排除于状态机外。
                    var epsilonTransitions = avaliableState.Transitions
                        .Where(transition => transition is TEpsilonTransition)
                        .ToList();
                    foreach (var epsilonTransition in epsilonTransitions)
                        nfa.RemoveTransition(avaliableState, epsilonTransition);

                    // 如果存在一个有效状态可以仅通过 ε 转换到达结束状态的话，那么这个状态应该被标记为结束状态。
                    if (epsilonClosure.Any(state => state.IsTerminal))
                        avaliableState.IsTerminal = true;
                }
            }
        }
        #endregion
    }
}
