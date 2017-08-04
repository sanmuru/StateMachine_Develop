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

            if (transition.Target.RecurGetReachableStates() is HashSet<IState> states)
            {
                states.Add(transition.Target);
                return states;
            }
            else return Enumerable.Empty<IState>();
        }

        private static void RecurGetReachableStatesInternal(IState startState, HashSet<IState> states)
        {
            foreach (var transition in startState.Transitions.Where(_transition => _transition != null && _transition.Target != null))
            {
                bool f = states.Add(transition.Target);

                if (f && transition is IEpsilonTransition)
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

            if (transition.Target.RecurGetReachableStates<TState, TTransition>() is HashSet<TState> states)
            {
                states.Add(transition.Target);
                return states;
            }
            else return Enumerable.Empty<TState>();
        }

        private static void RecurGetReachableStatesInternal<TState, TTransition>(TState startState, HashSet<TState> states)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            foreach (var transition in startState.Transitions.Where(_transition => _transition != null && _transition.Target != null))
            {
                bool f = states.Add(transition.Target);

                if (f && transition is IEpsilonTransition)
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
                transitions.Add(transition);
                if (transition is IEpsilonTransition)
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
                bool f = transitions.Add(t);

                if (f && t is IEpsilonTransition)
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
                transitions.Add(transition);
                if (transition is IEpsilonTransition)
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
                bool f = transitions.Add(t);

                if (f && t is IEpsilonTransition)
                    FSMUtility.RecurGetReachableTransitionsInternal<TState, TTransition>(transition, transitions);
            }
        }
        #endregion

#if DEBUG
        public static string GetStringInfo(this IFSM fsm)
        {
            var states = fsm.States.ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("StartState: ({0}), total: {1}{2}", states.IndexOf(fsm.StartState), states.Count, Environment.NewLine);
            sb.Append(string.Join(
                Environment.NewLine,
                (from state in states
                 from transition in state.Transitions
                 select string.Format("  ({0}) ---> ({1})", states.IndexOf(state), states.IndexOf(transition.Target))
                ).ToArray()
            ));

            return sb.ToString();
        }

        public static string GetStringInfo<TState, TTransition>(this IFSM<TState, TTransition> fsm)
            where TState : IState<TTransition>
            where TTransition : ITransition<TState>
        {
            var states = fsm.States.ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("StartState: ({0}), total: {1}{2}", states.IndexOf(fsm.StartState), states.Count, Environment.NewLine);
            sb.Append(string.Join(
                Environment.NewLine,
                (from state in states
                 from transition in state.Transitions
                 select string.Format("  ({0}) ---> ({1})", states.IndexOf(state), states.IndexOf(transition.Target))
                ).ToArray()
            ));

            return sb.ToString();
        }
#endif
    }
}
