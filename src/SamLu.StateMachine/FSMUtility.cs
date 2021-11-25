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
        /// <summary>
        /// 消除不确定的有限状态机中的所有 ε 闭包。
        /// </summary>
        public static void EpsilonClosure( this INFA nfa)
        {
            ArgumentNullException.ThrowIfNull(nfa);

            if (nfa.StartState
                .RecurGetTransitions()
                .Any(transition => transition.IsEpsilon)
            )
            {
                // 获取所有状态作为样本。
                var states = nfa.StartState.RecurGetStates();
                // 计算有效状态
                var avaliableStates = states.Where(state =>
                    // 起始状态
                    object.Equals(state, nfa.StartState) ||
                    // 被非 ε 转换的输入转换指向的状态
                    states.SelectMany(_state => _state.Transitions)
                        .Where(transition => object.Equals(transition.Target, state))
                        .Any(transition => !transition.IsEpsilon)
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
                        .Where(transition => !transition.IsEpsilon)
                        .ToArray();
                    foreach (var avaliableTransition in avaliableTransitions)
                        avaliableState.AttachTransition(avaliableTransition);

                    /* 移除状态 avaliableState 的所有 ε 转换。 */
                    // 与此同时，由于此状态机框架的实现方式：移除某个转换且其所指向的状态不为状态机任意可达转换的目标时，此状态不可达，即被排除于状态机外。
                    var epsilonTransitions = avaliableState.Transitions
                        .Where(transition => transition.IsEpsilon)
                        .ToList();
                    foreach (var epsilonTransition in epsilonTransitions)
                        avaliableState.RemoveTransition(epsilonTransition);

                    // 如果存在一个有效状态可以仅通过 ε 转换到达结束状态的话，那么这个状态应该被标记为结束状态。
                    if (epsilonClosure.Any(state => state.IsTerminal))
                        avaliableState.IsTerminal = true;
                }
            }
        }

        #region RecurGetStates
        /// <summary>
        /// 递归获取指定起始状态开始能达到的所有状态的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态。</param>
        /// <returns>指定起始状态开始能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<IState> RecurGetStates(this IState startState)
        {
            ArgumentNullException.ThrowIfNull(startState);

            HashSet<IState> states = new();
            FSMUtility.RecurGetStatesInternal(startState, states);

            return states;
        }

        /// <summary>
        /// 递归获取指定转换开始能达到的所有状态的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        /// <exception cref="TargetNullException"><paramref name="transition"/> 指向的状态为 <see langword="null"/> 。</exception>
        public static IEnumerable<IState> RecurGetStates(this ITransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            if (transition.Target is null) throw new TargetNullException();
            return transition.Target.RecurGetStates();
        }

        private static void RecurGetStatesInternal(IState startState, HashSet<IState> states)
        {
            if (states.Add(startState))
            {
                foreach (var transition in startState.Transitions)
                {
                    if (transition.Target is null) throw new TargetNullException();
                    FSMUtility.RecurGetStatesInternal(transition.Target, states);
                }
            }
        }
        #endregion

        #region RecurGetTransitions
        /// <summary>
        /// 递归获取指定起始状态开始能经到的所有转换的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态</param>
        /// <returns>指定起始状态开始能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<ITransition> RecurGetTransitions(this IState startState)
        {
            ArgumentNullException.ThrowIfNull(startState);

            HashSet<ITransition> transitions = new();
            foreach (var transition in startState.Transitions)
                transitions.UnionWith(transition.RecurGetTransitions());

            return transitions;
        }

        /// <summary>
        /// 递归获取指定转换开始能经到的所有转换的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<ITransition> RecurGetTransitions(this ITransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            HashSet<ITransition> transitions = new();
            FSMUtility.RecurGetTransitionsInternal(transition, transitions);

            return transitions;
        }

        private static void RecurGetTransitionsInternal(ITransition transition, HashSet<ITransition> transitions)
        {
            if (transitions.Add(transition))
            {
                if (transition.Target is not null)
                {
                    foreach (var trans in transition.Target.Transitions.Where(t => t is not null))
                        FSMUtility.RecurGetTransitionsInternal(trans, transitions);
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
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<IState> RecurGetReachableStates(this IState startState)
        {
            ArgumentNullException.ThrowIfNull(startState);

            HashSet<IState> states = new();
            FSMUtility.RecurGetReachableStatesInternal(startState, states);

            return states;
        }

        /// <summary>
        /// 递归获取指定转换开始无须接受输入就能达到的所有状态的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始无须接受输入就能达到的所有状态的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<IState> RecurGetReachableStates(this ITransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            if (transition.Target is null) throw new TargetNullException();
            HashSet<IState> states = new() { transition.Target };
            FSMUtility.RecurGetReachableStatesInternal(transition.Target, states);

            return states;
        }

        private static void RecurGetReachableStatesInternal(IState startState, HashSet<IState> states)
        {
            foreach (var transition in startState.Transitions)
            {
                if (transition.Target is null) throw new TargetNullException();

                if (transition.IsEpsilon && states.Add(transition.Target))
                    FSMUtility.RecurGetReachableStatesInternal(transition.Target, states);
            }
        }
        #endregion

        #region RecurGetReachableTransitions
        /// <summary>
        /// 递归获取指定起始状态开始无须接受输入就能经到的所有转换的集合。
        /// </summary>
        /// <param name="startState">指定的起始状态</param>
        /// <returns>指定起始状态开始无须接受输入就能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startState"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<ITransition> RecurGetReachableTransitions(this IState startState)
        {
            ArgumentNullException.ThrowIfNull(startState);

            HashSet<ITransition> transitions = new();
            foreach (var transition in startState.Transitions.Where(_transition => _transition is not null))
            {
                if (transitions.Add(transition) && transition.IsEpsilon)
                    transitions.UnionWith(transition.RecurGetReachableTransitions());
            }

            return transitions;
        }

        /// <summary>
        /// 递归获取指定转换开始无须接受输入就能经到的所有转换的集合。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>指定转换开始无须接受输入就能经到的所有转换的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public static IEnumerable<ITransition> RecurGetReachableTransitions(this ITransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            HashSet<ITransition> transitions = new();
            FSMUtility.RecurGetReachableTransitionsInternal(transition, transitions);

            return transitions;
        }

        private static void RecurGetReachableTransitionsInternal(ITransition transition, HashSet<ITransition> transitions)
        {
            if (transition.Target is null) throw new TargetNullException();

            foreach (var t in transition.Target.Transitions)
            {
                if (transitions.Add(t) && t.IsEpsilon)
                    FSMUtility.RecurGetReachableTransitionsInternal(transition, transitions);
            }
        }
        #endregion
    }
}
