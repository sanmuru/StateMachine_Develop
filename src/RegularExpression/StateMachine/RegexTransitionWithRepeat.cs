using SamLu.StateMachine.ObjectModel;
using System.Diagnostics;

namespace SamLu.RegularExpression.StateMachine;

internal class RegexTransitionWithRepeat<T> : RegexTransition<T>
{
    protected internal ulong? minimumCount;
    protected internal ulong? maximumCount;
    protected internal readonly bool isGreedy;

    public RegexTransitionWithRepeat(IEnumerable<InputEntry<T>> inputEntries,
        ulong? minimumCount,
        ulong? maximumCount,
        bool isGreedy) : base(inputEntries)
    {
        this.minimumCount = minimumCount;
        this.maximumCount = maximumCount;
        this.isGreedy = isGreedy;
    }

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
