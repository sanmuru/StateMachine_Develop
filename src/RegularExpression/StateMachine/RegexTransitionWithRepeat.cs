using SamLu.StateMachine.ObjectModel;
using System.Diagnostics;

namespace SamLu.RegularExpression.StateMachine;

internal class RegexTransitionWithRepeat<T>(IEnumerable<InputEntry<T>> inputEntries,
    ulong? minimumCount,
    ulong? maximumCount,
    bool isGreedy) : RegexTransition<T>(inputEntries)
{
    protected internal ulong? minimumCount = minimumCount;
    protected internal ulong? maximumCount = maximumCount;
    protected internal readonly bool isGreedy = isGreedy;

    internal override RegexTransition<T> WithRepeat(ulong? minimumCount = 0, ulong? maximumCount = null, bool isGreedy = true)
    {
        Debug.Assert(!maximumCount.HasValue || (minimumCount ?? 0) < maximumCount.Value);

        if (((this.minimumCount ?? 0) == (minimumCount ?? 0)) &&
            this.maximumCount == maximumCount &&
            this.isGreedy == isGreedy
            )
            return this;

        return base.WithRepeat(minimumCount, maximumCount, isGreedy);
    }
}
