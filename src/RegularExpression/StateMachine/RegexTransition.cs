using SamLu.StateMachine;
using SamLu.StateMachine.ObjectModel;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SamLu.RegularExpression.StateMachine;

public class RegexTransition<T> : Transition<T, RegexState<T>>
{
    private readonly bool isEpsilon;
    public override bool IsEpsilon => this.isEpsilon;

    protected IEnumerable<InputEntry<T>> inputEntries;
    public override ImmutableArray<InputEntry<T>> InputEntries => this.inputEntries.ToImmutableArray();

    public RegexTransition() => this.isEpsilon = true;

    public RegexTransition(IEnumerable<InputEntry<T>> inputEntries)
    {
        this.isEpsilon = false;
        this.inputEntries = inputEntries;
    }

    internal virtual RegexTransition<T> WithRepeat(ulong? minimumCount = 0, ulong? maximumCount = null, bool isGreedy = true)
    {
        Debug.Assert(!maximumCount.HasValue || (minimumCount ?? 0) < maximumCount.Value);

        if (this.isEpsilon)
            return this;

        return new RegexTransitionWithRepeat<T>(this.inputEntries, minimumCount, maximumCount, isGreedy)
        {
            target = this.target
        };
    }
}
