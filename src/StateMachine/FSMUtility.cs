using SamLu.StateMachine.ObjectModel;
using System.Collections.Immutable;

namespace SamLu.StateMachine;

/// <summary>
/// 提供了一系列操作、获取有关有限状态机及其部件信息的静态方法。
/// </summary>
public static class FSMUtility
{
    /// <summary>
    /// 消除不确定的有限状态机中的所有 ε 闭包。
    /// </summary>
    public static void EpsilonClosure(this INFA nfa)
    {
        ArgumentNullExceptionExtension.ThrowIfNull(nfa);

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

    internal sealed class UnionState(IEnumerable<IState> innerStates) : IEquatable<UnionState>
    {
        public ImmutableHashSet<IState> InnerStates { get; } = [.. innerStates];
        public HashSet<UnionTransition> Transitions { get; } = [];

        public bool Equals(UnionState? other) => other is not null && this.InnerStates.SetEquals(other.InnerStates);

        public override bool Equals(object? obj) => this.Equals(obj as UnionState);

        public override int GetHashCode() => this.InnerStates.GetHashCode();

        private IState? result = null;
        public IState Build(INFA builder)
        {
            if (result is null)
            {
                result = builder.CreateDFAState(this.InnerStates);
                foreach (var t in this.Transitions)
                    result.AttachTransition(t.Build(builder));
            }
            return result;
        }
    }

    internal sealed class UnionTransition(IEnumerable<InputEntry> inputEntries, IEnumerable<ITransition> innerTransitions) : IEquatable<UnionTransition>
    {
        public ImmutableArray<InputEntry> InputEntries { get; } = [.. inputEntries];
        public ImmutableHashSet<ITransition> InnerTransitions { get; } = [.. innerTransitions];
        public UnionState? Target { get; set; }

        public bool Equals(UnionTransition? other) => other is not null && this.InnerTransitions.SetEquals(other.InnerTransitions);

        public override bool Equals(object? obj) => this.Equals(obj as UnionTransition);

        public override int GetHashCode() => this.InnerTransitions.GetHashCode();

        private ITransition? result = null;
        public ITransition Build(INFA builder)
        {
            if (result is null)
            {
                result = builder.CreateDFATransition(this.InputEntries, this.InnerTransitions);
                if (this.Target is not null)
                    result.SetTarget(this.Target.Build(builder));
            }
            return result;
        }
    }

    /// <summary>
    /// 执行不确定的有限状态机的确定化处理，返回确定化后的初始确定的有限状态机的起始状态。
    /// </summary>
    /// <param name="nfa">要执行确定化处理的不确定的有限状态机。</param>
    /// <param name="provider">用于匹配输入符号的提供者。</param>
    /// <returns>确定化得到的确定的有限状态机的起始状态。</returns>
    public static IState Determine(this INFA nfa, IInputSymbolProvider provider)
    {
        nfa.EpsilonClosure();

        Queue<UnionState> processing = new();
        Dictionary<UnionState, UnionState> processed = [];

        UnionState start, current;
        start = new([nfa.StartState]);
        processing.Enqueue(start);
        while (processing.Count > 0)
        {
            current = processing.Dequeue();
            var entries = provider.SplitEntries(
                from s in current.InnerStates
                from t in s.Transitions
                from e in t.InputEntries select e
            );
            var groups =
                from entry in entries
                let correspondingTransitions = (
                    from s in current.InnerStates
                    from t in s.Transitions
                    where provider.Contains(t.InputEntries, entry)
                    select t
                ).Distinct()
                let correspondingStates = (
                    from t in correspondingTransitions
                    select t.Target
                ).Distinct()
                let unionStates = new UnionState(correspondingStates)
                group (entry: entry, transitions: correspondingTransitions) by unionStates;
            foreach (var group in groups)
            {
                if (!processed.TryGetValue(group.Key, out var next))
                {
                    next = group.Key;
                    processed.Add(next, next);
                    processing.Enqueue(next);
                }
                var transition = new UnionTransition(
                    group.Select(t => t.entry).Distinct(new InputEntryEqualityComparer(provider)),
                    group.SelectMany(t => t.transitions)
                );
                current.Transitions.Add(transition);
                transition.Target = next;
            }
        }

        return start.Build(nfa);
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
        ArgumentNullExceptionExtension.ThrowIfNull(startState);

        HashSet<IState> states = [];
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
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

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
        ArgumentNullExceptionExtension.ThrowIfNull(startState);

        HashSet<ITransition> transitions = [];
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
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        HashSet<ITransition> transitions = [];
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
        ArgumentNullExceptionExtension.ThrowIfNull(startState);

        HashSet<IState> states = [];
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
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        if (transition.Target is null) throw new TargetNullException();
        HashSet<IState> states = [transition.Target];
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
        ArgumentNullExceptionExtension.ThrowIfNull(startState);

        HashSet<ITransition> transitions = [];
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
        ArgumentNullExceptionExtension.ThrowIfNull(transition);

        HashSet<ITransition> transitions = [];
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
